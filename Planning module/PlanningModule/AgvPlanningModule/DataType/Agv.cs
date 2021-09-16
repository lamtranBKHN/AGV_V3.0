using System;
using System.Collections.Generic;
using System.Text;

namespace AgvPlanningModule.DataType
{
    public class Agv
    {
        private int id;
        private string ip;
        private int port;
        private string camIp;
        private int orientation;
        private int currentPod;
        private int battery;
        private bool runningStatus;
        private int nodeId;
        private bool isAvailable;
        private int velocity;

        public int Id { get => id; set => id = value; }
        public string Ip { get => ip; set => ip = value; }
        public int Port { get => port; set => port = value; }
        public int Orientation { get => orientation; set => orientation = value; }
        public int CurrentPod { get => currentPod; set => currentPod = value; }
        public int Battery { get => battery; set => battery = value; }
        public bool RunningStatus { get => runningStatus; set => runningStatus = value; }
        public int NodeId { get => nodeId; set => nodeId = value; }
        public bool IsAvailable { get => isAvailable; set => isAvailable = value; }
        public int Velocity { get => velocity; set => velocity = value; }
        public string CamIp { get => camIp; set => camIp = value; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="orientation"></param>
        /// <param name="currentPod"></param>
        /// <param name="battery"></param>
        /// <param name="runningStatus"></param>
        /// <param name="nodeId"></param>
        /// <param name="isAvailable"></param>
        /// <param name="velocity"></param>
        public Agv(int id, string ip, int port, string camIp, int orientation, int currentPod, int battery, bool runningStatus, int nodeId, bool isAvailable, int velocity)
        {
            this.id             = id;
            this.ip             = ip;
            this.port           = port;
            this.camIp          = camIp;
            this.orientation    = orientation;
            this.currentPod     = currentPod;
            this.battery        = battery;
            this.runningStatus  = runningStatus;
            this.nodeId         = nodeId;
            this.isAvailable    = isAvailable;
            this.velocity       = velocity;
        }
        ~Agv() { }
    }
}
