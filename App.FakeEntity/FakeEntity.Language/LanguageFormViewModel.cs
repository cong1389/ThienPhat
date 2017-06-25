using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace App.FakeEntity.Language
{
	public class LanguageFormViewModel
	{
		[Display(Name= "LanguageCode", ResourceType=typeof(Resources.FormUI))]
		public string LanguageCode
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

		[Display(Name= "LanguageName", ResourceType=typeof(FormUI))]
		public string LanguageName
        {
			get;
			set;
		}

		public LanguageFormViewModel()
		{
		}
	}
}