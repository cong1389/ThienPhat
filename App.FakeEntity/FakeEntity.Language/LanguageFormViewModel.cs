using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace App.FakeEntity.Language
{
	public class LanguageFormViewModel
	{
		[Display(Name="Code", ResourceType=typeof(FormUI))]
		public string Code
		{
			get;
			set;
		}

		[Display(Name="Image", ResourceType=typeof(FormUI))]
		public HttpPostedFileBase File
		{
			get;
			set;
		}

		public int LangId
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

		[Display(Name="FullName", ResourceType=typeof(FormUI))]
		public string Title
		{
			get;
			set;
		}

		public LanguageFormViewModel()
		{
		}
	}
}