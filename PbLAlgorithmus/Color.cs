using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbLAlgorithmus
{
    class Color
    {
        public static readonly Color BLACK = new Color(false, false, false);
        public static readonly Color RED = new Color(true, false, false);
        public static readonly Color GREEN = new Color(false, true, false);
        public static readonly Color BLUE = new Color(false, false, true);
        public static readonly Color CYAN = new Color(false, true, true);
        public static readonly Color PINK = new Color(true, false, true);
        public static readonly Color YELLOW = new Color(true, true, false);
        public static readonly Color WHITE = new Color(true, true, true);

        private Boolean Red;
        private Boolean Green;
        private Boolean Blue;

        private Color(Boolean red, Boolean green, Boolean blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public Byte[] ToByte()
        {
            Byte[] b = { Convert.ToByte(Red), Convert.ToByte(Green), Convert.ToByte(Blue) };
            return b;
        }
    }
}
