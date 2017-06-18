using App.SeoSitemap.Common;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace App.SeoSitemap.Translations
{
	public class SitemapPageTranslation
	{
		[XmlAttribute("hreflang")]
		public string Language
		{
			get;
			set;
		}

		[XmlAttribute("rel")]
		public string Rel
		{
			get;
			set;
		}

		[Url]
		[XmlAttribute("href")]
		public string Url
		{
			get;
			set;
		}

		internal SitemapPageTranslation()
		{
		}

		public SitemapPageTranslation(string url, string language, string rel = "alternate")
		{
			this.Url = url;
			this.Language = language;
			this.Rel = rel;
		}
	}
}