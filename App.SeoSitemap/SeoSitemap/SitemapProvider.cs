using App.SeoSitemap.Common;
using System;
using System.Web.Mvc;

namespace App.SeoSitemap
{
	public class SitemapProvider : ISitemapProvider
	{
		private readonly IBaseUrlProvider _baseUrlProvider;

		public SitemapProvider()
		{
		}

		public SitemapProvider(IBaseUrlProvider baseUrlProvider)
		{
			this._baseUrlProvider = baseUrlProvider;
		}

		public ActionResult CreateSitemap(SitemapModel sitemapModel)
		{
			if (sitemapModel == null)
			{
				throw new ArgumentNullException("sitemapModel");
			}
			return new XmlResult<SitemapModel>(sitemapModel, this._baseUrlProvider);
		}

		public ActionResult CreateSitemapIndex(SitemapIndexModel sitemapIndexModel)
		{
			if (sitemapIndexModel == null)
			{
				throw new ArgumentNullException("sitemapIndexModel");
			}
			return new XmlResult<SitemapIndexModel>(sitemapIndexModel, this._baseUrlProvider);
		}
	}
}