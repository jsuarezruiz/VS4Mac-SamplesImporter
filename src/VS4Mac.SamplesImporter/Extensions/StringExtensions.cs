using System;

namespace VS4Mac.SamplesImporter.Extensions
{
	public static class StringExtensions
	{
		public static bool Contains(this string source, string value, StringComparison comparisonType)
		{
			return source?.IndexOf(value, comparisonType) >= 0;
		}
	}
}