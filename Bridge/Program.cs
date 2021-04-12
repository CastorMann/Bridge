using System;
using System.Collections.Generic;

namespace Bridge
{
    class Program
    {
        static void Main(string[] args)
        {
            int totError = 0;
            for (int x = 0; x < 10; x++)
            {
                Console.WriteLine("Deal#: " + x);
                Table table = new Table(1);
                table.addDeal();
                table.activeDeal.Value.makeBid(0);
                table.activeDeal.Value.makeBid(0);
                table.activeDeal.Value.makeBid(0);
                table.activeDeal.Value.makeBid(7);
                table.activeDeal.Value.makeBid(0);
                table.activeDeal.Value.makeBid(0);
                table.activeDeal.Value.makeBid(0);
                foreach (Player player in table.players)
                {
                    player.updateData();
                }
                table.players[table.activeDeal.Value.playerOnTurn].AIPlay(200);
                foreach (Player player in table.players)
                {
                    player.updateData();
                }
                for (int i = 0; i < 51; i++)
                {
                    table.players[table.activeDeal.Value.playerOnTurn].AIPlay(50);
                }
                //Console.WriteLine("NSTricks Optimal: " + solver.getTricks());
                //Console.WriteLine("NSTricks Actual: " + table.activeDeal.Value.NSTricks);
                Console.WriteLine("Error margin: " + table.errorMargin);
                totError += table.errorMargin;
            }
            Console.WriteLine("Error: " + totError);
            
            





            /*
            Deal deal = new Deal(1);
            
            deal.shuffle();
            deal.playCard(deal.deck.northHand[0].ID);
            deal.print(true);
            string myHand = deal.ToString().Split(" ")[1];
            string pHand = deal.ToString().Split(" ")[3];
            Dictionary<string, int> data = new Dictionary<string, int>();
            for (int i = 0; i < 500; i++)
            {
                Deal myDeal = new Deal(1);
                myDeal.predeal(deal, east: myHand, west: pHand);
                //myDeal.print(true);
                DoubleDummySolver solver = new DoubleDummySolver(myDeal.ToString(), string.Join(" ", myDeal.playedCards));
                foreach (Card card in myDeal.deck.cards)
                {
                    if (card.isPlayable)
                    {
                        if (!data.ContainsKey(card.ToString()))
                        {
                            data.Add(card.ToString(), solver.getTricksEx(card.ToString()));
                        }
                        else
                        {
                            data[card.ToString()] += solver.getTricksEx(card.ToString());
                        }
                    }
                }
                solver.destroy();
            }
            foreach (string card in data.Keys)
            {
                Console.WriteLine(card + ": " + data[card]/500.0);
            }
            DoubleDummySolver solver2 = new DoubleDummySolver(deal.ToString(), string.Join(" ", deal.playedCards));
            Console.WriteLine("Facit:");
            foreach (Card card in deal.deck.cards)
            {
                if (card.isPlayable)
                {
                    Console.WriteLine(card.ToString() + ": " + solver2.getTricksEx(card.ToString()));
                }
            }
            
            
            
            var watch = new System.Diagnostics.Stopwatch();
            int[] results = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Deal deal = new Deal(1);
            watch.Start();
            for (int i = 0; i < 1000; i++)
            {
                deal.predeal(north: "AK87.A5432.2.A32", east: ".Q.Q.JT9", south: "QJT9.6.A6543.KQ4", west: ".K.K.876");
                deal.solver.setTrump(3);
                int x = 0;
                foreach (Card card in deal.deck.cards)
                {
                    if (!card.isPlayable)
                    {
                        continue;
                    }
                    deal.solver.getTricksEx(card.ToString());
                    int res = deal.solver.getTricksEx(card.ToString());
                    results[x] += res;
                    //Console.WriteLine("Number of tricks possible to take if player plays " + card.ToString() + ": " + res);
                    x++;
                }
            }
            watch.Stop();
            Console.WriteLine("----------------------");
            for (int i = 0; i < 13; i++)
            {
                Console.WriteLine(deal.deck.northHand[i].ToString() + ": " + results[i] / 1000.0);
            }
            Console.WriteLine("----------------------");
            Console.WriteLine("Execution Time: " + watch.ElapsedMilliseconds);


            */

        }
    }
}
