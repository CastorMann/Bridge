using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Hand
    {
        public List<Card> cards;
        public List<Card> spades;
        public List<Card> hearts;
        public List<Card> diamonds;
        public List<Card> clubs;

        public Hand()
        {
            this.cards = new List<Card>();
            this.spades = new List<Card>();
            this.hearts = new List<Card>();
            this.diamonds = new List<Card>();
            this.clubs = new List<Card>();
        }

        public Hand(Card[] cards)
        {
            setHand(cards);
        }

        /// <summary> Adds a card to the hand </summary>
        /// <returns> true if the card could be added, otherwise false </returns>
        public bool add(Card card)
        {
            if (this.cards.Contains(card))
            {
                return false;
            }

            switch (card.suit.value)
            {
                case "S":
                    this.spades.Add(card);
                    break;
                case "H":
                    this.hearts.Add(card);
                    break;
                case "D":
                    this.diamonds.Add(card);
                    break;
                case "C":
                    this.clubs.Add(card);
                    break;
                default:
                    Console.WriteLine("Suit has invalid value");
                    return false;
            }
            this.cards.Add(card);
            return true;
        }

        public void setHand(Card[] cards)
        {
            this.cards = new List<Card>();
            this.spades = new List<Card>();
            this.hearts = new List<Card>();
            this.diamonds = new List<Card>();
            this.clubs = new List<Card>();

            foreach (Card card in cards)
            {
                this.cards.Add(card);
                switch (card.suit.value)
                {
                    case "S":
                        this.spades.Add(card);
                        break;
                    case "H":
                        this.hearts.Add(card);
                        break;
                    case "D":
                        this.diamonds.Add(card);
                        break;
                    case "C":
                        this.clubs.Add(card);
                        break;
                    default:
                        Console.WriteLine("Suit has invalid value");
                        break;
                }
            }
        }
    }
}
