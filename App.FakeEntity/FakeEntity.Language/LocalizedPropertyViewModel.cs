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

        [Display(Name = "LocaleValue", ResourceType = typeof(Resources.FormUI))]
        public string LocaleKey
        {
            get;
            set;
        }

        [Display(Name = "LocaleValue", ResourceType = typeof(FormUI))]
        public string LocaleKeyGroup
        {
            get;
            set;
        }

        [Display(Name = "LocaleValue", ResourceType = typeof(FormUI))]
        public int EntityId
        {
			get;
			set;
		}

        public int Id
        {
            get;
            set;
        }

        [Display(Name= "LocaleValue", ResourceType=typeof(FormUI))]
		public int LanguageId
        {
			get;
			set;
		}

        
        public App.FakeEntity.Language.LanguageFormViewModel Language
        {
            get;
            set;
        }

        public LocalizedPropertyViewModel()
		{
		}
	}
}