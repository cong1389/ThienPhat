using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App.Aplication.PagedSort.SortUtils
{
	[Serializable]
	[TypeConverter(typeof(SortExpressionConverter))]
	public class SortExpression
	{
		public SortDirection Direction
		{
			get;
			set;
		}

		public string Expression
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public SortExpression()
		{
			this.Title = "";
			this.Expression = "";
			this.Direction = SortDirection.Ascending;
		}

		public SortExpression(string title, string sortExpression, SortDirection direction ) : this()
		{
			this.Title = title;
			this.Expression = sortExpression;
			this.Direction = direction;
		}

		public static SortExpression DeSerialize(string data)
		{
			return (SortExpression)TypeDescriptor.GetConverter(typeof(SortExpression)).ConvertFrom(data);
		}

		public string Serialize()
		{
			return SortExpression.Serialize(this);
		}

		public static string Serialize(SortExpression sortExpression)
		{
			return (string)TypeDescriptor.GetConverter(typeof(SortExpression)).ConvertTo(sortExpression, typeof(string));
		}

		public void ToggleDirection()
		{
			this.Direction = (this.Direction == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending);
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", this.Title, this.Direction);
		}
	}
}