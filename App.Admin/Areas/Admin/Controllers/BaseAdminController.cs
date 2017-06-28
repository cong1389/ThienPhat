using App.FakeEntity.Language;
using App.Service.Language;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class BaseAdminController : Controller
	{
        protected virtual void AddLocales<TLocalizedPropertyViewModelLocal>(ILanguageService languageService, IList<TLocalizedPropertyViewModelLocal> locales) where TLocalizedPropertyViewModelLocal : LocalizedPropertyViewModel
        {
            AddLocales(languageService, locales, null);
        }

        protected virtual void AddLocales<TLocalizedPropertyViewModelLocal>(ILanguageService languageService, IList<TLocalizedPropertyViewModelLocal> locales, Action<TLocalizedPropertyViewModelLocal, int> configure) where TLocalizedPropertyViewModelLocal : LocalizedPropertyViewModel
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

		public BaseAdminController()
		{
		}

		protected string RenderRazorViewToString(string viewName, object model)
		{
			string str;
			base.ViewData.Model = model;
			using (StringWriter stringWriter = new StringWriter())
			{
				ViewEngineResult viewEngineResult = ViewEngines.Engines.FindPartialView(base.ControllerContext, viewName);
				ViewContext viewContext = new ViewContext(base.ControllerContext, viewEngineResult.View, base.ViewData, base.TempData, stringWriter);
				viewEngineResult.View.Render(viewContext, stringWriter);
				viewEngineResult.ViewEngine.ReleaseView(base.ControllerContext, viewEngineResult.View);
				str = stringWriter.GetStringBuilder().ToString();
			}
			return str;
		}
	}
}