using System;
using System.Collections.Generic;
using System.Text;
using AgvPlanningModule.DataType;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Linq;

namespace AgvPlanningModule
{
    class AgvController : AgvCommunication
    {
        // Define MQTT Topic
        public const string UPDATE_DASHBOARD_TOPIC = "dashboardPodCarryingRequest";
        public const string POD_MOVING_REQUEST_TOPIC = "dashboardPodCarryingRequest";
        public const string DASHBOARD_NOTIFYCATION = "dashboardNotifycation";
        public const string AGV_HANDLER = "agvHandler";

        Database database = new Database();
        private Dictionary<int, Agv> m_agvList  = new Dictionary<int, Agv>();        
        PathFinding pathFinding = new PathFinding();
        DataProcessing dataProcessing = new DataProcessing();
        private Logging Log = new Logging();
        private PackageDataCommand packageCommandCreator = new PackageDataCommand();

        public AgvController() : base()
        {
            try
            {
                m_agvList = database.UpdateAgv();
                this.m_client.MqttMsgPublishReceived += handleCarryPodRequest;
                this.m_client.MqttMsgPublishReceived += handleAgvIncomingData;
                ConnectToAllAgv();
                // Connect to Dashboard
                SubcribeMsg(UPDATE_DASHBOARD_TOPIC);
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConnectToAllAgv()
        {
            foreach (int key in this.m_agvList.Keys)
            {
                string agvId = key.ToString();
                Log.PrintAndWriteLog($"Connecting to AGV {agvId} ...");
                Log.PrintAndWriteLog($"Done");
            }
            SubcribeMsg(AGV_HANDLER);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleCarryPodRequest(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                if (e.Topic != POD_MOVING_REQUEST_TOPIC) return;
                //string currentTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

                int podId = Int32.Parse(Encoding.UTF8.GetString(e.Message).Split("_")[0]);
                int endNode = Int32.Parse(Encoding.UTF8.GetString(e.Message).Split("_")[1]);

                Dictionary<int, Pod> podList = database.UpdatePod();
                int podNodeId = podList[podId].NodeId;

                Dictionary<int, Agv> agvList = database.UpdateAgv();

                Log.PrintAndWriteLog($"Received request carrying POD {podId} to node {endNode}");
                Log.PrintAndWriteLog("Finding nearest available AGV ...");

                // Calculate distance from this pod to all available AGV
                Dictionary<int, double> distanceToAllAgv = pathFinding.FindDistanceToAllAgv(podId);

                // Check if there is no available agv
                if (distanceToAllAgv.Count == 0)
                {
                    string warningError = "There is no available AGV at the moment. Please try again later.";
                    Log.PrintAndWriteLog(warningError);
                    Publish(DASHBOARD_NOTIFYCATION, warningError);
                    return;
                }

                // Find minimum distance from pod to agv
                double minDistance = distanceToAllAgv.Values.Min();
                // Get agv id from minimum distance
                int selectedAgvId = distanceToAllAgv.FirstOrDefault(x => x.Value == minDistance).Key;

                Log.PrintAndWriteLog($"Select nearest AGV {selectedAgvId} which is {minDistance} away from POD {podId}");

                // Find best route from agv to pod
                int agvNodeId = agvList[selectedAgvId].NodeId;
                List<int> planAgvToPod = pathFinding.FindRoute(agvNodeId, podNodeId, false);
                // Find best route to destination
                List<int> planPodtoEnd = pathFinding.FindRoute(podNodeId, endNode, false);

                var commandHexPackage = packageCommandCreator.CreateActionPackageToAgv(selectedAgvId, planAgvToPod, planPodtoEnd);
                // Update dashboard
                Publish("dashboardControllerPodMoving", packageCommandCreator.UpdateDashboardRoute(planAgvToPod, planPodtoEnd));
                // Send command to AGV
                Publish($"agvWorker{selectedAgvId}", commandHexPackage);
                // Set agv is busy
                database.Update("agv", "isAvailable", "0", "id", selectedAgvId.ToString());
                return;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
        }

        private void handleAgvIncomingData(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                if (e.Topic != AGV_HANDLER) return;
                string rawData = Encoding.UTF8.GetString(e.Message);

                var parsedData = dataProcessing.ParseDataCommand(rawData);
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }           
        }

        ~AgvController() { }        
    }
}
