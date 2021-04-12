using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    interface Bid
    {
        //* This interface is implemented by the class StrainBid and NonStrainBid

        /// <summary> Gets the type of the bid - StrainBid or NonStrainBid </summary>
        /// <returns> An integer: 0 --> StrainBid,  1 --> NonStrainBid </returns>
        int getType();
        string getValue();
        int getLevel();
        bool compare(Bid other);
        int getId();
        string ToString();
    }
}
