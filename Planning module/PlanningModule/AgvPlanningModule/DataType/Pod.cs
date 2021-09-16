using System;
using System.Collections.Generic;
using System.Text;

namespace AgvPlanningModule.DataType
{
    public class Pod
    {
        private int id;

        private int productId;

        private int quantity;

        private int nodeId;

        private int load;

        public int Id { get => id; set => id = value; }
        public int NodeId { get => nodeId; set => nodeId = value; }
        public int Load { get => load; set => load = value; }
        public int ProductId { get => productId; set => productId = value; }
        public int Quantity { get => quantity; set => quantity = value; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goods"></param>
        /// <param name="goodInfo"></param>
        /// <param name="nodeId"></param>
        /// <param name="load"></param>
        public Pod(int id, int productId, int quantity, int nodeId, int load)
        {
            this.id         = id;
            this.productId  = productId;
            this.quantity   = quantity;
            this.nodeId     = nodeId;
            this.load       = load;
        }
        ~Pod() { }
    }
}
