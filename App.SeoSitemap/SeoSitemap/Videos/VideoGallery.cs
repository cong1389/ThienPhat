using App.SeoSitemap.Common;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace App.SeoSitemap.Videos
{
	public class VideoGallery
	{
		[XmlAttribute("title")]
		public string Title
		{
			get;
			set;
		}

		[Url]
		[XmlText]
		public string Url
		{
			get;
			set;
		}

		internal VideoGallery()
		{
		}

		public VideoGallery(string url)
		{
			this.Url = url;
		}
	}
}