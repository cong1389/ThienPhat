using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace App.SeoSitemap.Common
{
	internal class UrlPropertyModel
	{
		public List<PropertyInfo> ClassProperties
		{
			get;
		}

		public List<PropertyInfo> EnumerableProperties
		{
			get;
		}

		public List<PropertyInfo> UrlProperties
		{
			get;
		}

		public UrlPropertyModel()
		{
			this.UrlProperties = new List<PropertyInfo>();
			this.EnumerableProperties = new List<PropertyInfo>();
			this.ClassProperties = new List<PropertyInfo>();
		}
	}
}