using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace App.FakeEntity.Slide
{
	public class SlideShowViewModel
	{
		[AllowHtml]
		[Display(Name="Description", ResourceType=typeof(FormUI))]
		public string Description
		{
			get;
			set;
		}

		[Display(Name="FromDate", ResourceType=typeof(FormUI))]
		public TimeSpan? FromDate
		{
			get;
			set;
		}

		[Display(Name="Height", ResourceType=typeof(FormUI))]
		public string Height
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		[Display(Name="ImageUrl", ResourceType=typeof(FormUI))]
		public HttpPostedFileBase Image
		{
			get;
			set;
		}

		[Display(Name="ImageUrl", ResourceType=typeof(FormUI))]
		public string ImgPath
		{
			get;
			set;
		}

		[Display(Name="OrderDisplay", ResourceType=typeof(FormUI))]
		public int OrderDisplay
		{
			get;
			set;
		}

		[Display(Name="Status", ResourceType=typeof(FormUI))]
		public int Status
		{
			get;
			set;
		}

		[Display(Name="Target", ResourceType=typeof(FormUI))]
		public string Target
		{
			get;
			set;
		}

		[Display(Name="FullName", ResourceType=typeof(FormUI))]
		public string Title
		{
			get;
			set;
		}

		[Display(Name="ToDate", ResourceType=typeof(FormUI))]
		public TimeSpan? ToDate
		{
			get;
			set;
		}

		public bool Video
		{
			get;
			set;
		}

		[Display(Name="WebsiteLink", ResourceType=typeof(FormUI))]
		public string WebsiteLink
		{
			get;
			set;
		}

		[Display(Name="Width", ResourceType=typeof(FormUI))]
		public string Width
		{
			get;
			set;
		}

		public SlideShowViewModel()
		{
		}
	}
}