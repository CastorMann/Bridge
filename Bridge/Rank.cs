using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Rank
    {
        public int value;
        public string representation;

        public Rank(int value)
        {
            this.value = value;
            this.representation = value.ToString();
            if (value > 9)
            {
                switch (value)
                {
                    case 10:
                        this.representation = "T";
                        break;
                    case 11:
                        this.representation = "J";
                        break;
                    case 12:
                        this.representation = "Q";
                        break;
                    case 13:
                        this.representation = "K";
                        break;
                    case 14:
                        this.representation = "A";
                        break;
                    default:
                        Console.WriteLine("Invalid rank value");
                        this.representation = "-1";
                        break;
                }
            }
        }

        public override string ToString()
        {
            return representation;
        }
    }
}
