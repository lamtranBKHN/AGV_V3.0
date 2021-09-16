using System;
using System.Collections.Generic;
using System.Text;

namespace AgvPlanningModule.DataType
{
    public class Barrier
    {
        private int id;

        private int nodeId;
        public int Id { get => id; set => id = value; }
        public int NodeId { get => nodeId; set => nodeId = value; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nodeId"></param>
        public Barrier(int id, int nodeId)
        {
            this.id = id;
            this.nodeId = nodeId;
        }
        ~Barrier() { }
    }
}
