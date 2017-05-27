using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term_Project_John_Stone
{
    class xyPosition
    {
        private int x;
        private int y;
        private float size;
        private xyPosition head;

        public xyPosition(int xPos, int yPos)
        {
            x = xPos;
            y = yPos;
        }
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public void setH(xyPosition z)
        {
            head = z;
        }

        public xyPosition getH()
        {
            return head;
        }

        public bool equaling(xyPosition t)
        {
            if (this.head == t)
            {
                return true;
            }
            else
                return false;
        }
    }
}
