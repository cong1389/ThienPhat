using App.SeoSitemap.Common;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace App.SeoSitemap
{
	[XmlRoot("sitemap", Namespace="http://www.sitemaps.org/schemas/sitemap/0.9")]
	public class SitemapIndexNode
	{
		[XmlElement("lastmod", Order=2)]
		public DateTime? LastModificationDate
		{
			get;
			set;
		}

		[Url]
		[XmlElement("loc", Order=1)]
		public string Url
		{
			get;
			set;
		}

		internal SitemapIndexNode()
		{
		}

		public SitemapIndexNode(string url)
		{
			this.Url = url;
		}

		public bool ShouldSerializeLastModificationDate()
		{
			return this.LastModificationDate.HasValue;
		}
	}
}