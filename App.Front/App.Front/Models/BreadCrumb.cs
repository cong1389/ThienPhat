using System;
using System.Runtime.CompilerServices;

namespace App.Front.Models
{
	public class BreadCrumb
	{
		public bool Current
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public BreadCrumb()
		{
		}
	}
}