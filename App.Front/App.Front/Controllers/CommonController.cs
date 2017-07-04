using App.Service.Common;
using App.Service.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public  partial class CommonController : FrontBaseController
    {
        private readonly ICommonServices _services;

        private readonly ILanguageService _language;

        public CommonController(ICommonServices services, ILanguageService language)
        {
            _services = services;
            _language = language;
        }

        public ActionResult SetLanguage(int langid, string returnUrl = "")
        {
            var language = _language.GetLanguageById(langid);
            if (language != null && language.Status == 1)
            {
                _services.WorkContext.WorkingLanguage = language;
            }

            return Redirect(returnUrl);
        }

    }
}