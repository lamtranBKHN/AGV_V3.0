using System;
using System.Collections.Generic;
using System.Text;

namespace AgvPlanningModule.DataType
{
    public class WaitingZone
    {
        private int id;

        private int nodeId;

        private bool isInQuere;

        public int Id { get => id; set => id = value; }
        public int NodeId { get => nodeId; set => nodeId = value; }
        public bool IsInQuere { get => isInQuere; set => isInQuere = value; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nodeId"></param>
        /// <param name="isInQuere"></param>
        public WaitingZone(int id, int nodeId, bool isInQuere)
        {
            this.id = id;
            this.nodeId = nodeId;
            this.isInQuere = isInQuere;
        }
        ~WaitingZone() { }

    }
}
