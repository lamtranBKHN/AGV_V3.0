using System;
using System.Collections.Generic;
using AgvPlanningModule.DataType;
using System.Linq;

namespace AgvPlanningModule
{
    class DataProcessing
    {
        public const string UPDATE_DASHBOARD_TOPIC = "dashboardPodCarryingRequest";

        private AgvCommunication mqtt = new AgvCommunication();
        private Logging Log         = new Logging();
        private Database database   = new Database();
        public Dictionary<int, Map> m_mapList = new Dictionary<int, Map>();
        /// <summary>
        /// 
        /// </summary>
        public DataProcessing() 
        {
            m_mapList = database.UpdateMap();
        }

        ~DataProcessing() { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private Dictionary<string, string> ParseAgvDataIncomingCommandHeader(string rawData)
        {
            try
            {
                Dictionary<string, string> dataHeaderObject = new Dictionary<string, string>();
                if (rawData.Length % 2 != 0) throw new ArithmeticException("Recieved raw data is not in pairs.");

                // Parse Frame header
                dataHeaderObject.Add("framHead",    rawData.Substring(0, 2));
                dataHeaderObject.Add("commandCode", rawData.Substring(2, 2));
                dataHeaderObject.Add("frameLength", rawData.Substring(4, 2));
                dataHeaderObject.Add("agvId",       rawData.Substring(6, 2));
                dataHeaderObject.Add("timestamp",   rawData.Substring(8, 2));

                return dataHeaderObject;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
                return null;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="dataHeader"></param>
        /// <returns></returns>
        private bool ValidDataCommand(string rawData, Dictionary<string, string> dataHeader)
        {
            try
            {
                int actualLength = rawData.Length;
                int frameLengthInData = Int32.Parse(dataHeader["frameLength"]);
                if (actualLength != frameLengthInData) return false;
                else if (dataHeader["framHead"].ToUpper() != "AB") return false;
                else return true;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public Dictionary<string, string> ParseDataCommand(string rawData)
        {
            try
            {
                Dictionary<string, string> parsedData = new Dictionary<string, string>();
                var frameHeader = ParseAgvDataIncomingCommandHeader(rawData);
                if (!ValidDataCommand(rawData, frameHeader)) throw new ArithmeticException("Invalid command data");
                parsedData = frameHeader;

                //string currentTime;
                switch (Int32.Parse(frameHeader["commandCode"]))
                {
                    case 1:     // New RFID
                        {                           
                            Log.PrintAndWriteLog($"Recieved meet new RFID command : {rawData}");
                            parsedData["agvId"] = rawData.Substring(6, 2);
                            parsedData["rfid"] = rawData.Substring(18, 8);

                            // Update to database
                            UpdateAgvPositon(Int32.Parse(parsedData["agvId"]), parsedData["rfid"]);
                            mqtt.Publish(UPDATE_DASHBOARD_TOPIC, "UPDATE_AGV_POSITION"); // Just ping
                        }
                        break;
                    case 2:     // Login
                        {
                            Log.PrintAndWriteLog($"Recieved login command : {rawData}");

                            parsedData["agvId"] = rawData.Substring(6, 2);
                            parsedData["password"] = rawData.Substring(18, 2);

                            // TODO: Check password

                            // Set AGV status to "ON"
                            string agvId = parsedData["agvId"];
                            database.Update("agv", "status", "1", "id", agvId);
                            database.Update("agv", "isAvailable", "1", "id", agvId);

                            mqtt.Publish(UPDATE_DASHBOARD_TOPIC, "UPDATE_AGV_LOGIN"); // Just ping
                        }
                        break;
                    case 3:     // Heartbeat
                        {
                            Log.PrintAndWriteLog($"Recieved heartbeat command : {rawData}");
                            parsedData["agvId"] = rawData.Substring(6, 2);
                            parsedData["batteryLevel"] = rawData.Substring(18, 2);
                            parsedData["rfid"] = rawData.Substring(20, 8);

                            string agvId = parsedData["agvId"];
                            string agvBatteryLevel = parsedData["batteryLevel"];
                            database.Update("agv", "batt", agvBatteryLevel, "id", agvId);
                        }
                        break;
                    case 4:     // Failure 
                        {
                            Log.PrintAndWriteLog($"Recieved failure command : {rawData}");
                            parsedData["failureCode"] = rawData.Substring(18, 2);

                            // TODO: Check battery

                            mqtt.Publish(UPDATE_DASHBOARD_TOPIC, "UPDATE_AGV_FAILURE"); // Just ping
                        }
                        break;
                }
                return parsedData;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Format rfid string
        /// </summary>
        /// <param name="rawRfid"></param>
        /// <returns></returns>
        private string FormatRfid(string rawRfid)
        {
            try
            {
                rawRfid = String.Format("{0} {1} {2} {3} {4} {5} {6}", rawRfid.Substring(0, 2), rawRfid.Substring(2, 2), rawRfid.Substring(4, 2), rawRfid.Substring(6, 2), rawRfid.Substring(8, 2), rawRfid.Substring(10, 2), rawRfid.Substring(12, 2));
                return rawRfid;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
                return null;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rfid"></param>
        /// <returns></returns>
        private Nullable<int> FindNodeIdFromRfid(string rfid)
        {
            try
            {
                for (int i = 0; i < m_mapList.Count(); i++)
                {
                    if (m_mapList[i].Rfid == rfid)
                    {
                        return m_mapList[i].Id;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agvId"></param>
        /// <param name="rfid"></param>
        private void UpdateAgvPositon(int agvId, string rfid)
        {
            try
            {
                rfid = FormatRfid(rfid);
                int? currentNodeId = FindNodeIdFromRfid(rfid);
                if (currentNodeId == null) throw new ArithmeticException($"Can't find node ID from {rfid} RFID");
                else
                {
                    database.Update("agv", "rfid", $"{currentNodeId}", "id", $"{currentNodeId}");
                }
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
        }
    }
}
