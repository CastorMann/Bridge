using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Deck
    {
        public Card[] cards;
        public int[] distribution;
        public int[] original_distribution;
        public List<Card> cardList;
        public List<Card> northHand;
        public List<Card> eastHand;
        public List<Card> southHand;
        public List<Card> westHand;
        public List<Card>[] hands;
        Dictionary<string, int> translations;

        public Deck()
        {
            translations = new Dictionary<string, int>();
            cards = new Card[52];
            distribution = new int[52];
            original_distribution = new int[52];
            cardList = new List<Card>();
            northHand = new List<Card>();
            eastHand = new List<Card>();
            southHand = new List<Card>();
            westHand = new List<Card>();
            hands = new List<Card>[] { northHand, eastHand, southHand, westHand };
            for (int i = 0; i < 13; i++)
            {
                cards[i] = new Card(new Rank(14 - i), new Suit("S"));
                cards[i + 13] = new Card(new Rank(14 - i), new Suit("H"));
                cards[i + 26] = new Card(new Rank(14 - i), new Suit("D"));
                cards[i + 39] = new Card(new Rank(14 - i), new Suit("C"));
                distribution[i] = 0;        original_distribution[i] = 0;
                distribution[i + 13] = 1;   original_distribution[i + 13] = 1;
                distribution[i + 26] = 2;   original_distribution[i + 26] = 2;
                distribution[i + 39] = 3;   original_distribution[i + 39] = 3;
                translations.Add(cards[i].rank.representation, i);
            }
            translations.Add("S", 0);
            translations.Add("H", 1);
            translations.Add("D", 2);
            translations.Add("C", 3);
            update();
        }

        public Deck(string format)
        {
            translations = new Dictionary<string, int>();
            cards = new Card[52];
            distribution = new int[52];
            original_distribution = new int[52];
            cardList = new List<Card>();
            northHand = new List<Card>();
            eastHand = new List<Card>();
            southHand = new List<Card>();
            westHand = new List<Card>();
            hands = new List<Card>[] { northHand, eastHand, southHand, westHand };
            for (int i = 0; i < 13; i++)
            {
                cards[i] = new Card(new Rank(14 - i), new Suit("S"));
                cards[i + 13] = new Card(new Rank(14 - i), new Suit("H"));
                cards[i + 26] = new Card(new Rank(14 - i), new Suit("D"));
                cards[i + 39] = new Card(new Rank(14 - i), new Suit("C"));
                translations.Add(cards[i].rank.representation, i);
            }
            translations.Add("S", 0);
            translations.Add("H", 1);
            translations.Add("D", 2);
            translations.Add("C", 3);
            string[] data = format.Split(".");
            for (int i = 0; i < 52; i++)
            {
                distribution[i] = int.Parse(data[i]);
                original_distribution[i] = int.Parse(data[i]);
            }
            update();
        }

        private void update()
        {
            northHand.Clear();
            eastHand.Clear();
            southHand.Clear();
            westHand.Clear();
            cardList.Clear();

            for (int i = 0; i < 52; i++)
            {
                if (distribution[i] == -1)
                {
                    cardList.Add(cards[i]);
                    continue;
                }
                hands[distribution[i]].Add(cards[i]);
            }
        }

        /// <summary> Shuffles the cards in the deck, changing the distribution attribute - not the cards attribute </summary>
        public void shuffle()
        {
            for (int i = 0; i < 52; i++)
            {
                int idx = new Random().Next(i, 52);
                int temp = distribution[idx];
                distribution[idx] = distribution[i];
                distribution[i] = temp;
            }
            for (int i = 0; i < 52; i++)
            {
                original_distribution[i] = distribution[i];
            }
            update();
        }

        public int hcp(int player)
        {
            int res = 0;
            foreach (Card card in hands[player])
            {
                res += card.hcp;
            }
            return res;
        }

        public int nbrOfCards(int hand, int suit)
        {
            int res = 0;
            foreach (Card card in hands[hand])
            {
                res += card.suit.ID == suit ? 1 : 0;
            }
            return res;
        }

        public void predeal(int[] distribution, int[] maxHcp = null, int[] minHcp = null, int[][] maxCards = null, int[][] minCards = null)
        {
            if (!(distribution.Length == 52))
            {
                throw new Exception("Wrong size of dist array");
            }
            northHand.Clear();
            eastHand.Clear();
            southHand.Clear();
            westHand.Clear();
            cardList.Clear();
            for (int i = 0; i < 52; i++)
            {
                this.distribution[i] = -1;
                original_distribution[i] = -1;
            }
            for (int i = 0; i < 52; i++)
            {
                if (distribution[i] == -1)
                {
                    cardList.Insert(new Random().Next(cardList.Count + 1), cards[i]);
                }
                else
                {
                    hands[distribution[i]].Add(cards[i]);
                    this.distribution[i] = distribution[i];
                }
            }

            int _i = 0;
            foreach (List<Card> hand in hands)
            {
                while (hand.Count < 13)
                {
                    Card card = cardList[0];
                    cardList.RemoveAt(0);
                    hand.Add(card);
                    this.distribution[card.ID] = _i;
                }
                _i++;
            }

            for (int i = 0; i < 52; i++)
            {
                original_distribution[i] = this.distribution[i];
            }

                if (cardList.Count > 0)
            {
                throw new Exception("cards left in cardList after predeal");
            }

        }
        
        public void predeal(string north = "...", string east = "...", string south = "...", string west = "...")
        {
            //TODO: MAKE MORE EFFICIENT - MAYBE IMPLEMENT DLL DEALER PROGRAM???
            northHand.Clear();
            eastHand.Clear();
            southHand.Clear();
            westHand.Clear();
            cardList.Clear();
            for (int i = 0; i < 52; i++)
            {
                distribution[i] = -1;
                cardList.Insert(new Random().Next(cardList.Count + 1), cards[i]);
            }
            string[][] arr = new string[][] { north.Split("."), east.Split("."), south.Split("."), west.Split(".") };
            for (int hand = 0; hand < 4; hand++)
            {
                for (int suit = 0; suit < 4; suit++)
                {
                    foreach (char c in arr[hand][suit])
                    {
                        distribution[13 * suit + translations[c.ToString()]] = hand;
                        hands[hand].Add(cards[13 * suit + translations[c.ToString()]]);
                        cardList.Remove(cards[13 * suit + translations[c.ToString()]]);
                    }
                }
            }
            int _i = 0;
            foreach (List<Card> hand in hands)
            {
                while (hand.Count < 13)
                {
                    Card card = cardList[0];
                    cardList.RemoveAt(0);
                    hand.Add(card);
                    distribution[card.ID] = _i;
                }
                _i++;
            }
            if (cardList.Count > 0)
            {
                throw new Exception("cards left in cardList after predeal");
            }
        }
        
        // TODO: PREDEAL FUNCTION FOR STATE INSIDE DEAL (NOT BEFORE PLAY START) !!!!! NOT ONLY AFTER TRICK
        public void predeal(int northCards, int eastCards, int southCards, int westCards, LinkedList<Card> playedCards, 
            string north = "...", string east = "...", string south = "...", string west = "...")
        {
            if (northCards+eastCards+southCards+westCards != 52-playedCards.Count)
            {
                throw new Exception("Not right amount of cards");
            }
            // TODO: FIX TO WORK INSIDE TRICK
            northHand.Clear();
            eastHand.Clear();
            southHand.Clear();
            westHand.Clear();
            cardList.Clear();
            List<int> cardIds = new List<int>();
            foreach (Card card in playedCards)
            {
                cardIds.Add(card.ID);
                original_distribution[card.ID] = card.playedBy;
            }
            for (int i = 0; i < 52; i++)
            {
                if (cardIds.Contains(i))
                {
                    continue;
                }
                cardList.Insert(new Random().Next(cardList.Count + 1), cards[i]);
            }
            string[][] arr = new string[][] { north.Split("."), east.Split("."), south.Split("."), west.Split(".") };
            for (int hand = 0; hand < 4; hand++)
            {
                for (int suit = 0; suit < 4; suit++)
                {
                    foreach (char c in arr[hand][suit])
                    {
                        distribution[13 * suit + translations[c.ToString()]] = hand;
                        original_distribution[13 * suit + translations[c.ToString()]] = hand;
                        hands[hand].Add(cards[13 * suit + translations[c.ToString()]]);
                        cardList.Remove(cards[13 * suit + translations[c.ToString()]]);
                    }
                }
            }

            while (northHand.Count < northCards)
            {
                Card card = cardList[0];
                cardList.RemoveAt(0);
                northHand.Add(card);
                distribution[card.ID] = 0;
                original_distribution[card.ID] = 0;
            }

            while (eastHand.Count < eastCards)
            {
                Card card = cardList[0];
                cardList.RemoveAt(0);
                eastHand.Add(card);
                distribution[card.ID] = 1;
                original_distribution[card.ID] = 1;
            }

            while (southHand.Count < southCards)
            {
                Card card = cardList[0];
                cardList.RemoveAt(0);
                southHand.Add(card);
                distribution[card.ID] = 2;
                original_distribution[card.ID] = 2;
            }

            while (westHand.Count < westCards)
            {
                Card card = cardList[0];
                cardList.RemoveAt(0);
                westHand.Add(card);
                distribution[card.ID] = 3;
                original_distribution[card.ID] = 3;
            }

            if (cardList.Count > 0)
            {
                throw new Exception("cards left in cardList after predeal");
            }
        }
        public string format()
        {
            string res = "";
            res += string.Join('.', distribution);
            return res;
        }
    }
}