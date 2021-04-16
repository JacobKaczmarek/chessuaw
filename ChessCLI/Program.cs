using Chessuaw;
using System;

namespace ChessCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            board.LoadFen("5K2/2Q5/n1p3P1/P2b1B1q/7N/5r1p/p1P3p1/4k3");
            board.DrawBoard();
            System.Console.WriteLine(board.debug());
        }
    }
}
