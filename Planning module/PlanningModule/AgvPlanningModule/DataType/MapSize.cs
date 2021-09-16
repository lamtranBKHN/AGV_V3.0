using System;
using System.Collections.Generic;
using System.Text;

namespace AgvPlanningModule.DataType
{
    public class MapSize
    {
        private int height;
        private int width;

        public int Height { get => height; set => height = value; }
        public int Width { get => width; set => width = value; }

        public MapSize(int height, int width)
        {
            this.height = height;
            this.width = width;
        }
        ~MapSize() { }
    }
}
