using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.FakeEntity.SeoGlobal
{
	public class SettingSeoGlobalViewModel
	{
		[AllowHtml]
		[Display(Name="FacebookRetargetSnippet", ResourceType=typeof(FormUI))]
		public string FacebookRetargetSnippet
		{
			get;
			set;
		}

		[Display(Name="FbAdminsId", ResourceType=typeof(FormUI))]
		public string FbAdminsId
		{
			get;
			set;
		}

		[Display(Name="FbAppId", ResourceType=typeof(FormUI))]
		public string FbAppId
		{
			get;
			set;
		}

		[AllowHtml]
		[Display(Name="GoogleRetargetSnippet", ResourceType=typeof(FormUI))]
		public string GoogleRetargetSnippet
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		[AllowHtml]
		[Display(Name="MetaTagMasterTool", ResourceType=typeof(FormUI))]
		public string MetaTagMasterTool
		{
			get;
			set;
		}

		[AllowHtml]
		[Display(Name="PublisherGooglePlus", ResourceType=typeof(FormUI))]
		public string PublisherGooglePlus
		{
			get;
			set;
		}

		[AllowHtml]
		[Display(Name="SnippetGoogleAnalytics", ResourceType=typeof(FormUI))]
		public string SnippetGoogleAnalytics
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

		public SettingSeoGlobalViewModel()
		{
		}
	}
}