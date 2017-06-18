using App.SeoSitemap.Serialization;
using App.SeoSitemap.StyleSheets;
using App.SeoSitemap.Translations;
using App.Utils.SEO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace App.SeoSitemap
{
	[XmlRoot("urlset", Namespace="http://www.sitemaps.org/schemas/sitemap/0.9")]
	public class SitemapModel : IXmlNamespaceProvider, IHasStyleSheets
	{
		[XmlElement("url")]
		public List<SitemapNode> Nodes
		{
			get;
		}

		[XmlIgnore]
		public List<XmlStyleSheet> StyleSheets
		{
			get;
			set;
		}

		internal SitemapModel()
		{
		}

		public SitemapModel(List<SitemapNode> nodes)
		{
			this.Nodes = nodes;
		}

		public IEnumerable<string> GetNamespaces()
		{
			if (this.Nodes == null)
			{
				yield break;
			}
			List<SitemapNode> nodes = this.Nodes;
            //Test
			//if (nodes.Any<SitemapNode>((SitemapNode node) => {
			//	if (node.Images == null)
			//	{
			//		return false;
			//	}
			//	return node.Images.Any<SiteMapImage>();
			//}))
			//{
			//	yield return "http://www.google.com/schemas/sitemap-image/1.1";
			//}
			List<SitemapNode> sitemapNodes = this.Nodes;
			if (sitemapNodes.Any<SitemapNode>((SitemapNode node) => node.News != null))
			{
				yield return "http://www.google.com/schemas/sitemap-news/0.9";
			}
			List<SitemapNode> nodes1 = this.Nodes;
			if (nodes1.Any<SitemapNode>((SitemapNode node) => node.Video != null))
			{
				yield return "http://www.google.com/schemas/sitemap-video/1.1";
			}
			List<SitemapNode> sitemapNodes1 = this.Nodes;
			if (sitemapNodes1.Any<SitemapNode>((SitemapNode node) => node.Mobile != null))
			{
				yield return "http://www.google.com/schemas/sitemap-mobile/1.0";
			}
			List<SitemapNode> nodes2 = this.Nodes;
			if (nodes2.Any<SitemapNode>((SitemapNode node) => {
				if (node.Translations == null)
				{
					return false;
				}
				return node.Translations.Any<SitemapPageTranslation>();
			}))
			{
				yield return "http://www.w3.org/1999/xhtml";
			}
		}
	}
}