using App.SeoSitemap.StyleSheets;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace App.SeoSitemap
{
	[XmlRoot("sitemapindex", Namespace="http://www.sitemaps.org/schemas/sitemap/0.9")]
	public class SitemapIndexModel : IHasStyleSheets
	{
		[XmlElement("sitemap")]
		public List<SitemapIndexNode> Nodes
		{
			get;
		}

		[XmlIgnore]
		public List<XmlStyleSheet> StyleSheets
		{
			get;
			set;
		}

		internal SitemapIndexModel()
		{
		}

		public SitemapIndexModel(List<SitemapIndexNode> nodes)
		{
			this.Nodes = nodes;
		}
	}
}