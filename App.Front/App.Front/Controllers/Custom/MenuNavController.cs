using App.Core.Common;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Meu;
using App.Front.Controllers;
using App.Front.Models;
using App.Service.Common;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Menu;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers.Custom
{
    public class MenuNavController : FrontBaseController
    {
        private readonly IWorkContext _workContext;

        private readonly IMenuLinkService _menuLinkService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        private readonly ILanguageService _languageService;

        public MenuNavController(IMenuLinkService menuLinkService, ILocalizedPropertyService localizedPropertyService
            , IWorkContext workContext, ILanguageService languageService)
        {
            this._menuLinkService = menuLinkService;
            this._localizedPropertyService = localizedPropertyService;
            this._workContext = workContext;
            this._languageService = languageService;
        }

        private List<MenuNav> CreateMenuNav(int? parentId, IEnumerable<MenuNav> source)
        {
            List<MenuNav> ieMenuNav = (from x in source
                                       orderby x.OrderDisplay descending
                                       select x).Where<MenuNav>((MenuNav x) =>
                                       {
                                           int? nullable1 = x.ParentId;
                                           int? nullable = parentId;
                                           if (nullable1.GetValueOrDefault() != nullable.GetValueOrDefault())
                                           {
                                               return false;
                                           }
                                           return nullable1.HasValue == nullable.HasValue;
                                       }).Select<MenuNav, MenuNav>((MenuNav x) => new MenuNav()
                                       {
                                           MenuId = x.MenuId,
                                           ParentId = x.ParentId,
                                           MenuName = x.MenuName,
                                           SeoUrl = x.SeoUrl,
                                           OrderDisplay = x.OrderDisplay,
                                           ImageUrl = x.ImageUrl,
                                           CurrentVirtualId = x.CurrentVirtualId,
                                           VirtualId = x.VirtualId,
                                           TemplateType = x.TemplateType,
                                           OtherLink = x.OtherLink,
                                           IconNav = x.IconNav,
                                           IconBar = x.IconBar,
                                           ChildNavMenu = this.CreateMenuNav(new int?(x.MenuId), source)
                                       }).ToList<MenuNav>();

            return ieMenuNav;
        }

        public ActionResult GetFixedHomePage()
        {
            List<MenuNav> menuNavs = new List<MenuNav>();
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType == 6 && x.DisplayOnHomePage, true);
            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNav> menuNav =
                    from x in menuLinks
                    select new MenuNav()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetFooterLink()
        {
            List<MenuNav> menuNavs = new List<MenuNav>();
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.Position == 2, true);
            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNav> menuNav =
                    from x in menuLinks
                    select new MenuNav()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }

        [ChildActionOnly]
        public ActionResult GetMenuLink()
        {
            int languageId = _workContext.WorkingLanguage.Id;
                        
            List<MenuNav> menuNavs = new List<MenuNav>();
            IEnumerable<MenuLink> menuLinks = _menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.Position == 1, true);
            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNav> menuNav =
                    from x in menuLinks
                    select new MenuNav()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.GetLocalizedByLocaleKey(x.MenuName, x.Id, languageId, "MenuLink", "MenuName"),
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };

                menuNavs = this.CreateMenuNav(null, menuNav);
            }

            return base.PartialView(menuNavs);
        }


        /// <summary>
        /// Get danh mục cha bên trái ở trang chủ, 
        /// </summary>
        /// <param name="virtualId"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult GetCategoryHome(string virtualId)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            List<MenuNav> menuNavs = new List<MenuNav>();
            virtualId = (virtualId != null && virtualId.Count(i => i.Equals('/')) > 0) ? virtualId.Split('/')[0] : virtualId;
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) =>
                                                    x.Status == 1
                                                    && x.VirtualId.Contains(virtualId)
                                                    , true);

            if (menuLinks == null)
                return HttpNotFound();

            IEnumerable<MenuNav> menuNavLocalized = null;
            if (menuLinks.Any<MenuLink>())
            {
                 menuNavLocalized =
                    from x in menuLinks
                    select new MenuNav()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.GetLocalizedByLocaleKey(x.MenuName, x.Id, languageId, "MenuLink", "MenuName"),
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(null, menuNavLocalized);                
            }
            return base.PartialView(menuNavs);
        }

        /// <summary>
        /// Menu category slidebar bên trái
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetMenuCategory()
        {
            List<MenuNav> menuNavs = new List<MenuNav>();
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && (x.Position == 5 || x.Position == 1), true);
            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNav> menuNav =
                    from x in menuLinks
                    select new MenuNav()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetMenuLinkSideBar(List<int> Ids = null)
        {
            List<MenuNav> menuNavs = new List<MenuNav>();
            MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.Status == 1 && x.TemplateType == 2 && !x.ParentId.HasValue, true);
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType == 2 && x.VirtualId.Contains(menuLink.VirtualId), true);
            ((dynamic)base.ViewBag).ProIds = Ids;
            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNav> menuNav =
                    from x in menuLinks
                    select new MenuNav()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(new int?(menuLink.Id), menuNav);
            }
            return base.PartialView(menuNavs);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetMenuOnHomePage()
        {
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.DisplayOnHomePage, false);
            return base.PartialView(menuLinks);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetMenuTopHead()
        {
            List<MenuNav> menuNavs = new List<MenuNav>();
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.Position == 7, true);
            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNav> menuNav =
                    from x in menuLinks
                    select new MenuNav()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType
                    };
                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }

        public ActionResult StickyBar()
        {
            List<MenuNav> menuNavs = new List<MenuNav>();
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType == 6 && x.DisplayOnSearch, true);
            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNav> menuNav =
                    from x in menuLinks
                    select new MenuNav()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }
    }


}