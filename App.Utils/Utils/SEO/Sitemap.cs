using System;
using System.Collections;
using System.Xml.Serialization;

namespace App.Utils.SEO
{
	[XmlRoot("urlset", Namespace="http://www.sitemaps.org/schemas/sitemap/0.9")]
	public class Sitemap
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

		public Sitemap()
		{
			this.map = new ArrayList();
		}

		public int Add(Location item)
		{
			return this.map.Add(item);
		}
	}
}