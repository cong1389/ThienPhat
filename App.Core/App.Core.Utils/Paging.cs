using System;
using System.Runtime.CompilerServices;

namespace App.Core.Utils
{
	public class Paging
	{
		public int PageNumber
		{
			get;
			set;
		}

		public int PageSize
		{
			get;
			set;
		}

		public int TotalRecord
		{
			get;
			set;
		}

		public Paging()
		{
		}
	}
}