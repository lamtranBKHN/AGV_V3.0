using System;
using System.Collections.Generic;
using System.Text;

namespace AgvPlanningModule.DataType
{
    public class Station
    {
        private int id;

        private int nodeId;

        private bool isAvailable;

        public int Id { get => id; set => id = value; }
        public int NodeId { get => nodeId; set => nodeId = value; }
        public bool IsAvailable { get => isAvailable; set => isAvailable = value; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nodeId"></param>
        /// <param name="isAvailable"></param>
        public Station(int id, int nodeId, bool isAvailable)
        {
            this.id = id;
            this.nodeId = nodeId;
            this.isAvailable = isAvailable;
        }

        ~Station() { }
    }
}
