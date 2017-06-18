using App.SeoSitemap.Common;
using App.SeoSitemap.Enum;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace App.SeoSitemap.Videos
{
	public class VideoPlayer
	{
		[XmlAttribute("allow_embed")]
		public YesNo AllowEmbed
		{
			get;
			set;
		}

		[XmlAttribute("autoplay")]
		public string Autoplay
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

		internal VideoPlayer()
		{
		}

		public VideoPlayer(string url)
		{
			this.Url = url;
		}

		public bool ShouldSerializeAllowEmbed()
		{
			return this.AllowEmbed != YesNo.None;
		}
	}
}