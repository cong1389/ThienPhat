using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace App.SeoSitemap
{
	public class DynamicSitemapIndexProvider : IDynamicSitemapIndexProvider
	{
		public DynamicSitemapIndexProvider()
		{
		}

		private ActionResult CreateSitemap<T>(ISitemapProvider sitemapProvider, ISitemapIndexConfiguration<T> sitemapIndexConfiguration, List<T> items)
		{
			ISitemapIndexConfiguration<T> sitemapIndexConfiguration1 = sitemapIndexConfiguration;
			List<SitemapNode> list = items.Select<T, SitemapNode>(new Func<T, SitemapNode>(sitemapIndexConfiguration1.CreateNode)).ToList<SitemapNode>();
			return sitemapProvider.CreateSitemap(new SitemapModel(list)
			{
				StyleSheets = sitemapIndexConfiguration.SitemapStyleSheets
			});
		}

		public ActionResult CreateSitemapIndex<T>(ISitemapProvider sitemapProvider, ISitemapIndexConfiguration<T> sitemapIndexConfiguration)
		{
			if (sitemapProvider == null)
			{
				throw new ArgumentNullException("sitemapProvider");
			}
			if (sitemapIndexConfiguration == null)
			{
				throw new ArgumentNullException("sitemapIndexConfiguration");
			}
			int num = sitemapIndexConfiguration.DataSource.Count<T>();
			if (sitemapIndexConfiguration.Size >= num)
			{
				return this.CreateSitemap<T>(sitemapProvider, sitemapIndexConfiguration, sitemapIndexConfiguration.DataSource.ToList<T>());
			}
			if (!sitemapIndexConfiguration.CurrentPage.HasValue || sitemapIndexConfiguration.CurrentPage.Value <= 0)
			{
				int num1 = (int)Math.Ceiling((double)num / (double)sitemapIndexConfiguration.Size);
				return sitemapProvider.CreateSitemapIndex(this.CreateSitemapIndex<T>(sitemapIndexConfiguration, num1));
			}
			int? currentPage = sitemapIndexConfiguration.CurrentPage;
			int value = (currentPage.Value - 1) * sitemapIndexConfiguration.Size;
			List<T> list = sitemapIndexConfiguration.DataSource.Skip<T>(value).Take<T>(sitemapIndexConfiguration.Size).ToList<T>();
			return this.CreateSitemap<T>(sitemapProvider, sitemapIndexConfiguration, list);
		}

		private SitemapIndexModel CreateSitemapIndex<T>(ISitemapIndexConfiguration<T> sitemapIndexConfiguration, int sitemapCount)
		{
			IEnumerable<int> nums = Enumerable.Range(1, sitemapCount);
			if (sitemapIndexConfiguration.UseReverseOrderingForSitemapIndexNodes)
			{
				nums = nums.Reverse<int>();
			}
			ISitemapIndexConfiguration<T> sitemapIndexConfiguration1 = sitemapIndexConfiguration;
			return new SitemapIndexModel(nums.Select<int, SitemapIndexNode>(new Func<int, SitemapIndexNode>(sitemapIndexConfiguration1.CreateSitemapIndexNode)).ToList<SitemapIndexNode>())
			{
				StyleSheets = sitemapIndexConfiguration.SitemapIndexStyleSheets
			};
		}
	}
}