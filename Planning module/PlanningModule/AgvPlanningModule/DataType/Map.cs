using System;
using System.Collections.Generic;
using System.Text;

namespace AgvPlanningModule.DataType
{
    public class Map
    {
        private int id;

        private string rfid;

        private int x;

        private int y;

        private bool isBlocked;

        private bool isActive;

        public int Id { get => id; set => id = value; }
        public string Rfid { get => rfid; set => rfid = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public bool IsBlocked { get => isBlocked; set => isBlocked = value; }
        public bool IsActive { get => isActive; set => isActive = value; }

        public Map(int id, string rfid, int x, int y, bool isBlocked, bool isActive)
        {
            this.id = id;
            this.rfid = rfid;
            this.x = x;
            this.y = y;
            this.isBlocked = isBlocked;
            this.isActive = isActive;
        }

        ~Map() { }
    }
}
