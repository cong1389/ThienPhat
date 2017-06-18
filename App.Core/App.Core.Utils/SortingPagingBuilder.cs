using System;
using System.Runtime.CompilerServices;

namespace App.Core.Utils
{
	public class SortingPagingBuilder
	{
		public string Keywords
		{
			get;
			set;
		}

		public SortBuilder Sorts
		{
			get;
			set;
		}

		public SortingPagingBuilder()
		{
		}
	}
}