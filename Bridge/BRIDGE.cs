using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    static class BRIDGE
    {
        public static StrainBid ONE_CLUB = new StrainBid(new Strain("C"), 1);
        public static StrainBid TWO_CLUBS = new StrainBid(new Strain("C"), 2);
        public static StrainBid THREE_CLUBS = new StrainBid(new Strain("C"), 3);
        public static StrainBid FOUR_CLUBS = new StrainBid(new Strain("C"), 4);
        public static StrainBid FIVE_CLUBS = new StrainBid(new Strain("C"), 5);
        public static StrainBid SIX_CLUBS = new StrainBid(new Strain("C"), 6);
        public static StrainBid SEVEN_CLUBS = new StrainBid(new Strain("C"), 7);

        public static StrainBid ONE_DIAMOND = new StrainBid(new Strain("D"), 1);
        public static StrainBid TWO_DIAMONDS = new StrainBid(new Strain("D"), 2);
        public static StrainBid THREE_DIAMONDS = new StrainBid(new Strain("D"), 3);
        public static StrainBid FOUR_DIAMONDS = new StrainBid(new Strain("D"), 4);
        public static StrainBid FIVE_DIAMONDS = new StrainBid(new Strain("D"), 5);
        public static StrainBid SIX_DIAMONDS = new StrainBid(new Strain("D"), 6);
        public static StrainBid SEVEN_DIAMONDS = new StrainBid(new Strain("D"), 7);

        public static StrainBid ONE_HEART = new StrainBid(new Strain("H"), 1);
        public static StrainBid TWO_HEARTS = new StrainBid(new Strain("H"), 2);
        public static StrainBid THREE_HEARTS = new StrainBid(new Strain("H"), 3);
        public static StrainBid FOUR_HEARTS = new StrainBid(new Strain("H"), 4);
        public static StrainBid FIVE_HEARTS = new StrainBid(new Strain("H"), 5);
        public static StrainBid SIX_HEARTS = new StrainBid(new Strain("H"), 6);
        public static StrainBid SEVEN_HEARTS = new StrainBid(new Strain("H"), 7);

        public static StrainBid ONE_SPADE = new StrainBid(new Strain("S"), 1);
        public static StrainBid TWO_SPADES = new StrainBid(new Strain("S"), 2);
        public static StrainBid THREE_SPADES = new StrainBid(new Strain("S"), 3);
        public static StrainBid FOUR_SPADES = new StrainBid(new Strain("S"), 4);
        public static StrainBid FIVE_SPADES = new StrainBid(new Strain("S"), 5);
        public static StrainBid SIX_SPADES = new StrainBid(new Strain("S"), 6);
        public static StrainBid SEVEN_SPADES = new StrainBid(new Strain("S"), 7);

        public static StrainBid ONE_NT = new StrainBid(new Strain("NT"), 1);
        public static StrainBid TWO_NT = new StrainBid(new Strain("NT"), 2);
        public static StrainBid THREE_NT = new StrainBid(new Strain("NT"), 3);
        public static StrainBid FOUR_NT = new StrainBid(new Strain("NT"), 4);
        public static StrainBid FIVE_NT = new StrainBid(new Strain("NT"), 5);
        public static StrainBid SIX_NT = new StrainBid(new Strain("NT"), 6);
        public static StrainBid SEVEN_NT = new StrainBid(new Strain("NT"), 7);

        public static NonStrainBid PASS = new NonStrainBid(new NonStrain("P"));
        public static NonStrainBid DOUBLE = new NonStrainBid(new NonStrain("X"));
        public static NonStrainBid REDOUBLE = new NonStrainBid(new NonStrain("XX"));

        public static Suit SPADES = new Suit("S");
        public static Suit HEARTS = new Suit("H");
        public static Suit DIAMONDS = new Suit("D");
        public static Suit CLUBS = new Suit("C");

        public static Bid[] BIDS = new Bid[] 
        {
            PASS, DOUBLE, REDOUBLE,
            ONE_CLUB, ONE_DIAMOND, ONE_HEART, ONE_SPADE, ONE_NT,
            TWO_CLUBS, TWO_DIAMONDS, TWO_HEARTS, TWO_SPADES, TWO_NT,
            THREE_CLUBS, THREE_DIAMONDS, THREE_HEARTS, THREE_SPADES, THREE_NT,
            FOUR_CLUBS, FOUR_DIAMONDS, FOUR_HEARTS, FOUR_SPADES, FOUR_NT,
            FIVE_CLUBS, FIVE_DIAMONDS, FIVE_HEARTS, FIVE_SPADES, FIVE_NT,
            SIX_CLUBS, SIX_DIAMONDS, SIX_HEARTS, SIX_SPADES, SIX_NT,
            SEVEN_CLUBS, SEVEN_DIAMONDS, SEVEN_HEARTS, SEVEN_SPADES, SEVEN_NT
        };

        public static string[] CARDS = new string[]
        {
            "AS", "KS", "QS", "JS", "TS", "9S", "8S", "7S", "6S", "5S", "4S", "3S", "2S",
            "AH", "KH", "QH", "JH", "TH", "9H", "8H", "7H", "6H", "5H", "4H", "3H", "2H",
            "AD", "KD", "QD", "JD", "TD", "9D", "8D", "7D", "6D", "5D", "4D", "3D", "2D",
            "AC", "KC", "QC", "JC", "TC", "9C", "8C", "7C", "6C", "5C", "4C", "3C", "2C"
        };
    }
}
