using App.SeoSitemap.Common;
using App.SeoSitemap.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace App.SeoSitemap.Videos
{
	public class SitemapVideo
	{
		[XmlElement("category", Order=13)]
		public string Category
		{
			get;
			set;
		}

		[Url]
		[XmlElement("content_loc", Order=4)]
		public string ContentUrl
		{
			get;
			set;
		}

		[XmlElement("description", Order=3)]
		public string Description
		{
			get;
			set;
		}

		[XmlElement("duration", Order=6)]
		public int? Duration
		{
			get;
			set;
		}

		[XmlElement("expiration_date", Order=7)]
		public DateTime? ExpirationDate
		{
			get;
			set;
		}

		[XmlElement("family_friendly", Order=11)]
		public YesNo? FamilyFriendly
		{
			get;
			set;
		}

		[XmlElement("gallery_loc", Order=15)]
		public VideoGallery Gallery
		{
			get;
			set;
		}

		[XmlElement("live", Order=20)]
		public YesNo? Live
		{
			get;
			set;
		}

		[XmlElement("platform", Order=19)]
		public string Platform
		{
			get;
			set;
		}

		[XmlElement("player_loc", Order=5)]
		public VideoPlayer Player
		{
			get;
			set;
		}

		[XmlElement("price", Order=16)]
		public List<VideoPrice> Prices
		{
			get;
			set;
		}

		[XmlElement("publication_date", Order=10)]
		public DateTime? PublicationDate
		{
			get;
			set;
		}

		[XmlElement("rating", Order=8)]
		public decimal? Rating
		{
			get;
			set;
		}

		[XmlElement("requires_subscription", Order=17)]
		public YesNo? RequiresSubscription
		{
			get;
			set;
		}

		[XmlElement("restriction", Order=14)]
		public VideoRestriction Restriction
		{
			get;
			set;
		}

		[XmlElement("tag", Order=12)]
		public string[] Tags
		{
			get;
			set;
		}

		[Url]
		[XmlElement("thumbnail_loc", Order=1)]
		public string ThumbnailUrl
		{
			get;
			set;
		}

		[XmlElement("title", Order=2)]
		public string Title
		{
			get;
			set;
		}

		[XmlElement("uploader", Order=18)]
		public VideoUploader Uploader
		{
			get;
			set;
		}

		[XmlElement("view_count", Order=9)]
		public long? ViewCount
		{
			get;
			set;
		}

		internal SitemapVideo()
		{
		}

		public SitemapVideo(string title, string description, string thumbnailUrl, string contentUrl)
		{
			this.Title = title;
			this.Description = description;
			this.ThumbnailUrl = thumbnailUrl;
			this.ContentUrl = contentUrl;
		}

		public SitemapVideo(string title, string description, string thumbnailUrl, VideoPlayer player)
		{
			this.Title = title;
			this.Description = description;
			this.ThumbnailUrl = thumbnailUrl;
			this.Player = player;
		}

		public bool ShouldSerializeDuration()
		{
			return this.Duration.HasValue;
		}

		public bool ShouldSerializeExpirationDate()
		{
			return this.ExpirationDate.HasValue;
		}

		public bool ShouldSerializeFamilyFriendly()
		{
			return this.FamilyFriendly.HasValue;
		}

		public bool ShouldSerializeLive()
		{
			return this.Live.HasValue;
		}

		public bool ShouldSerializePublicationDate()
		{
			return this.PublicationDate.HasValue;
		}

		public bool ShouldSerializeRating()
		{
			return this.Rating.HasValue;
		}

		public bool ShouldSerializeRequiresSubscription()
		{
			return this.RequiresSubscription.HasValue;
		}

		public bool ShouldSerializeViewCount()
		{
			return this.ViewCount.HasValue;
		}
	}
}