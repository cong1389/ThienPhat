using System;

namespace App.Admin.Helpers
{
	public static class AdminHelper
	{
		public static string SplitWords(int lenght, string words)
		{
			if (string.IsNullOrEmpty(words))
			{
				return string.Empty;
			}
			if (words.Length < lenght)
			{
				return words;
			}
			return string.Concat(words.Substring(0, lenght), "...");
		}
	}
}