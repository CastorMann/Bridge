using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Deal
    {
        public int id;
        public Deck deck;
        public Bidding bidding;
        public Contract contract;
        public LinkedList<Card> playedCards;
        public LinkedList<Card> lastTrick;
        public int NSTricks;
        public int EWTricks;
        public int playerOnTurn;


        public Deal(int id)
        {
            deck = new Deck();
            this.id = id;
            bidding = new Bidding();
            playedCards = new LinkedList<Card>();
            lastTrick = new LinkedList<Card>();
            NSTricks = 0;
            EWTricks = 0;
            setPlayerOnTurn((id - 1) % 4);
        }

        public Deal(string format)
        {
            playedCards = new LinkedList<Card>();
            lastTrick = new LinkedList<Card>();
            string[] data = format.Split(" ");
            id = int.Parse(data[0]);
            deck = new Deck(data[1]);
            int idx = 0;
            foreach (string s in data[2].Split("."))
            {
                if (s == "")
                {
                    continue;
                }
                playedCards.AddLast(deck.cards[int.Parse(s)]);
                if (idx + (data[2].Split(".").Length % 4) >= data[2].Split(".").Length)
                {
                    lastTrick.AddLast(deck.cards[int.Parse(s)]);
                }
                idx++;
            }
            bidding = new Bidding(data[3]);
            contract = bidding.getContract(id);
            string[] ints = data[4].Split(".");
            NSTricks = int.Parse(ints[0]);
            EWTricks = int.Parse(ints[1]);
            setPlayerOnTurn(int.Parse(ints[2]));


        }


        public void shuffle()
        {
            deck.shuffle();
            setPlayerOnTurn(playerOnTurn);
        }

        public void predeal(int playerOnTurn, int northCards, int eastCards, int southCards, int westCards, 
            LinkedList<Card> playedCards, string north = "...", string east = "...", string south = "...", string west = "...")
        {
            deck.predeal(northCards, eastCards, southCards, westCards, playedCards, north, east, south, west);
            foreach (Card card in playedCards)
            {
                playCard(card.ID);
            }
            if (this.playerOnTurn != playerOnTurn)
            {
                throw new Exception("Wrong player on turn?");
            }
        }

        public void predeal(Deal deal, string north = "...", string east = "...", string south = "...", string west = "...")
        {
            predeal(deal.playerOnTurn, deal.deck.northHand.Count, deal.deck.eastHand.Count, deal.deck.southHand.Count,
                deal.deck.westHand.Count, deal.playedCards, north, east, south, west);
        }

        public void predeal(string north = "...", string east = "...", string south = "...", string west = "...")
        {
            deck.predeal(north, east, south, west);
        }

        public void predeal(int[] distribution)
        {
            deck.predeal(distribution);
        }

        public Deal copy()
        {
            return new Deal(format());
        }

        public bool makeBid(Bid bid)
        {
            bool isBid = bidding.makeBid(bid);
            if (!isBid)
            {
                return false;
            }
            playerOnTurn = (playerOnTurn + 1) % 4;
            if (bidding.isBiddingOver())
            {
                enterPlay();
            }
            return true;
        }

        public bool makeBid(int bidID)
        {
            return makeBid(BRIDGE.BIDS[bidID]);
        }

        public bool playCard(int cardID)
        {
            Card card = deck.cards[cardID];
            bool returnValue = lastTrick.Count == 0 ? true : card.suit.ID == lastTrick.First.Value.suit.ID;
            if (!card.isPlayable)
            {
                //throw new Exception("Error: Tried to play a card that is marked as unplayable: " + BRIDGE.CARDS[cardID]);
                //Console.WriteLine("[WARNING] PLAYING CARD THAT IS MARKED AS UNPLAYABLE!!!");
            }
            card.setPlayable(false);
            playedCards.AddLast(card);
            lastTrick.AddLast(card);
            deck.distribution[card.ID] = -1;
            card.playedBy = playerOnTurn;
            deck.hands[playerOnTurn].Remove(deck.cards[cardID]);
            nextPlayer();
            if (lastTrick.Count == 4)
            {
                evalTrick();
            }
            return returnValue;
        }

        public void undoLastPlayedCard()
        {
            if (playedCards.Count == 0)
            {
                throw new Exception("Cannot undo played card when no cards have been played");
            }
            if (lastTrick.Count == 0)
            {
                //TODO:
                throw new Exception("NOT YET IMPLEMENTED");
            }
            else
            {
                Card card = playedCards.Last.Value;
                playedCards.RemoveLast();
                lastTrick.RemoveLast();
                deck.distribution[card.ID] = (playerOnTurn + 3) % 4;
                setPlayerOnTurn((playerOnTurn + 3) % 4);
            }
        }


        /// <summary> Evaluates the current trick: first checks if all players have played a card
        /// then calculates the winner and updates the number of tricks taken and the player on turn </summary>
        public void evalTrick()
        {
            if (lastTrick.Count != 4)
            {
                Console.WriteLine("Error: wrong number of cards in last trick");
                return;
            }
            int w1 = lastTrick.First.Value.compare(lastTrick.First.Next.Value, lastTrick.First.Value.suit) ? 0 : 1;
            int w2 = lastTrick.Last.Value.compare(lastTrick.Last.Previous.Value, lastTrick.First.Value.suit) ? 3 : 2;
            int winner = (w1 == 0 ? lastTrick.First.Value : lastTrick.First.Next.Value).compare(w2 == 3 ? lastTrick.Last.Value : lastTrick.Last.Previous.Value, lastTrick.First.Value.suit) ? w1 : w2;
            setPlayerOnTurn((playerOnTurn + winner) % 4);
            if (playerOnTurn % 2 == 0)
            {
                NSTricks++;
            }
            else
            {
                EWTricks++;
            }
            lastTrick.Clear();
        }

        /// <summary> Checks if the bidding is over, then gets the contract and sets the player on turn </summary>
        public void enterPlay()
        {
            if (!bidding.isBiddingOver())
            {
                Console.WriteLine("Error: Tried to exit bidding stage while bidding is not yet over");
                return;
            }
            contract = bidding.getContract(id);
            setPlayerOnTurn((contract.getDeclarer() + 1) % 4);
        }

        public void setPlayerOnTurn(int playerOnTurn, Suit suitOnLead = null)
        {
            for (int i = 0; i < 52; i++)
            {
                deck.cards[i].setPlayable(deck.distribution[i] == playerOnTurn && (suitOnLead == null || deck.cards[i].suit.value == suitOnLead.value || !playerHasSuit(playerOnTurn, suitOnLead)));
            }
            this.playerOnTurn = playerOnTurn;
            //Console.WriteLine("Player On Turn: " + this.playerOnTurn);
        }

        public bool playerHasSuit(int player, Suit suit)
        {
            for (int i = suit.ID * 13; i < suit.ID * 13 + 13; i++)
            {
                if (deck.distribution[i] == player)
                {
                    return true;
                }
            }
            return false;
        }

        public void nextPlayer()
        {
            setPlayerOnTurn((playerOnTurn + 1) % 4, lastTrick.First.Value.suit);
        }

        public void print(bool original = false)
        {
            int[] arr = new int[4] { 0, 1, 2, 3 };
            int[] arr2 = new int[3] { 0, 3, 2 };
            foreach (int i in arr2) 
            {
                foreach (int j in arr)
                {
                    int offset = 13;
                    if (i == 0 || i == 2)
                    {
                        Console.Write("          ");
                    }
                    for (int k = 0; k < 13; k++)
                    {
                        if ((original ? deck.original_distribution[13 * j + k] : deck.distribution[13 * j + k]) == i)
                        {
                            Console.Write(deck.cards[13 * j + k].rank.representation);
                            offset--;
                        }
                    }
                    if (i == 3)
                    {
                        Console.Write("        ");
                        for (int x = 0; x < offset; x++)
                        {
                            Console.Write(" ");
                        }
                        for (int k = 0; k < 13; k++)
                        {
                            if ((original ? deck.original_distribution[13 * j + k] : deck.distribution[13 * j + k]) == i-2)
                            {
                                Console.Write(deck.cards[13 * j + k].rank.representation);
                            }
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        public override string ToString()
        {
            string res = "";
            int[] arr = new int[4] { 0, 1, 2, 3 };
            for (int i = 0; i < 4; i++)
            {
                foreach (int j in arr)
                {
                    for (int k = 0; k < 13; k++)
                    {
                        if (deck.original_distribution[13 * j + k] == i)
                        {
                           res += deck.cards[13 * j + k].rank.representation;
                        }
                    }
                    if (j != 3)
                    {
                        res += ".";
                    }
                }
                if (i != 3)
                {
                    res += " ";
                }
                
            }
            return res;
        }
    
        public string format()
        {
            string res = "";

            res += id.ToString() + " ";

            res += deck.format() + " ";

            if (playedCards.Count != 0)
            {
                LinkedListNode<Card> curr = playedCards.First;
                while (curr.Next != null)
                {
                    res += curr.Value.ID.ToString() + ".";
                    curr = curr.Next;
                }
                res += curr.Value.ID.ToString();
            }
            res += " ";

            res += bidding.format() + " ";

            res += $"{NSTricks}.{EWTricks}.{playerOnTurn}";
            Console.WriteLine(res);
            return res;
        }

    }
}
