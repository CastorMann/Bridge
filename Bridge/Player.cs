using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Player
    {
        public int id;
        public Hand hand;
        public Table table;

        // Information about current deal
        public int[] data;
        public int[] maxHcp;
        public int[] minHcp;
        public int[][] maxCards;
        public int[][] minCards;
        // ------------------------------
        public Player(Table table, int id)
        {
            this.id = id;
            hand = new Hand();
            this.table = table;
            resetData();
            
        }

        public Player(Table table, Card[] cards, int id)
        {
            this.id = id;
            hand = new Hand(cards);
            this.table = table;
            resetData();
            
        }

        public void play(int cardID)
        {
            table.playCard(cardID);
        }

        public void AIPlay(int skillLevel = 100)
        {
            int errorMargin = 0;
            if (id == (table.activeDeal.Value.contract.getDeclarer() + 2) % 4)
            {
                table.players[(id + 2) % 4].AIPlay(skillLevel);
                return;
            }
            //Console.WriteLine("Starting AI play from player: " + id);
            //Console.WriteLine("Player on Lead: " + table.activeDeal.Value.playerOnTurn);
            Dictionary<string, int> results = new Dictionary<string, int>();
            Deal myDeal = new Deal(table.activeDeal.Value.id);
            for (int i = 0; i < skillLevel; i++)
            {
                //Console.WriteLine("Predealing...");
                myDeal.predeal(data);
                while(checkEligibility(myDeal))
                {
                    //Console.WriteLine("Deal was not eligible, retrying...");
                    myDeal.predeal(data);
                }
                myDeal.setPlayerOnTurn((myDeal.id + 3) % 4);
                myDeal.contract = table.activeDeal.Value.contract;
                myDeal.playedCards.Clear();
                myDeal.lastTrick.Clear();
                DoubleDummySolver solver = new DoubleDummySolver(myDeal);
                foreach (Card card in table.activeDeal.Value.playedCards)
                {
                    myDeal.playCard(card.ID);
                }
                solver.exec(string.Join(" ", myDeal.playedCards));
                foreach (Card card in myDeal.deck.cards)
                {
                    if (card.isPlayable)
                    {
                        if (!results.ContainsKey(card.ToString()))
                        {
                            results.Add(card.ToString(), solver.getTricksEx(card.ToString()));
                        }
                        else
                        {
                            results[card.ToString()] += solver.getTricksEx(card.ToString());
                        }
                    }
                }
                solver.destroy();
            }
            int max = -1;
            string cardToPlay = "";
            foreach (string card in results.Keys)
            {
                if (results[card] >= max)
                {
                    cardToPlay = card;
                    max = results[card];
                }
            }
            DoubleDummySolver s = new DoubleDummySolver(table.activeDeal.Value);
            errorMargin += s.getTricks() - s.getTricksEx(cardToPlay);
            s.destroy();
            play(Array.IndexOf(BRIDGE.CARDS, cardToPlay));
            //Console.WriteLine("AI plays: " + cardToPlay);
            table.errorMargin += errorMargin;
            //table.activeDeal.Value.print();
            
        }
        /// <summary>
        /// Checks if myDeal meets requirements for current known data
        /// </summary>
        /// <param name="myDeal"> The deal to be checked </param>
        /// <returns>false if all requirements are met, otherwise true</returns>
        private bool checkEligibility(Deal myDeal)
        {
            for (int i = 0; i < 4; i++)
            {
                if (myDeal.deck.hcp(i) > maxHcp[i] || myDeal.deck.hcp(i) < minHcp[i])
                {
                    //Console.WriteLine("Deal is not eligible because hcp is out of bounds: " + myDeal.deck.hcp(i));
                    return true;
                }
                for (int j = 0; j < 4; j++)
                {
                    if (myDeal.deck.nbrOfCards(i, j) > maxCards[i][j] || myDeal.deck.nbrOfCards(i, j) < minCards[i][j])
                    {
                        //Console.WriteLine("Deal is not eligible because #cards is out of bounds: " + myDeal.deck.nbrOfCards(i, j));
                        return true;
                    }
                }
            }
            
            return false;
        }

        public void updateData()
        {
            resetData();

            // My hand to data
            addHandToData(id);

            // Dummy to Data
            if (table.activeDeal.Value.playedCards.Count > 0)
            {
                addHandToData((table.activeDeal.Value.contract.getDeclarer() + 2) % 4);
            }

            // Played cards to data
            foreach (Card card in table.activeDeal.Value.playedCards)
            {
                updateData(card, card.playedBy);
            }
        }

        public void updateData(Card card, int player)
        {
            if (data[card.ID] != -1)
            {
                if (data[card.ID] != player)
                {
                    throw new Exception("Data does not match actual board");
                }
                return;
            }
            data[card.ID] = player;
            minCards[player][card.suit.ID]++;
            /*
            Console.WriteLine("Data Updated: ");
            foreach (int[] hand in minCards)
            {
                if (hand[0]+hand[1]+hand[2]+hand[3] > 13)
                {
                    throw new Exception("!!!!!!!!!!!!!!!!!!!!");
                }
                foreach (int nbrC in hand)
                {
                    Console.Write(nbrC + " ");
                }
                Console.WriteLine();
            }
            */
            
        }

        public void resetData()
        {
            data = new int[52];
            maxHcp = new int[4] { 37, 37, 37, 37 };
            minHcp = new int[4] { 0, 0, 0, 0 };
            maxCards = new int[4][] { 
                new int[4] { 13, 13, 13, 13 }, 
                new int[4] { 13, 13, 13, 13 }, 
                new int[4] { 13, 13, 13, 13 }, 
                new int[4] { 13, 13, 13, 13 } 
            };
            minCards = new int[4][] {
                new int[4] { 0, 0, 0, 0 },
                new int[4] { 0, 0, 0, 0 },
                new int[4] { 0, 0, 0, 0 },
                new int[4] { 0, 0, 0, 0 }
            };
            for (int i = 0; i < 52; i++)
            {
                data[i] = -1;
            }
        }

        public void addHandToData(int id)
        {
            for (int i = 0; i < 52; i++)
            {
                if (table.activeDeal.Value.deck.distribution[i] == id)
                {
                    updateData(table.activeDeal.Value.deck.cards[i], id);
                }
            }
        }
    }
}
