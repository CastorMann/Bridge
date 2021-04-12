using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Card
    {
        public Rank rank;
        public Suit suit;
        public bool isTrump;
        public int ID;
        public bool isPlayable;
        public int hcp;
        public int playedBy;

        public Card(Rank rank, Suit suit, bool isTrump = false, int ID = -1, bool isPlayable = false)
        {
            this.rank = rank;
            this.suit = suit;
            this.isTrump = isTrump;
            this.ID = ID == -1 ? 14 - rank.value + 13 * suit.ID : ID;
            this.isPlayable = isPlayable;
            hcp = Math.Max(0, (rank.value - 10));
            playedBy = -1;
        }

        /// <summary> Checks wether this card beats a specified other card </summary>
        /// <returns> true if this card beats the other card, otherwise false </returns>
        public bool compare(Card other, Suit suitOnLead)
        {
            if (other.suit.value != suitOnLead.value && !other.isTrump)
            {
                return true;
            }
            if (isTrump && !other.isTrump)
            {
                return true;
            }
            if (!isTrump && other.isTrump)
            {
                return false;
            }
            if (!isTrump)
            {
                if (suit.value != suitOnLead.value)
                {
                    return false;
                }
            }
            return rank.value > other.rank.value;
        }

        public void setPlayable(bool playable = true)
        {
            isPlayable = playable;
        }

        public void setTrump(bool trump = true)
        {
            isTrump = trump;
            suit.isTrump = trump;
        }

        public override string ToString()
        {
            return rank.ToString() + suit.ToString();
        }
    }
}
