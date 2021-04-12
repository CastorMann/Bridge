using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class Contract
    {
        public int level;
        public Strain strain;
        public bool isDoubled;
        public bool isRedoubled;
        public bool isVulnerable;
        public string declarer;

        public Contract()
        {

        }

        public Contract(int level, Strain strain, bool isDoubled, bool isRedoubled, bool isVulnerable, string declarer)
        {
            this.level = level;
            this.strain = strain;
            this.isDoubled = isDoubled;
            this.isRedoubled = isRedoubled;
            this.isVulnerable = isVulnerable;
            this.declarer = declarer;
        }

        /// <summary> Calculates the score of the contract given the number of tricks </summary>
        /// <param name=nbrOfTricks> The number of tricks taken </param>
        /// <returns> An integer representing the score for the contract </returns>
        public int getScore(int nbrOfTricks)
        {
            int res = 0;
            if (nbrOfTricks >= level + 6)
            {         // Making contract
                int contract_bonus = 0;
                int overtrick_bonus = 0;

                contract_bonus += (strain.value == "C" || strain.value == "D" ? 20 : 30) * level + (strain.value == "NT" ? 10 : 0);
                if (isDoubled)
                {
                    contract_bonus *= 2;
                    overtrick_bonus += (isVulnerable ? 200 : 100) * (nbrOfTricks - level - 6);
                }
                else if (isRedoubled)
                {
                    contract_bonus *= 4;
                    overtrick_bonus += (isVulnerable ? 400 : 200) * (nbrOfTricks - level - 6);
                }
                else
                {
                    overtrick_bonus += (strain.value == "C" || strain.value == "D" ? 20 : 30) * (nbrOfTricks - level - 6);
                }
                if (contract_bonus >= 100)
                {
                    res += (isVulnerable ? 500 : 300);
                }
                if (level == 6)
                {
                    res += (isVulnerable ? 750 : 500);
                }
                if (level == 7)
                {
                    res += (isVulnerable ? 1500 : 1000);
                }
                res += contract_bonus + overtrick_bonus;

            }

            else
            {                                  // Not making contract
                if (isDoubled)
                {
                    res -= (isVulnerable ? 200 : 100) + 200 * (level + 5 - nbrOfTricks) + (int)MathF.Max((level + 5 - nbrOfTricks) - (isVulnerable ? 0 : 2), 0) * 100;
                }
                else if (isRedoubled)
                {
                    res -= 2 * ((isVulnerable ? 200 : 100) + 200 * (level + 5 - nbrOfTricks) + (int)MathF.Max((level + 5 - nbrOfTricks) - (isVulnerable ? 0 : 2), 0) * 100);
                }
                else
                {
                    res -= (level + 6 - nbrOfTricks) * (isVulnerable ? 100 : 50);
                }
            }

            return res;
        }

        public void Double()
        {
            isRedoubled = false;
            isDoubled = true;
        }

        public void Redouble()
        {
            isRedoubled = true;
            isDoubled = false;
        }

        public int getDeclarer()
        {
            return declarer == "N" ? 0 : declarer == "E" ? 1 : declarer == "S" ? 2 : 3;
        }

        public override string ToString()
        {
            return $"{level}{strain}{declarer}" + (isDoubled ? "X" : isRedoubled ? "XX" : "");
        }
    }
}
