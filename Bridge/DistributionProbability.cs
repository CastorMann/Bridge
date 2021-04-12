using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class DistributionProbability
    {
        public double[] north = new double[52];
        public double[] east = new double[52];
        public double[] south = new double[52];
        public double[] west = new double[52];
        public double[][] players;

        public DistributionProbability()
        {
            Array.Fill(north, 0.25);
            Array.Fill(east, 0.25);
            Array.Fill(south, 0.25);
            Array.Fill(west, 0.25);
            players = new double[4][] { north, east, south, west };
        }

        public void setCard(int cardID, int player)
        {
            for (int i = 0; i < 4; i++)
            {
                players[i][cardID] = i == player ? 1.0 : 0.0;
            }
        }

        public void raiseProbability(int cardID, int player, double factor)
        {
            if (factor < 1)
            {
                throw new ArgumentException("Factor must be greater than or equal to 1");
            }
            players[player][cardID] += factor-1.0;
            refresh(cardID);
        }

        public void setProbability(int cardID, int player, double probability)
        {
            if (players[player][cardID] == 1.0 && probability == 0.0)
            {
                throw new ArgumentException("Cannot set card with p1 to p0");
            }
            players[player][cardID] = probability;
            refresh(cardID);
        }

        public void lowerProbability(int cardID, int player, double factor)
        {
            players[player][cardID] /= factor;
            refresh(cardID);
        }

        private void refresh(int cardID)
        {
            double sum = north[cardID] + east[cardID] + south[cardID] + west[cardID];
            for (int i = 0; i < 4; i++)
            {
                players[i][cardID] /= sum;
            }
        }

        private int getHand(int cardID)
        {
            double val = new Random().NextDouble();
            for (int i = 0; i < 4; i++)
            {
                val -= players[i][cardID];
                if (val < 0)
                {
                    return i;
                }
            }
            return 3;
        }

        public int[] getDistribution()
        {
            int[] result = new int[52];
            for (int i = 0; i < 52; i++)
            {
                //TODO:
            }
            return result;
        }
    }
}


