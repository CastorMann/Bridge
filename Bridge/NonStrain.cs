using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class NonStrain
    {
        public string value;

        public NonStrain(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }
    }
}
