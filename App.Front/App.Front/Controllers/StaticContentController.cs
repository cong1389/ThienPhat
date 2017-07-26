using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using App.Service.Common;
using App.Service.Language;
using App.Service.Menu;
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

        private readonly IMenuLinkService _menuLinkService;

        public StaticContentController(IStaticContentService staticContentService
            , IWorkContext workContext
            , IMenuLinkService menuLinkService
            )
        {
            this._staticContentService = staticContentService;
            this._workContext = workContext;
            this._menuLinkService = menuLinkService;
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

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            dynamic viewBag = base.ViewBag;

            //Get link sản phẩm
            IEnumerable<MenuLink> menuLinksProduct = _menuLinkService.FindBy((MenuLink x) =>
                                                    x.Status == 1
                                                    && x.VirtualId == "ca19fb4a-10a1-4515-bdb2-0c091b4107d5"
                                                    , true);
            if (menuLinksProduct.Any<MenuLink>())
            {               
                viewBag.objMenuLinkProduct = menuLinksProduct.ElementAt(0);
            }

            //Get link sản phẩm
            IEnumerable<MenuLink> menuLinksIntro = _menuLinkService.FindBy((MenuLink x) =>
                                                    x.Status == 1
                                                    && x.VirtualId == "5ff97ccf-29d4-47d2-82d9-9d217119a68d"
                                                    , true);
            if (menuLinksIntro.Any<MenuLink>())
            {
                viewBag.objMenuLinkIntro = menuLinksIntro.ElementAt(0);
            }
        }
    }
}