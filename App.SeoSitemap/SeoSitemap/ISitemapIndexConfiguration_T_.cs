using App.SeoSitemap.StyleSheets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.SeoSitemap
{
	public interface ISitemapIndexConfiguration<T>
	{
		int? CurrentPage
		{
			get;
		}

		IQueryable<T> DataSource
		{
			get;
		}

		List<XmlStyleSheet> SitemapIndexStyleSheets
		{
			get;
		}

		List<XmlStyleSheet> SitemapStyleSheets
		{
			get;
		}

		int Size
		{
			get;
		}

		bool UseReverseOrderingForSitemapIndexNodes
		{
			get;
		}

		SitemapNode CreateNode(T source);

		SitemapIndexNode CreateSitemapIndexNode(int currentPage);
	}
}