using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace App.FakeEntity.System
{
	public class SystemSettingViewModel
	{
		[Display(Name="Description", ResourceType=typeof(FormUI))]
		public string Description
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string Favicon
		{
			get;
			set;
		}

		[AllowHtml]
		[Display(Name="FooterContent", ResourceType=typeof(FormUI))]
		public string FooterContent
		{
			get;
			set;
		}

		public string Hotline
		{
			get;
			set;
		}

		[Display(Name="Favicon", ResourceType=typeof(FormUI))]
		public HttpPostedFileBase Icon
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string Language
		{
			get;
			set;
		}

		[Display(Name="LogoImage", ResourceType=typeof(FormUI))]
		public HttpPostedFileBase Logo
		{
			get;
			set;
		}

		public string LogoImage
		{
			get;
			set;
		}

		[Display(Name="MaintanceSite", ResourceType=typeof(FormUI))]
		public bool MaintanceSite
		{
			get;
			set;
		}

		[Display(Name="MetaDescription", ResourceType=typeof(FormUI))]
		public string MetaDescription
		{
			get;
			set;
		}

		[Display(Name="MetaKeywords", ResourceType=typeof(FormUI))]
		public string MetaKeywords
		{
			get;
			set;
		}

		[Display(Name="MetaTitle", ResourceType=typeof(FormUI))]
		public string MetaTitle
		{
			get;
			set;
		}

		[Display(Name="Slogan")]
		public string Slogan
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

		[Display(Name="TimeWork", ResourceType=typeof(FormUI))]
		public string TimeWork
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

		public SystemSettingViewModel()
		{
		}
	}
}