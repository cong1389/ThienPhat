using App.SeoSitemap.StyleSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace App.SeoSitemap
{
	public abstract class SitemapIndexConfiguration<T> : ISitemapIndexConfiguration<T>
	{
		public int? CurrentPage
		{
			get;
		}

		public IQueryable<T> DataSource
		{
			get;
		}

		public List<XmlStyleSheet> SitemapIndexStyleSheets
		{
			get
			{
				return JustDecompileGenerated_get_SitemapIndexStyleSheets();
			}
			set
			{
				JustDecompileGenerated_set_SitemapIndexStyleSheets(value);
			}
		}

		private List<XmlStyleSheet> JustDecompileGenerated_SitemapIndexStyleSheets_k__BackingField;

		public List<XmlStyleSheet> JustDecompileGenerated_get_SitemapIndexStyleSheets()
		{
			return this.JustDecompileGenerated_SitemapIndexStyleSheets_k__BackingField;
		}

		protected void JustDecompileGenerated_set_SitemapIndexStyleSheets(List<XmlStyleSheet> value)
		{
			this.JustDecompileGenerated_SitemapIndexStyleSheets_k__BackingField = value;
		}

		public List<XmlStyleSheet> SitemapStyleSheets
		{
			get
			{
				return JustDecompileGenerated_get_SitemapStyleSheets();
			}
			set
			{
				JustDecompileGenerated_set_SitemapStyleSheets(value);
			}
		}

		private List<XmlStyleSheet> JustDecompileGenerated_SitemapStyleSheets_k__BackingField;

		public List<XmlStyleSheet> JustDecompileGenerated_get_SitemapStyleSheets()
		{
			return this.JustDecompileGenerated_SitemapStyleSheets_k__BackingField;
		}

		protected void JustDecompileGenerated_set_SitemapStyleSheets(List<XmlStyleSheet> value)
		{
			this.JustDecompileGenerated_SitemapStyleSheets_k__BackingField = value;
		}

		public int Size
		{
			get
			{
				return JustDecompileGenerated_get_Size();
			}
			set
			{
				JustDecompileGenerated_set_Size(value);
			}
		}

		private int JustDecompileGenerated_Size_k__BackingField;

		public int JustDecompileGenerated_get_Size()
		{
			return this.JustDecompileGenerated_Size_k__BackingField;
		}

		protected void JustDecompileGenerated_set_Size(int value)
		{
			this.JustDecompileGenerated_Size_k__BackingField = value;
		}

		public bool UseReverseOrderingForSitemapIndexNodes
		{
			get
			{
				return JustDecompileGenerated_get_UseReverseOrderingForSitemapIndexNodes();
			}
			set
			{
				JustDecompileGenerated_set_UseReverseOrderingForSitemapIndexNodes(value);
			}
		}

		private bool JustDecompileGenerated_UseReverseOrderingForSitemapIndexNodes_k__BackingField;

		public bool JustDecompileGenerated_get_UseReverseOrderingForSitemapIndexNodes()
		{
			return this.JustDecompileGenerated_UseReverseOrderingForSitemapIndexNodes_k__BackingField;
		}

		protected void JustDecompileGenerated_set_UseReverseOrderingForSitemapIndexNodes(bool value)
		{
			this.JustDecompileGenerated_UseReverseOrderingForSitemapIndexNodes_k__BackingField = value;
		}

		protected SitemapIndexConfiguration(IQueryable<T> dataSource, int? currentPage)
		{
			this.DataSource = dataSource;
			this.CurrentPage = currentPage;
			this.Size = 50000;
		}

		public abstract SitemapNode CreateNode(T source);

		public abstract SitemapIndexNode CreateSitemapIndexNode(int currentPage);
	}
}