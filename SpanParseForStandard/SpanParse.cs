using System;
using System.Globalization;

namespace SpanParseForStandard
{
	public static class SpanParse
	{
		public static int ParseInt(ReadOnlySpan<char> charSpan)
		{
			var trimmed = charSpan.Trim();
			var result = 0;
			var multiplicator = 1;
			for (var i = trimmed.Length - 1; i >= 0; i--)
			{
				if (i == 0 && trimmed[i] == '-')
				{
					result *= -1;
					break;
				}
				result += multiplicator * CharToInt(trimmed[i]);
				multiplicator *= 10;
			}
			return result;
		}

		public static bool TryParseInt(ReadOnlySpan<char> charSpan, out int value)
		{
			try
			{
				value = ParseInt(charSpan);
				return true;
			}
			catch 
			{
				value = default;
				return false; 
			}
		}

		public static double ParseDouble(ReadOnlySpan<char> charSpan)
		{
			var trimmed = charSpan.Trim();
			var isNegative = trimmed[0] == '-';

			//Find decimal separator
			var separator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.AsSpan();
			var separatorStart = -1;
			for (var i = 0; i < trimmed.Length - separator.Length; i++)
			{
				if (trimmed.Slice(i, separator.Length).Equals(separator, StringComparison.OrdinalIgnoreCase))
				{
					separatorStart = i;
					break;
				}
			}

			// If we have no decimal separator we are basically parsing an integer
			if (separatorStart == -1) return ParseInt(trimmed);

			// Integer part
			double result = ParseInt(trimmed.Slice(0, separatorStart));

			// Decimal part
			var decimalPart = trimmed.Slice(separatorStart + separator.Length);
			var decimals = ParseInt(decimalPart) / Math.Pow(10, decimalPart.Length);

			result += isNegative ? -decimals : decimals; 

			return result;
		}

		public static bool TryParseDouble(ReadOnlySpan<char> charSpan, out double value)
		{
			try
			{
				value = ParseDouble(charSpan); 
				return true;
			}
			catch
			{
				value = default;
				return false;
			}
		}


		private static int CharToInt(char c) => char.IsDigit(c) ? c - 48 : throw new FormatException("ReadOnlySpan<char> was in incorrect format.");
	}
}
