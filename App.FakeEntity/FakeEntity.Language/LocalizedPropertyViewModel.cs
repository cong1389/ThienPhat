using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace App.FakeEntity.Language
{
	public class LocalizedPropertyViewModel
	{
		[Display(Name= "LocaleValue", ResourceType=typeof(Resources.FormUI))]
		public string LocaleValue
        {
			get;
			set;
		}

        [Display(Name = "LocaleKey", ResourceType = typeof(Resources.FormUI))]
        public string LocaleKey
        {
            get;
            set;
        }

        [Display(Name = "LocaleKeyGroup", ResourceType = typeof(FormUI))]
        public string LocaleKeyGroup
        {
            get;
            set;
        }

        [Display(Name = "EntityId", ResourceType = typeof(FormUI))]
        public int EntityId
        {
			get;
			set;
		}

		[Display(Name= "LanguageId", ResourceType=typeof(FormUI))]
		public int LanguageId
        {
			get;
			set;
		}

		

		public LocalizedPropertyViewModel()
		{
		}
	}
}