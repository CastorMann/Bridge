using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Bridge
{
    /// <summary>
    /// Wrapper class for libbcalcDDS.ddl.
    /// </summary>
    class DoubleDummySolver
    {
        /// <summary>
        /// Allocates new instance of BCalc solver.
        /// </summary>
        /// <param name="format">Deal format string. See the original documentation for details.</param>
        /// <param name="hands">Card distribution.</param>
        /// <param name="trump">Trump denomination, in numeric format. See the original documentation for details.</param>
        /// <param name="leader">Player on lead, in numeric format. See the original documentation for details.</param>
        /// <returns>Pointer to solver instance.</returns>
        /// <remarks>Original documentation: http://bcalc.w8.pl/API_C/bcalcdds_8h.html#ab636045f65412652246b769e8e95ed6f</remarks>
        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr bcalcDDS_new(IntPtr format, IntPtr hands, int trump, int leader);

        /// <summary>
        /// Conducts DD analysis.
        /// </summary>
        /// <param name="solver">Pointer to solver instance.</param>
        /// <returns>Number of tricks to take by leading side.</returns>
        /// <remarks>Original documentation: http://bcalc.w8.pl/API_C/bcalcdds_8h.html#a369ce661d027bef3f717967e42bf8b33</remarks>
        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int bcalcDDS_getTricksToTake(IntPtr solver);

        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int bcalcDDS_getTricksToTakeEx(IntPtr solver, int tricks_target, IntPtr card);

        /// <summary>
        /// Checks for solver errors.
        /// </summary>
        /// <param name="solver">Pointer to solver instance.</param>
        /// <returns>Error string or NULL if no error accured.</returns>
        /// <remarks>Original documentation: http://bcalc.w8.pl/API_C/bcalcdds_8h.html#a89cdec200cde91331d40f0900dc0fb46</remarks>
        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr bcalcDDS_getLastError(IntPtr solver);

        /// <summary>
        /// Frees allocated solver instance and cleans up after it.
        /// </summary>
        /// <param name="solver">Pointer to solver instance.</param>
        /// <remarks>Original documentation: http://bcalc.w8.pl/API_C/bcalcdds_8h.html#a4a68da83bc7da4663e2257429539912d</remarks>
        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void bcalcDDS_delete(IntPtr solver);

        /// <summary>
        /// Sets trump denomination and resets the analysis.
        /// </summary>
        /// <param name="solver">Pointer to solver instance.</param>
        /// <param name="trump">Trump denomination, in numeric format. See the original documentation for details.</param>
        /// <remarks>Original documentation: http://bcalc.w8.pl/API_C/bcalcdds_8h.html#a88fba3432e66efa5979bbc9e1f044164</remarks>
        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void bcalcDDS_setTrumpAndReset(IntPtr solver, int trump);

        /// <summary>
        /// Sets leading player and resets the analysis.
        /// </summary>
        /// <param name="solver">Pointer to solver instance.</param>
        /// <param name="player">Player on lead, in numeric format. See the original documentation for details.</param>
        /// <remarks>Original documentation: http://bcalc.w8.pl/API_C/bcalcdds_8h.html#a616031c1e1d856c4aac14390693adb4c</remarks>
        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void bcalcDDS_setPlayerOnLeadAndReset(IntPtr solver, int player);

        /// <summary>
        /// Execute commands which can modify the state of given solver (current situation in game represented by it).
        /// Commands should be separated by spaces and each can be one of:
        /// <C><S> - where<C> is card symbol(like: A, Q, T, 6), <S> is suit symbol(one of: C, D, H, S), play choosen card
        /// x - play smallest possible card following played suit
        /// u - unplay one card
        /// ut - unplay last trick
        /// ua - unplay all tricks
        /// </summary>
        /// <param name="solver">Pointer to solver instance.</param>
        /// <param name="cmds">Commands to execute.</param>
        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void bcalcDDS_exec(IntPtr solver, IntPtr cmds);


        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr bcalcDDS_getCards(IntPtr solver, IntPtr result, int player, int suit);

        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int bcalcDDS_getCardsLeftCount(IntPtr solver);

        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern string[] bcalcDDS_getCardsToPlay(IntPtr solver, string[] result, int suit);

        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int bcalcDDS_getPlayerToPlay(IntPtr solver);

        [DllImport("libbcalcdds.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int bcalcDDS_getTrump(IntPtr solver);

        /// <summary>
        /// Translation table between player characters and numeric values.
        /// </summary>
        public string table = "NESW";
        /// <summary>
        /// Translation table between denomination characters and numeric values.
        /// </summary>
        public string denominations = "CDHSN";

        /// <summary>
        /// Pointer to solver instance.
        /// </summary>
        private IntPtr solver;
        /// <summary>
        /// Trump suit, in numeric format.
        /// </summary>
        private int trumps;
        private int leader;

        /// <summary>
        /// Constructor of class instance for single deal analysis.
        /// </summary>
        /// <param name="deal">Deal distribution in BCalc's NESW format with prefixed number.</param> // no prefix....
        public DoubleDummySolver(string deal, string cmds = "")
        {
            // "deal" should be of format "<deal#>: <Nsp>.<Nhe>.<Ndi>.<Ncl> <Esp>.<Ehe>.<Edi>.<Ecl> <Ssp>.<She>.<Sdi>.<Scl> <Wsp>.<Whe>.<Wdi>.<Wcl>"
            
            solver = bcalcDDS_new(Marshal.StringToHGlobalAnsi(table), Marshal.StringToHGlobalAnsi(deal), 0, 0);
            errorCheck();
            if (cmds != "")
            {
                exec(cmds);
            }
            errorCheck();
        }

        public DoubleDummySolver(Deal deal)
        {
            solver = bcalcDDS_new(Marshal.StringToHGlobalAnsi(table), Marshal.StringToHGlobalAnsi(deal.ToString()), 0, 0);
            errorCheck();
            setTrump(deal.contract.strain.getValue());
            setLeader((deal.contract.getDeclarer() + 1) % 4);
            if (deal.playedCards.Count > 0)
            {
                exec(string.Join(" ", deal.playedCards));
            }
            errorCheck();
        }

        /// <summary>
        /// Sets the trump denomination.
        /// </summary>
        /// <param name="trumpSuit">Trump denomination, in numeric format.</param>
        public void setTrump(int trumpSuit)
        {
            if (trumpSuit < 0)
            {
                throw new Exception("BCalcWrapper_trumpError: " + trumpSuit);
            }
            bcalcDDS_setTrumpAndReset(solver, trumpSuit);
            errorCheck();
            trumps = trumpSuit;
        }

        public int getTrump()
        {
            return bcalcDDS_getTrump(solver);
        }

        public void setLeader(int _leader)
        {
            if (_leader < 0 || _leader > 3)
            {
                throw new Exception("BCalcWrapper_leaderError: " + _leader);
            }
            bcalcDDS_setPlayerOnLeadAndReset(solver, _leader);
            errorCheck();
            leader = _leader;
            
        }

        public int getLeader()
        {
            return bcalcDDS_getPlayerToPlay(solver);
        }


        public string[] getPlayableCards(int suit)
        {
            string[] result = new string[14];
            //result = bcalcDDS_getCardsToPlay(solver, Marshal.StringToHGlobalAnsi(result), suit);

            return result;
        }

        /// <summary>
        /// Runs the single contract analysis.
        /// </summary>
        /// <param name="declarer">Declaring player, in numeric format.</param>
        /// <returns>Result structur for the contract.</returns>
        public int getTricks()
        {
            int result = bcalcDDS_getTricksToTake(solver);
            errorCheck();
            return result;
        }

        public int getTricksEx(string card)
        {
            int result = bcalcDDS_getTricksToTakeEx(solver, -1, Marshal.StringToHGlobalAnsi(card));
            errorCheck();
            return result;
        }

        public void exec(string cmds)
        {
            bcalcDDS_exec(solver, Marshal.StringToHGlobalAnsi(cmds));
            errorCheck();
        }

        /// <summary>
        /// Releases the solver instances.
        /// </summary>
        public void destroy()
        {
            bcalcDDS_delete(solver);
        }

        public void printFullDDAnalisys()
        {
            Console.WriteLine("   N     E     S     W");
            for(int i = 0; i<5; i++)
            {
                setTrump(i);
                Console.Write(denominations[i] + ": ");
                for (int j = 0; j < 4; j++)
                {
                    setLeader(j);
                    int result = getTricks();
                    Console.Write(result + "    ");
                    if (result < 10)
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("------------------------");
            }
            setLeader(0);
            setTrump(4);
        }

        /// <summary>
        /// Checks for errors, throws exception if any occured.
        /// </summary>
        private void errorCheck()
        {
            IntPtr error = bcalcDDS_getLastError(solver);
            string eStr = Marshal.PtrToStringAnsi(error);
            if (eStr != null)
            {
                throw new Exception(eStr);
            }
        }
    }

    /*********************************************************/
    /*                                                       */
    /*      STRUCT/class  for result from deal analysis      */
    /*                                                       */
    /*********************************************************/

    /// <summary>
    /// Structure holding single deal analysis result.
    /// </summary>
    public struct BCalcResult
    {
        /// <summary>
        /// Trump denomination (N/S/H/D/C).
        /// </summary>
        public char trumpSuit;
        /// <summary>
        /// Declaring player (N/E/S/W).
        /// </summary>
        public char declarer;
        /// <summary>
        /// Number of tricks taken by the declaring side.
        /// </summary>
        public int tricks;
        /// <summary>
        /// Constructor for result structure.
        /// </summary>
        /// <param name="suit">Trump denomination (N/S/H/D/C).</param>
        /// <param name="decl">Declaring player (N/E/S/W).</param>
        /// <param name="tricksTaken">Number of tricks taken by the declaring side.</param>
        public BCalcResult(char suit, char decl, int tricksTaken)
        {
            trumpSuit = suit;
            declarer = decl;
            tricks = tricksTaken;
        }
    };
}
