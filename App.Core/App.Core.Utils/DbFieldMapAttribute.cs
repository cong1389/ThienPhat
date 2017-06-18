using System;
using System.Runtime.CompilerServices;

namespace App.Core.Utils
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DbFieldMapAttribute : Attribute
	{
		public string Field
		{
			get;
			set;
		}

		public DbFieldMapAttribute(string field)
		{
			this.Field = field;
		}
	}
}