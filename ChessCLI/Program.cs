using Chessuaw;
using System;

namespace ChessCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Board board = new Board("rnbqk1nr/pppppppp/8/8/4B3/8/PPPPPPPP/RNBQKBNR");
            // Moves moves = new Moves(board);
            // board.DrawBoard();
            // moves.PrintBitboards();

            Utils.PrintBitboard(Utils.StringToBitboard("1000000000000000000000000000000100000000000000000000000000000000"));
            Console.WriteLine(Utils.StringToBitboard("1000000000000000000000000000000100000000000000000000000000000000"));
            Utils.PrintBitboard(Utils.ReverseLong(Utils.StringToBitboard("1000000000000000000000000000000100000000000000000000000000000000")));
        }
    }
}
