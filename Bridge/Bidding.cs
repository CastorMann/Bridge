using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Bidding
    {
        public LinkedList<Bid> bidding;

        /// <summary> Empty constructor; Instatiates an empty bidding object </summary>
        public Bidding()
        {
            bidding = new LinkedList<Bid>();
        }

        /// <summary> Nonempty constructor; Instatiates bidding object with the specified bids </summary>
        /// <param name="bids"> An array of of objects of that implements the interface Bid, representing all bids in the sequence </param>
        public Bidding(Bid[] bids)
        {
            bidding = new LinkedList<Bid>();
            foreach (Bid bid in bids)
            {
                bidding.AddLast(bid);
            }
        }

        public Bidding(string format)
        {
            bidding = new LinkedList<Bid>();
            foreach (string i in format.Split("."))
            {
                if (i == "")
                {
                    continue;
                }
                makeBid(int.Parse(i));
            }
        }

        /// <summary> Adds a new bid to the bidding sequence </summary>
        /// <param name="bid"> The bid to be added to the sequence </param>
        /// <returns> true if the bid could be added, otherwise false </returns>
        public bool makeBid(Bid bid)
        {
            if (isValidBid(bid))
            {
                bidding.AddLast(bid);
                Console.WriteLine("Bid: " + bid.ToString());
                return true;
            }
            Console.WriteLine("Invalid Bid");
            return false;
        }

        public bool makeBid(int bidID)
        {
            return makeBid(BRIDGE.BIDS[bidID]);
        }

        /// <summary> Checks whether a specified bid is currently a valid bid to make </summary>
        /// <param name="bid"> The bid to check the validity of </param>
        /// <returns> true if the bid is valid, otherwise false </returns>
        public bool isValidBid(Bid bid)
        {
            if (isBiddingOver())
            {
                return false;
            }
            if (bid.getValue() == "P")
            {
                return true;
            }
            if (bidding.Count == 0)
            {
                return bid.getType() == 0;
            }
            if (bid.getValue() == "X")
            {
                if (bidding.Last.Value.getType() == 0)
                {
                    return true;
                }
                if (bidding.Count >= 3)
                {
                    return bidding.Last.Value.getValue() == "P" && bidding.Last.Previous.Value.getValue() == "P" && bidding.Last.Previous.Previous.Value.getType() == 0;
                }
                return false;
            }
            if (bid.getValue() == "XX")
            {
                if (bidding.Last.Value.getValue() == "X")
                {
                    return true;
                }
                if (bidding.Count >= 3)
                {
                    return bidding.Last.Value.getValue() == "P" && bidding.Last.Previous.Value.getValue() == "P" && bidding.Last.Previous.Previous.Value.getValue() == "X";
                }
                return false;
            }
            return bid.compare(getLastStrainBid());
        }

        /// <summary> Checks whether the bidding is completed or not </summary>
        /// <returns> true if the bidding is completed, otherwise false </returns>
        public bool isBiddingOver()
        {
            if (bidding.Count < 4)
            {
                return false;
            }
            return bidding.Last.Value.getValue() == "P" && bidding.Last.Previous.Value.getValue() == "P" && bidding.Last.Previous.Previous.Value.getValue() == "P";
        }

        /// <summary> Gets the final contract of the bidding </summary>
        /// <returns> The final contract of the bidding - an object of the class Contract </returns>
        public Contract getContract(int dealID)
        {
            Bid lastBid = getLastStrainBid();
            if (lastBid == null)
            {
                return null;
            }
            Bid modifier = bidding.Last.Previous.Previous.Previous.Value;
            bool isDoubled = modifier.getValue() == "X";
            bool isRedoubled = modifier.getValue() == "XX";
            int declarer = (dealID - 1) % 4;
            int idx = declarer;
            LinkedListNode<Bid> current = bidding.First;
            while (current.Value != lastBid)
            {
                current = current.Next;
                idx++;
            }
            int mod = idx % 2;
            idx = declarer;
            current = bidding.First;
            while (current.Value.getValue() != lastBid.getValue() || idx % 2 != mod)
            {
                current = current.Next;
                idx++;
                declarer++;
            }
            declarer %= 4;
            return new Contract(lastBid.getLevel(), new Strain(lastBid.getValue()), isDoubled, isRedoubled, true, declarer == 0 ? "N" : declarer == 1 ? "E" : declarer == 2 ? "S" : "W");

        }
        private Bid getLastStrainBid()
        {
            if (bidding.Count == 0)
            {
                return null;
            }
            LinkedListNode<Bid> current = bidding.Last;
            while (current.Value.getType() == 1)
            {
                current = current.Previous;
                if (current == null)
                {
                    return null;
                }
            }
            return current.Value;
        }

        public string format()
        {
            string res = "";
            if (bidding.Count == 0)
            {
                return res;
            }
            LinkedListNode<Bid> curr = bidding.First;
            while (curr.Next != null)
            {
                res += curr.Value.getId().ToString() + ".";
                curr = curr.Next;
            }
            res += curr.Value.getId().ToString();

            return res;
        }
    }
}
