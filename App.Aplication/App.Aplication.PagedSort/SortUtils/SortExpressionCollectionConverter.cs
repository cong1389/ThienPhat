using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace App.Aplication.PagedSort.SortUtils
{
	public class SortExpressionCollectionConverter : TypeConverter
	{
		public const char SortExpressionDelimiter = ';';

		public SortExpressionCollectionConverter()
		{
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				return new SortExpressionCollection();
			}
			string str = value as string;
			if (str == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			if (str.Length < 1)
			{
				return new SortExpressionCollection();
			}
			string[] strArrays = str.Split(new char[] { ';' });
			if ((int)strArrays.Length < 1)
			{
				return new SortExpressionCollection();
			}
			SortExpressionCollection sortExpressionCollection = new SortExpressionCollection();
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(SortExpression));
			string[] strArrays1 = strArrays;
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				SortExpression sortExpression = converter.ConvertFrom(strArrays1[i]) as SortExpression;
				if (sortExpression != null)
				{
					sortExpressionCollection.Add(sortExpression);
				}
			}
			return sortExpressionCollection;
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value != null && !(value is SortExpressionCollection))
			{
				throw new Exception(string.Format("Unable to convert type '{0}'!", value.GetType()));
			}
			SortExpressionCollection sortExpressionCollection = value as SortExpressionCollection;
			if (destinationType != typeof(string))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(SortExpression));
			foreach (SortExpression sortExpression in sortExpressionCollection)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(';');
				}
				string str = (string)converter.ConvertTo(sortExpression, typeof(string));
				stringBuilder.Append(str);
			}
			return stringBuilder.ToString();
		}
	}
}