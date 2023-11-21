using SpanParseForStandard;
using System.Globalization;

namespace Tests;

public class ParseTests
{
	[Fact]
	public void TestSpanToDouble()
	{
		var testData = new Dictionary<string, double> {
			{ "12.5", 12.5 }, { "123.45", 123.45 }, { "12345.6789", 12345.6789 }, { "-1.0", -1.0 }, { "-5023.12", -5023.12 }, { "0.198", 0.198 }, { "-0.00123", -0.00123 },
			{ "-1", -1 }, { "52", 52 }, { "-5213", -5213 }, { ".55", 0.55 }, { "-.68", -0.68 }
		};
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; //Decimal separator is dot (.)
		Assert.All(testData, kv =>
		{
			ReadOnlySpan<char> span = kv.Key;
			Assert.Equal(kv.Value, SpanParse.ParseDouble(span), 0.0001);
		});
	}

	[Fact]
	public void TestSpanToInt()
	{
		var testData = new Dictionary<string, double> {
				{ "12", 12 }, { "123", 123 }, { "12345", 12345 }, { "-1", -1 }, { "-5023", -5023 }, { "0", 0 }, { "-0", 0 }
		};
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; //Decimal separator is dot (.)
		Assert.All(testData, kv =>
		{
			ReadOnlySpan<char> span = kv.Key;
			Assert.Equal(kv.Value, SpanParse.ParseInt(span), 0.0001);
		});
	}
}