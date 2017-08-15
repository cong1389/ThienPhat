using App.Admin.Controllers;
using App.Domain.Entities.Language;
using App.FakeEntity.Language;
using App.Service.Common;
using App.Service.Language;
using App.Service.LocaleStringResource;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class LocaleResourceController : BaseAdminController
    {
        private readonly ILanguageService _langService;

        private readonly ILocaleStringResourceService _localeStringResourceService;

        private readonly ICommonServices _services;

        public LocaleResourceController(ICommonServices services, ILocaleStringResourceService localeStringResourceService, ILanguageService langService)
        {
            this._langService = langService;
            this._services = services;
            this._localeStringResourceService = localeStringResourceService;
        }

        public ActionResult Index(int languageId, int page = 1, string keywords = "")
        {
            var resources = _services.Localization.GetByLanguageId(languageId);
            resources = resources.Where(m => m.ResourceName.Contains(keywords) || m.ResourceValue.Contains(keywords));
            ViewBag.Localization = resources.OrderByDescending(m => m.CreatedDate);

            //Lưu lại languageId, keywork để k bị mất value text ở view
            ViewBag.LanguageSelected = languageId;
            ViewBag.keywords = keywords;

            IEnumerable<Language> all = _langService.GetAll();
            ((dynamic)base.ViewBag).AllLanguage = all;

            return base.View(resources);
        }

        public ActionResult Resource(int languageId)
        {
            return base.View();
        }

        public ActionResult Create(LocaleStringResourceViewModel model)
        {
            var res = _services.Localization.GetLocaleStringResourceByName(model.LanguageId, model.ResourceName);
            return base.View();
        }

        public ActionResult Edit(LocaleStringResourceViewModel model)
        {
            LocaleStringResource locale = _services.Localization.GetById(model.Id);

            if (locale != null)
            {
                model.LanguageId = locale.LanguageId;
                model.IsFromPlugin = locale.IsFromPlugin;
                model.IsTouched = true;

                LocaleStringResource localeByMap = Mapper.Map<LocaleStringResourceViewModel, LocaleStringResource>(model, locale);
                _localeStringResourceService.Update(localeByMap);
            }
            else
            {
                LocaleStringResource localeByMap = Mapper.Map<LocaleStringResourceViewModel, LocaleStringResource>(model);
                _localeStringResourceService.Create(localeByMap);
            }

            //var resources = _services.Localization.GetByLanguageId(1);
            //ViewBag.Localization = resources;

            return base.Json(
                new
                {
                    succes = true
                }
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string id)
        {
            LocaleStringResource locale = _services.Localization.GetById(int.Parse(id));
            _services.Localization.Delete(locale);

            return base.Json(
                new { success = true }
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewRow(string languageId)
        {
            LocaleStringResource model = new LocaleStringResource();
            model.LanguageId = int.Parse(languageId);

            string newRow = this.RenderRazorViewToString("_NewRow", model);

            return base.Json(new { data = newRow, success = true }, JsonRequestBehavior.AllowGet);
        }

    }
}