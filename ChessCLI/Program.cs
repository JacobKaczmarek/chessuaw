using Chessuaw;
using System;

namespace ChessCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board("");
            Moves moves = new Moves(board);
            board.DrawBoard();
            Console.WriteLine(moves.WhiteMoves());
        }
    }
}
 