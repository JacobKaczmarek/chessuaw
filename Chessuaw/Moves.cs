using System;
using System.Numerics;

namespace Chessuaw {
  public class Moves {
    Board board;
    long FILE_A, FILE_B, FILE_C, FILE_D, FILE_E, FILE_F, FILE_G, FILE_H;
    long RANK_1, RANK_2, RANK_3, RANK_4, RANK_5, RANK_6, RANK_7, RANK_8;
    long CENTRE, EXTENDED_CENTRE, KING_SIDE, QUEEN_SIDE;
    long WHITE_PIECES; // All capturable white pieces (excluding king)
    long BLACK_PIECES; // All capturable black pieces (excluding king)
    long OCCUPIED;
    long[] FileMasks;
    long[] RankMasks;
    long[] DiagonalMasks;
    long[] AntiDiagonalMasks;
    long EMPTY;
    public Moves(Board board) {
      this.board = board;

      FILE_A = Utils.StringToBitboard("0000000100000001000000010000000100000001000000010000000100000001");
      FILE_B = FILE_A << 1;
      FILE_C = FILE_A << 2;
      FILE_D = FILE_A << 3;
      FILE_E = FILE_A << 4;
      FILE_F = FILE_A << 5;
      FILE_G = FILE_A << 6;
      FILE_H = FILE_A << 7;

      RANK_1 = Utils.StringToBitboard("1111111100000000000000000000000000000000000000000000000000000000");
      RANK_2 = (long) ((ulong) RANK_1 >> 8);
      RANK_3 = (long) ((ulong) RANK_1 >> 16);
      RANK_4 = (long) ((ulong) RANK_1 >> 24);
      RANK_5 = (long) ((ulong) RANK_1 >> 32);
      RANK_6 = (long) ((ulong) RANK_1 >> 40);
      RANK_7 = (long) ((ulong) RANK_1 >> 48);
      RANK_8 = (long) ((ulong) RANK_1 >> 56);

      FileMasks = new long[] { FILE_A, FILE_B, FILE_C, FILE_D, FILE_E, FILE_F, FILE_G, FILE_H };
      RankMasks = new long[] { RANK_1, RANK_2, RANK_3, RANK_4, RANK_5, RANK_6, RANK_7, RANK_8 };
      DiagonalMasks = new long[] { 0x1, 0x102, 0x10204, 0x1020408, 0x102040810, 0x10204081020, 0x1020408102040, 0x102040810204080, 0x204081020408000, 0x408102040800000, 0x810204080000000, 0x1020408000000000, 0x2040800000000000, 0x4080000000000000, -9223372036854775808 };
      AntiDiagonalMasks = new long[] { 0x80, 0x8040, 0x804020, 0x80402010, 0x8040201008, 0x804020100804, 0x80402010080402, -9205322385119247871, 0x4020100804020100, 0x2010080402010000, 0x1008040201000000,0x804020100000000, 0x402010000000000, 0x201000000000000, 0x100000000000000 };
      CENTRE = Utils.StringToBitboard("0000000000000000000000000001100000011000000000000000000000000000");
      EXTENDED_CENTRE = 66229406269440;
      KING_SIDE = 0; //TODO
      QUEEN_SIDE = 1085102592571150095;
    }

    public string WhiteMoves() {
      string list = "";
      OCCUPIED = (board.WP | board.WR | board.WN | board.WB | board.WQ | board.WK | board.BP | board.BR | board.BN | board.BB | board.BQ | board.BK);
      EMPTY = ~OCCUPIED;

      // list += WhitePawnMoves("");
      list += WhiteBishopMoves();

      return list;
    }

    public long HorizontalVerticalMoves(int position) {
      long binaryPos = 1 << position;
      long horizontal = (OCCUPIED - 2 * binaryPos) ^ Utils.ReverseLong(Utils.ReverseLong(OCCUPIED) - 2 * Utils.ReverseLong(binaryPos));
      long vertical = ((OCCUPIED & FileMasks[position % 8]) - (2 * binaryPos)) ^ Utils.ReverseLong(Utils.ReverseLong(OCCUPIED & FileMasks[position % 8]) - 2 * Utils.ReverseLong(binaryPos));
      
      return (horizontal & RankMasks[position / 8]) | (vertical & FileMasks[position % 8]);
    }

    public long Diagonal(int position) {
      long binaryPos = 1 << position;
      long diagonal = ((OCCUPIED & DiagonalMasks[(position / 8) + (position % 8)]) - (2 * binaryPos)) ^ Utils.ReverseLong(Utils.ReverseLong(OCCUPIED & DiagonalMasks[(position / 8) + (position % 8)]) - (2 * Utils.ReverseLong(binaryPos)));
      long antiDiagonal = ((OCCUPIED & AntiDiagonalMasks[(position / 8) + 7 - (position % 8)]) - (2 * binaryPos)) ^ Utils.ReverseLong(Utils.ReverseLong(OCCUPIED & AntiDiagonalMasks[(position / 8) + 7 - (position % 8)]) - (2 * Utils.ReverseLong(binaryPos)));

      return (diagonal & DiagonalMasks[(position / 8) + (position % 8)]) | (antiDiagonal & AntiDiagonalMasks[(position / 8) + 7 - (position % 8)]);
    }

    public string WhitePawnMoves(string history) {
      // TODO: Loop only to last 1
      string list = "";
      long PAWN_MOVES, POSSIBILITY;

      PAWN_MOVES = (board.WP >> 7) & BLACK_PIECES & ~RANK_8 & ~FILE_A; // Capture to the right

      for (int i = BitOperations.TrailingZeroCount(PAWN_MOVES); i < 64 - BitOperations.LeadingZeroCount((ulong) PAWN_MOVES); i++)
      {
        if (((PAWN_MOVES >> i) & 1) == 1) { list += $"{i / 8 + 1}{i % 8 - 1}{i / 8}{i % 8}"; }
      }

      PAWN_MOVES = (board.WP >> 9) & BLACK_PIECES & ~RANK_8 & ~FILE_H; // Capture to the left

      for (int i = BitOperations.TrailingZeroCount(PAWN_MOVES); i < 64 - BitOperations.LeadingZeroCount((ulong)PAWN_MOVES); i++)
      {
        if (((PAWN_MOVES >> i) & 1) == 1) { list += $"{i / 8 + 1}{i % 8 + 1}{i / 8}{i % 8}"; }
      }

      PAWN_MOVES = (board.WP >> 8) & EMPTY & ~RANK_8; // Move 1 forward
      for (int i = BitOperations.TrailingZeroCount(PAWN_MOVES); i < 64 - BitOperations.LeadingZeroCount((ulong)PAWN_MOVES); i++)
      {
        if (((PAWN_MOVES >> i) & 1) == 1) { list += $"{i / 8 + 1}{i % 8}{i / 8}{i % 8}"; }
      }

      PAWN_MOVES = (board.WP >> 16) & EMPTY & EMPTY >> 8 & RANK_4; // Move 2 forward

      for (int i = BitOperations.TrailingZeroCount(PAWN_MOVES); i < 64 - BitOperations.LeadingZeroCount((ulong)PAWN_MOVES); i++)
      {
        if (((PAWN_MOVES >> i) & 1) == 1) { list += $"{i / 8 + 2}{i % 8}{i / 8}{i % 8}"; }
      }

      // y1, y2, Promotion type, P
      PAWN_MOVES = (board.WP >> 7) & BLACK_PIECES & RANK_8 & ~FILE_A; // Promotion capture to the right

      for (int i = BitOperations.TrailingZeroCount(PAWN_MOVES); i < 64 - BitOperations.LeadingZeroCount((ulong)PAWN_MOVES); i++)
      {
        if (((PAWN_MOVES >> i) & 1) == 1) { list += $"{i % 8 - 1}{i % 8}QP{i % 8 - 1}{i % 8}RP{i % 8 - 1}{i % 8}BP{i % 8 - 1}{i % 8}NP"; }
      }

      PAWN_MOVES = (board.WP >> 9) & BLACK_PIECES & RANK_8 & ~FILE_H; // Promotion capture to the left

      for (int i = BitOperations.TrailingZeroCount(PAWN_MOVES); i < 64 - BitOperations.LeadingZeroCount((ulong)PAWN_MOVES); i++)
      {
        if (((PAWN_MOVES >> i) & 1) == 1) { list += $"{i % 8 + 1}{i % 8}QP{i % 8 + 1}{i % 8}RP{i % 8 + 1}{i % 8}BP{i % 8 + 1}{i % 8}NP"; }
      }

      PAWN_MOVES = (board.WP >> 7) & EMPTY & RANK_8 & ~FILE_A; // Promotion capture to the right

      for (int i = BitOperations.TrailingZeroCount(PAWN_MOVES); i < 64 - BitOperations.LeadingZeroCount((ulong)PAWN_MOVES); i++)
      {
        if (((PAWN_MOVES >> i) & 1) == 1) { list += $"{i % 8}{i % 8}QP{i % 8}{i % 8}RP{i % 8}{i % 8}BP{i % 8}{i % 8}NP"; }
      }

      //y1, y2, space, E
      if (history.Length >=  4) {
        int targetFile;

        if (history[history.Length - 1] == history[history.Length - 3] & history[history.Length - 2] - history[history.Length - 4] == 2) {
          targetFile = (int) Char.GetNumericValue(history[history.Length - 1]); // En Passant to the right
          POSSIBILITY = (board.WP << 1) & board.BP & RANK_5 & ~FILE_A & FileMasks[targetFile];

          if (POSSIBILITY != 0)
          {
            int index = BitOperations.TrailingZeroCount(POSSIBILITY);
            list += $"{index % 8 - 1}{index % 8} E";
          }

          targetFile = (int)Char.GetNumericValue(history[history.Length - 1]); // En Passant to the left
          POSSIBILITY = (board.WP >> 1) & board.BP & RANK_5 & ~FILE_H & FileMasks[targetFile];

          if (POSSIBILITY != 0)
          {
            int index = BitOperations.TrailingZeroCount(POSSIBILITY);
            list += $"{index % 8 + 1}{index % 8} E";
          }
        }

      }

        return list;
    }

    public string WhiteBishopMoves() {
      string list = "";
      long wb = board.WB;
      long i = wb & ~(wb - 1);
      long move;

      while (i != 0) {
        int iLocation = BitOperations.TrailingZeroCount(i);
        move = Diagonal(iLocation);

        long j = move & ~(move - 1);
        while (j!= 0) {
          int index = BitOperations.TrailingZeroCount(j);

          list += $"{iLocation / 8}{iLocation % 8}{index / 8}{index % 8}";
          move &= ~j;
          j = move & (move - 1);
        }

        wb &= ~i;
        i = wb & (wb - 1);
      }

      return list;
    }

    public void PrintBitboards() {
      Console.WriteLine("Rank 1");
      Utils.PrintBitboard(RANK_1);
      Console.WriteLine("--------------------------");
      Console.WriteLine("Rank 2");
      Utils.PrintBitboard(RANK_2);
      Console.WriteLine("--------------------------");
      Console.WriteLine("Rank 3");
      Utils.PrintBitboard(RANK_3);
      Console.WriteLine("--------------------------");
      Console.WriteLine("Rank 4");
      Utils.PrintBitboard(RANK_4);
      Console.WriteLine("--------------------------");
      Console.WriteLine("Rank 5");
      Utils.PrintBitboard(RANK_5);
      Console.WriteLine("--------------------------");
      Console.WriteLine("Rank 6");
      Utils.PrintBitboard(RANK_6);
      Console.WriteLine("--------------------------");
      Console.WriteLine("Rank 7");
      Utils.PrintBitboard(RANK_7);
      Console.WriteLine("--------------------------");
      Console.WriteLine("Rank 8");
      Utils.PrintBitboard(RANK_8);

      Console.WriteLine("==========================");

      Console.WriteLine("File A");
      Utils.PrintBitboard(FILE_A);
      Console.WriteLine("--------------------------");
      Console.WriteLine("File B");
      Utils.PrintBitboard(FILE_B);
      Console.WriteLine("--------------------------");
      Console.WriteLine("File C");
      Utils.PrintBitboard(FILE_C);
      Console.WriteLine("--------------------------");
      Console.WriteLine("File D");
      Utils.PrintBitboard(FILE_D);
      Console.WriteLine("--------------------------");
      Console.WriteLine("File E");
      Utils.PrintBitboard(FILE_E);
      Console.WriteLine("--------------------------");
      Console.WriteLine("File F");
      Utils.PrintBitboard(FILE_F);
      Console.WriteLine("--------------------------");
      Console.WriteLine("File G");
      Utils.PrintBitboard(FILE_G);
      Console.WriteLine("--------------------------");
      Console.WriteLine("File H");
      Utils.PrintBitboard(FILE_H);

      Console.WriteLine("==========================");

      Console.WriteLine("Centre");
      Utils.PrintBitboard(CENTRE);
      Console.WriteLine("--------------------------");
      Console.WriteLine("Extended centre");
      Utils.PrintBitboard(EXTENDED_CENTRE);
      Console.WriteLine("--------------------------");
      Console.WriteLine("King side");
      Utils.PrintBitboard(KING_SIDE);
      Console.WriteLine("Queen side");
      Console.WriteLine("--------------------------");
      Utils.PrintBitboard(QUEEN_SIDE);

      Console.WriteLine("==========================");

      Console.WriteLine("File masks");
      for (int i = 0; i < 8; i++) {
        Utils.PrintBitboard(FileMasks[i]);
        Console.WriteLine("--------------------------");
      }
    }

    public void PrintMoves(string moves) {

      for (int i = 0; i < moves.Length; i += 4) {
        Console.WriteLine($"{moves[i]}{moves[i + 1]} {moves[i + 2]}{moves[i + 3]}");
      }
    }
  }
}