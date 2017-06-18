using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace App.Utils.SEO
{
	public class Location
	{
		[XmlElement("changefreq")]
		public Location.eChangeFrequency? ChangeFrequency
		{
			get;
			set;
		}

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

		public Location()
		{
		}

		public bool ShouldSerializeChangeFrequency()
		{
			return this.ChangeFrequency.HasValue;
		}

		public bool ShouldSerializeLastModified()
		{
			return !string.IsNullOrEmpty(this.LastModified);
		}

		public bool ShouldSerializePriority()
		{
			return this.Priority.HasValue;
		}

		public enum eChangeFrequency
		{
			always,
			hourly,
			daily,
			weekly,
			monthly,
			yearly,
			never
		}
	}
}