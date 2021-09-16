using System;
using System.Collections.Generic;
using System.Text;

namespace AgvPlanningModule.DataType
{
    public class Charger
    {
        private int id;

        private int nodeId;

        private int orientation;

        private bool chargingStatus;

        private int agvId;

        public int Id { get => id; set => id = value; }
        public int NodeId { get => nodeId; set => nodeId = value; }
        public int Orientation { get => orientation; set => orientation = value; }
        public bool ChargingStatus { get => chargingStatus; set => chargingStatus = value; }
        public int AgvId { get => agvId; set => agvId = value; }

        public Charger(int id, int nodeId, int orientation, bool chargingStatus, int agvId)
        {
            this.id = id;
            this.nodeId = nodeId;
            this.orientation = orientation;
            this.chargingStatus = chargingStatus;
            this.agvId = agvId;
        }

        ~Charger() { }
    }
}
