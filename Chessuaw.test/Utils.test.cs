using System;
using Xunit;

namespace Chessuaw.test
{
	public class Utils
	{
		[Fact]
		public void StringToBitboard_BinaryString_ReturnsLongValue()
		{
			// Act
			string value = "0000000100000001000000010000000100000001000000010000000100000001";
			long result = Chessuaw.Utils.StringToBitboard(value);
			long actual = 72340172838076673;

			// Assert
			Assert.Equal(actual, result);
		}

		[Fact]
		public void StringToBitboard_NegativeValue_ReturnsLongValue()
		{
			// Act
			string value = "1111111100000000000000000000000000000000000000000000000000000000";
			long result = Chessuaw.Utils.StringToBitboard(value);
			long expected = -72057594037927936;

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void ReverseLong_Zero_ReturnsZero()
		{
			long value = 0;
			long result = Chessuaw.Utils.ReverseLong(value);
			long expected = 0;

			Assert.Equal(expected, result);
		}

		[Fact]
		public void ReverseLong_Positive_ReturnsReversed()
		{
			long value = 4294967296;
			long result = Chessuaw.Utils.ReverseLong(value);
			long expected = 2147483648;

			Assert.Equal(expected, result);
		}

		[Fact]
		public void ReverseLong_Negative_ReturnsReversed()
		{
			long value = -10;
			long result = Chessuaw.Utils.ReverseLong(value);
			long expected = 8070450532247928831;

			Assert.Equal(expected, result);
		}
	}
}
