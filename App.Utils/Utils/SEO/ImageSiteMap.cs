using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace App.Utils.SEO
{
	public class ImageSiteMap
	{
		private readonly ArrayList map;

		[XmlElement("url")]
		public Location[] Locations
		{
			get
			{
				Location[] locationArray = new Location[this.map.Count];
				this.map.CopyTo(locationArray);
				return locationArray;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this.map.Clear();
				Location[] locationArray = value;
				for (int i = 0; i < (int)locationArray.Length; i++)
				{
					Location location = locationArray[i];
					this.map.Add(location);
				}
			}
		}

		public ImageSiteMap()
		{
			this.map = new ArrayList();
		}

		public class ImageInfo
		{
			[XmlElement("lastmod")]
			public string LastModified
			{
				get;
				set;
			}

			[XmlElement("priority")]
			public double? Priority
			{
				get;
				set;
			}

			[XmlElement("loc")]
			public string Url
			{
				get;
				set;
			}

			public ImageInfo()
			{
			}

			public bool ShouldSerializeLastModified()
			{
				return !string.IsNullOrEmpty(this.LastModified);
			}

			public bool ShouldSerializePriority()
			{
				return this.Priority.HasValue;
			}
		}
	}
}