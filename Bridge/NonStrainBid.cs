using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class NonStrainBid : Bid
    {
        public NonStrain strain;

        public NonStrainBid(NonStrain strain)
        {
            this.strain = strain;
        }

        /// <summary> Gets the type of the bid, NonStrainBid </summary>
        /// <returns> 1, representing a NonStrainBid </returns>
        public int getType()
        {
            return 1;
        }

        public string getValue()
        {
            return strain.value;
        }

        public int getLevel()
        {
            return 0;
        }

        public bool compare(Bid other)
        {
            return true;
        }

        public int getId()
        {
            return strain.value == "P" ? 0 : strain.value == "X" ? 1 : 2;
        }

        public override string ToString()
        {
            return strain.value;
        }
    }
}
