using System;

namespace Chessuaw
{
    public class Board
    {
        // Bitboards
        long WP = 0, WN = 0, WB = 0, WR = 0, WQ = 0, WK = 0, BP = 0, BN = 0, BB = 0, BR = 0, BQ = 0, BK = 0;

        public Board()
        {

        }

        public string debug()
        {
            return Convert.ToString(BR, 2);
        }

        public void DrawBoard()
        {
            char symbol;

            for (int i = 0; i < 64; i++)
            {

                if (((WP >> i) & 1) == 1) { symbol = 'P';  }
                else if (((WN >> i) & 1) == 1) { symbol = 'N'; }
                else if (((WB >> i) & 1) == 1) { symbol = 'B'; }
                else if (((WR >> i) & 1) == 1) { symbol = 'R'; }
                else if (((WQ >> i) & 1) == 1) { symbol = 'Q'; }
                else if (((WK >> i) & 1) == 1) { symbol = 'K'; }
                else if (((BP >> i) & 1) == 1) { symbol = 'p'; }
                else if (((BN >> i) & 1) == 1) { symbol = 'n'; }
                else if (((BB >> i) & 1) == 1) { symbol = 'b'; }
                else if (((BR >> i) & 1) == 1) { symbol = 'r'; }
                else if (((BQ >> i) & 1) == 1) { symbol = 'q'; }
                else if (((BK >> i) & 1) == 1) { symbol = 'k'; }
                else { symbol = 'o'; }
           
                System.Console.Write(symbol);

                if ((i + 1) % 8 == 0) { System.Console.WriteLine(); }
            }
        }

        public void LoadFen(string fen)
        {
            String binary;
            int blankFields = 0;
            int blankCount = 0;
            int slashCount = 0;

            for (int i = 0; i < fen.Length - 1; i++)
            {
                //binary = "0000000000000000000000000000000000000000000000000000000000000000";
                //binary = binary.Substring(i + 1 - blankFields) + "1" + new string('0', blankFields) + binary.Substring(0, i);
                binary = new string('0', i + blankFields - slashCount - blankCount) + '1' + new string('0', 63 - i - blankFields + slashCount + blankCount);

                
                if (fen[i] != '/')
                {
                    System.Console.WriteLine(binary + fen[i]);
                }

                if (Char.IsDigit(fen[i]))
                {
                    blankCount++;
                    blankFields += (int) Char.GetNumericValue(fen[i]);
                }
                else if (fen[i] == '/')
                { 
                    slashCount++;
                }
                else 
                {
                    switch (fen[i])
                    {
                        case 'p':
                            WP += StringToBitboard(binary);
                            break;
                        case 'n':
                            WN += StringToBitboard(binary);
                            break;
                        case 'b':
                            WB += StringToBitboard(binary);
                            break;
                        case 'r':
                            WR += StringToBitboard(binary);
                            break;
                        case 'q':
                            WQ += StringToBitboard(binary);
                            break;
                        case 'k':
                            WK += StringToBitboard(binary);
                            break;
                        case 'P':
                            BP += StringToBitboard(binary);
                            break;
                        case 'N':
                            BN += StringToBitboard(binary);
                            break;
                        case 'B':
                            BB += StringToBitboard(binary);
                            break;
                        case 'R':
                            BR += StringToBitboard(binary);
                            break;
                        case 'Q':
                            BQ += StringToBitboard(binary);
                            break;
                        case 'K':
                            BK += StringToBitboard(binary);
                            break;
                    }
                }
            }
        }

        private long StringToBitboard(String str)
        {
            long a;
            if (str[0] == '0')
            {
                a = (long) Convert.ToInt64(str, 2);
            } else
            {
                a = (long) Convert.ToInt64('1' + str.Substring(1), 2) * 2;
            }

            return a;
        }
    }
}
