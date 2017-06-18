using System;

namespace App.Front.Models
{
	public static class FrontHelper
	{
		public static string SplitWords(int lenght, string words)
		{
			if (words.Length < lenght)
			{
				return words;
			}
			return string.Concat(words.Substring(0, lenght), "...");
		}
	}
}