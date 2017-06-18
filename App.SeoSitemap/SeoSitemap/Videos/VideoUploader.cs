using App.SeoSitemap.Common;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace App.SeoSitemap.Videos
{
	public class VideoUploader
	{
		[Url]
		[XmlAttribute("info")]
		public string Info
		{
			get;
			set;
		}

		[XmlText]
		public string Name
		{
			get;
			set;
		}

		internal VideoUploader()
		{
		}

		public VideoUploader(string name)
		{
			this.Name = name;
		}
	}
}