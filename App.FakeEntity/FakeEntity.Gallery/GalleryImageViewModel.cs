using System;
using System.Runtime.CompilerServices;

namespace App.FakeEntity.Gallery
{
	public class GalleryImageViewModel
	{
		public int AttributeValueId
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string ImagePath
		{
			get;
			set;
		}

		public string ImageThumbnail
		{
			get;
			set;
		}

		public int OrderDisplay
		{
			get;
			set;
		}

		public int PostId
		{
			get;
			set;
		}

		public double? Price
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public GalleryImageViewModel()
		{
		}
	}
}