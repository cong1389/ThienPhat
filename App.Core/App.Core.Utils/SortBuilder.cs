using System;
using System.Runtime.CompilerServices;

namespace App.Core.Utils
{
	public class SortBuilder
	{
		public string ColumnName
		{
			get;
			set;
		}

		public SortBuilder.SortOrder ColumnOrder
		{
			get;
			set;
		}

		public SortBuilder()
		{
		}

		public enum SortOrder
		{
			Ascending,
			Descending
		}
	}
}