using App.Domain.Entities.Data;
using App.Service.Common;
using App.Service.Language;
using App.Service.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public class StaticContentController : FrontBaseController
    {
        private IStaticContentService _staticContentService;

        private readonly IWorkContext _workContext;

        public StaticContentController(IStaticContentService staticContentService, IWorkContext workContext)
        {
            this._staticContentService = staticContentService;
            this._workContext = workContext;
        }

        // GET: StaticContent
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult GetSlogan(int MenuId)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            StaticContent staticContent = this._staticContentService.Get((StaticContent x) => x.Id == MenuId, true);

            if (staticContent == null)
                return HttpNotFound();

            StaticContent staticContentLocalized = new StaticContent
            {
                Id = staticContent.Id,
                MenuId = staticContent.MenuId,
                VirtualCategoryId = staticContent.VirtualCategoryId,
                Language = staticContent.Language,
                Status = staticContent.Status,
                SeoUrl = staticContent.SeoUrl,
                ImagePath = staticContent.ImagePath,

                Title = staticContent.GetLocalizedByLocaleKey(staticContent.Title, staticContent.Id, languageId, "StaticContent", "Title"),
                ShortDesc = staticContent.GetLocalizedByLocaleKey(staticContent.ShortDesc, staticContent.Id, languageId, "StaticContent", "ShortDesc"),
                Description = staticContent.GetLocalizedByLocaleKey(staticContent.Description, staticContent.Id, languageId, "StaticContent", "Description"),
                MetaTitle = staticContent.GetLocalizedByLocaleKey(staticContent.MetaTitle, staticContent.Id, languageId, "StaticContent", "MetaTitle"),
                MetaKeywords = staticContent.GetLocalizedByLocaleKey(staticContent.MetaKeywords, staticContent.Id, languageId, "StaticContent", "MetaKeywords"),
                MetaDescription = staticContent.GetLocalizedByLocaleKey(staticContent.MetaDescription, staticContent.Id, languageId, "StaticContent", "MetaDescription")
            };

            return base.PartialView(staticContentLocalized);
        }

        [ChildActionOnly]
        public ActionResult GetHomeIntro(int MenuId)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            StaticContent staticContent = this._staticContentService.Get((StaticContent x) => x.Id == MenuId, true);

            if (staticContent == null)
                return HttpNotFound();

            StaticContent staticContentLocalized = new StaticContent
            {
                Id = staticContent.Id,
                MenuId = staticContent.MenuId,
                VirtualCategoryId = staticContent.VirtualCategoryId,
                Language = staticContent.Language,
                Status = staticContent.Status,
                SeoUrl = staticContent.SeoUrl,
                ImagePath = staticContent.ImagePath,

                Title = staticContent.GetLocalizedByLocaleKey(staticContent.Title, staticContent.Id, languageId, "StaticContent", "Title"),
                ShortDesc = staticContent.GetLocalizedByLocaleKey(staticContent.ShortDesc, staticContent.Id, languageId, "StaticContent", "ShortDesc"),
                Description = staticContent.GetLocalizedByLocaleKey(staticContent.Description, staticContent.Id, languageId, "StaticContent", "Description"),
                MetaTitle = staticContent.GetLocalizedByLocaleKey(staticContent.MetaTitle, staticContent.Id, languageId, "StaticContent", "MetaTitle"),
                MetaKeywords = staticContent.GetLocalizedByLocaleKey(staticContent.MetaKeywords, staticContent.Id, languageId, "StaticContent", "MetaKeywords"),
                MetaDescription = staticContent.GetLocalizedByLocaleKey(staticContent.MetaDescription, staticContent.Id, languageId, "StaticContent", "MetaDescription")
            };

            return base.PartialView(staticContentLocalized);
        }

        [ChildActionOnly]
        public ActionResult GetHomeProduct(int MenuId)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            StaticContent staticContent = this._staticContentService.Get((StaticContent x) => x.Id == MenuId, true);

            if (staticContent == null)
                return HttpNotFound();

            StaticContent staticContentLocalized = new StaticContent
            {
                Id = staticContent.Id,
                MenuId = staticContent.MenuId,
                VirtualCategoryId = staticContent.VirtualCategoryId,
                Language = staticContent.Language,
                Status = staticContent.Status,
                SeoUrl = staticContent.SeoUrl,
                ImagePath = staticContent.ImagePath,

                Title = staticContent.GetLocalizedByLocaleKey(staticContent.Title, staticContent.Id, languageId, "StaticContent", "Title"),
                ShortDesc = staticContent.GetLocalizedByLocaleKey(staticContent.ShortDesc, staticContent.Id, languageId, "StaticContent", "ShortDesc"),
                Description = staticContent.GetLocalizedByLocaleKey(staticContent.Description, staticContent.Id, languageId, "StaticContent", "Description"),
                MetaTitle = staticContent.GetLocalizedByLocaleKey(staticContent.MetaTitle, staticContent.Id, languageId, "StaticContent", "MetaTitle"),
                MetaKeywords = staticContent.GetLocalizedByLocaleKey(staticContent.MetaKeywords, staticContent.Id, languageId, "StaticContent", "MetaKeywords"),
                MetaDescription = staticContent.GetLocalizedByLocaleKey(staticContent.MetaDescription, staticContent.Id, languageId, "StaticContent", "MetaDescription")
            };

            return base.PartialView(staticContentLocalized);
        }
    }
}