using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace App.Core.Utils
{
	public class PaginationSet<T>
	{
		public int Count
		{
			get
			{
				return (this.Items != null ? this.Items.Count<T>() : 0);
			}
		}

		public IEnumerable<T> Items
		{
			get;
			set;
		}

		public int Page
		{
			get;
			set;
		}

		public int TotalCount
		{
			get;
			set;
		}

		public int TotalPages
		{
			get;
			set;
		}

		public PaginationSet()
		{
		}
	}
}