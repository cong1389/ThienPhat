using App.Core.Localization;
using App.Service.Common;
using App.Service.Language;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Mvc;


namespace App.Front.Controllers
{
	public  class FrontBaseController : Controller
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
				return 20;
			}
		}

        public ICommonServices Services
        {
            get;
            set;
        }

        public FrontBaseController()
		{
            this.T = NullLocalizer.Instance;
        }

        public Localizer T
        {
            get;
            set;
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages

            // Validate culture name
            cultureName = Helpers.CultureHelper.GetImplementedCulture(cultureName); // This is safe


            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;


            return base.BeginExecuteCore(callback, state);
        }
    }
}