using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Strain
    {
        public string value;

        public Strain(string value)
        {
            this.value = value;
        }

        public int getValue()
        {
            return value == "C" ? 0 : value == "D" ? 1 : value == "H" ? 2 : value == "S" ? 3 : 4;
        }

        public override string ToString()
        {
            return value;
        }
    }
}
