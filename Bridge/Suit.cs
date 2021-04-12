using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Suit
    {
        public string value;
        public bool isTrump;
        public int ID;

        public Suit(string value, bool isTrump = false)
        {
            this.value = value;
            this.isTrump = isTrump;
            this.ID = value == "S" ? 0 : value == "H" ? 1 : value == "D" ? 2 : 3;
        }
        public override string ToString()
        {
            return value;
        }
    }
}
