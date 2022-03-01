using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Physics_Space_Program
{
    internal class PlanetValue
    {
        public Color textColor;
        public int value, planetID;

        public PlanetValue(int _pID, int _value, Color _textColor)
        {
            planetID = _pID;
            value = _value;
            textColor = _textColor;
        }
    }
}
