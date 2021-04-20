using System;
using System.Diagnostics;
using System.Numerics;

namespace Chessuaw {
  public class Utils {
    public static long StringToBitboard(string str)
    {
      if (str[0] == '0')
      {
        return Convert.ToInt64(str, 2);
      }
      else
      {
        return Convert.ToInt64(str, 2);
      }
    }

    public static long Benchmark(Func<string, string> f, int times) {
      Stopwatch stopwatch = new Stopwatch();

      stopwatch.Start();
      for (int i = 0; i < times; i++) {
        f("");
      }
      stopwatch.Stop();

      return stopwatch.ElapsedMilliseconds;
    }

    public static long ReverseLong(long num) {
      int count = 63;
      long reverseNum = num;
      bool negative = false;

      if (num < 0) {
        num = ~num;
        negative = true;
      }
      num >>= 1;

      while (num != 0) {
        reverseNum <<= 1;
        reverseNum |= num & 1;
        num >>= 1;
        count--;
      }

      reverseNum <<= count;

      return negative ? ~reverseNum :reverseNum;
    }

    public static void PrintBitboard(long bitboard)
    {
      string binary = Convert.ToString(bitboard, 2);
      binary = new string('0', BitOperations.LeadingZeroCount((ulong)bitboard)) + binary;

      for (int i = 0; i < 8; i++)
      {
        Console.WriteLine(binary.Substring(i * 8, 8));
      }
    }
  }
}