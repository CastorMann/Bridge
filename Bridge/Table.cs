using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Table
    {
        public LinkedList<Deal> deals;
        public LinkedListNode<Deal> activeDeal;
        public Player north;
        public Player east;
        public Player south;
        public Player west;
        public Player[] players;
        public int id;
        public int errorMargin = 0;

        public Table(int id)
        {
            deals = new LinkedList<Deal>();
            activeDeal = null;
            this.id = id;
            north = new Player(this, 0);
            east = new Player(this, 1);
            south = new Player(this, 2);
            west = new Player(this, 3);
            players = new Player[4] { north, east, south, west };

        }
        //TODO: FIX ERROR WITH deals = EMPTY ARRAY
        public Table(int id, Deal[] deals)
        {
            this.deals = new LinkedList<Deal>();
            foreach (Deal deal in deals)
            {
                this.deals.AddLast(deal);
            }
            activeDeal = deals.Length == 0 ? null : this.deals.First;
            this.id = id;
            north = new Player(this, 0);
            east = new Player(this, 1);
            south = new Player(this, 2);
            west = new Player(this, 3);
        }

        public Deal addDeal()
        {
            Console.WriteLine("Adding Deal - no argument");
            Deal deal = new Deal(deals.Count == 0 ? 1 : deals.Last.Value.id + 1);
            deal.shuffle();
            deals.AddLast(deal);
            if (activeDeal == null)
            {
                activeDeal = deals.First;
            }
            Console.WriteLine("Table currently has " + deals.Count + " deals");
            deal.print();
            return deal;
        }
        public void addDeal(Deal deal)
        {
            deals.AddLast(deal);
            if (activeDeal == null)
            {
                activeDeal = deals.First;
            }
        }
        public void addDeals(int nbrOfDeals)
        {
            for (int i = 0; i < nbrOfDeals; i++)
            {
                addDeal();
            }
            Console.WriteLine("Table currently has" + deals.Count + "deals");
        }

        public void addDeals(Deal[] deals)
        {
            foreach (Deal deal in deals)
            {
                addDeal(deal);
            }
        }

        /// <summary> Sets active deal to the next deal in the list. If no next deal exists a new deal is added </summary>
        /// <returns> true if a new deal was added, otherwise false </returns>
        public bool next()
        {
            if (activeDeal.Next == null)
            {
                Console.WriteLine("Last deal reached - adding new (TABLE)");
                Deal deal = new Deal(activeDeal.Value.id + 1);
                addDeal(deal);
                Console.WriteLine("Table currently has" + deals.Count + "deals");
                activeDeal = activeDeal.Next;
                return true;
            }
            activeDeal = activeDeal.Next;
            return false;
        }

        public void playCard(int cardID)
        {
            Console.WriteLine("Playing Card: " + BRIDGE.CARDS[cardID]);
            bool followedSuit = activeDeal.Value.playCard(cardID);
            Card card = activeDeal.Value.deck.cards[cardID];
            foreach (Player player in players)
            {
                player.updateData(card, card.playedBy);
                if (!followedSuit)
                {
                    int suitOnLead = -1;
                    if (activeDeal.Value.lastTrick.Count > 0)
                    {
                        suitOnLead = activeDeal.Value.lastTrick.First.Value.suit.ID;
                    }
                    else
                    {
                        suitOnLead = activeDeal.Value.playedCards.Last.Previous.Previous.Previous.Value.suit.ID;
                    }
                    //Console.WriteLine("Player did not follow suit");
                    player.maxCards[card.playedBy][suitOnLead] = player.minCards[card.playedBy][suitOnLead];
                    //Console.WriteLine($"Player {card.playedBy} must have initially held {player.minCards[card.playedBy][suitOnLead]} cards in suit on lead ({suitOnLead})");
                }
            }
            
        }
    }
}
