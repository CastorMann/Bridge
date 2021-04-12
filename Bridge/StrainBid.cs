using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class StrainBid : Bid
    {
        public Strain strain;
        public int level;

        public StrainBid(Strain strain, int level)
        {
            this.strain = strain;
            this.level = level;
        }

        /// <summary> Gets the type of the bid, StrainBid </summary>
        /// <returns> 0, representing a StrainBid </returns>
        public int getType()
        {
            return 0;
        }
        /// <summary> Gets the value of the bid </summary>
        /// <returns> A string: the value of the Strain or NonStrain of the bid </returns>
        public string getValue()
        {
            return strain.value;
        }

        public int getLevel()
        {
            return level;
        }

        public bool compare(Bid other)
        {
            if (other == null)
            {
                return true;
            }
            if (level > other.getLevel())
            {
                return true;
            }
            if (level == other.getLevel())
            {
                string[] ranks = new string[5] { "C", "D", "H", "S", "NT" };
                return Array.IndexOf(ranks, strain.value) > Array.IndexOf(ranks, other.getValue());
            }
            return false;
        }

        public int getId()
        {
            return 3 + strain.getValue() + (level - 1) * 5;
        }

        public override string ToString()
        {
            return level.ToString() + strain.value;
        }
    }
}
