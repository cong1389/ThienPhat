using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace App.SeoSitemap.Serialization
{
	internal class XmlNamespaceBuilder : IXmlNamespaceBuilder
	{
		private readonly IDictionary<string, string> prefixList;

		public XmlNamespaceBuilder()
		{
			this.prefixList = new Dictionary<string, string>()
			{
				{ "http://www.sitemaps.org/schemas/sitemap/0.9", "" },
				{ "http://www.google.com/schemas/sitemap-image/1.1", "image" },
				{ "http://www.google.com/schemas/sitemap-news/0.9", "news" },
				{ "http://www.google.com/schemas/sitemap-video/1.1", "video" },
				{ "http://www.google.com/schemas/sitemap-mobile/1.0", "mobile" },
				{ "http://www.w3.org/1999/xhtml", "xhtml" }
			};
		}

		public XmlSerializerNamespaces Create(IEnumerable<string> namespaces)
		{
			string str;
			XmlSerializerNamespaces xmlSerializerNamespace = new XmlSerializerNamespaces();
			xmlSerializerNamespace.Add("", "http://www.sitemaps.org/schemas/sitemap/0.9");
			foreach (string @namespace in namespaces)
			{
				if (!this.prefixList.TryGetValue(@namespace, out str))
				{
					continue;
				}
				xmlSerializerNamespace.Add(str, @namespace);
			}
			return xmlSerializerNamespace;
		}
	}
}