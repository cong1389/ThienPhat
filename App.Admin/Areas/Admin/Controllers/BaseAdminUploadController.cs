using App.Service.Language;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class BaseAdminUploadController : Controller
	{
        protected virtual void AddLocales<TLocalizedPropertyViewModelLocal>(ILanguageService languageService, IList<TLocalizedPropertyViewModelLocal> locales) where TLocalizedPropertyViewModelLocal : ILocalizedModelLocal
        {
            AddLocales(languageService, locales, null);
        }

        protected virtual void AddLocales<TLocalizedPropertyViewModelLocal>(ILanguageService languageService, IList<TLocalizedPropertyViewModelLocal> locales, Action<TLocalizedPropertyViewModelLocal, int> configure) where TLocalizedPropertyViewModelLocal : ILocalizedModelLocal
        {
            foreach (var language in languageService.GetAll())
            {
                var locale = Activator.CreateInstance<TLocalizedPropertyViewModelLocal>();
                locale.LanguageId = language.Id;
                if (configure != null)
                {
                    configure.Invoke(locale, locale.LanguageId);
                }
                locales.Add(locale);
            }
        }

        public int _pageSize
		{
			get
			{
				return int.Parse(ConfigurationManager.AppSettings["ItemsPerPage"] ?? "10");
			}
		}

		protected string Key
		{
			get
			{
				return base.ViewData["Key"].ToString();
			}
			set
			{
				base.ViewData["Key"] = value;
			}
		}

		public BaseAdminUploadController()
		{
		}
	}
}