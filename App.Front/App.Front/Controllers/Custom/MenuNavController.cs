using App.Core.Common;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.Front.Controllers;
using App.Front.Models;
using App.Service.LocalizedProperty;
using App.Service.Menu;
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
        private readonly IMenuLinkService _menuLinkService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        public MenuNavController(IMenuLinkService menuLinkService, ILocalizedPropertyService localizedPropertyService)
        {
            this._menuLinkService = menuLinkService;
            this._localizedPropertyService = localizedPropertyService;
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
        [PartialCache("Short")]
        public ActionResult GetMenuLink()
        {
            List<MenuNav> query1 = null;
            List<MenuNav> menuNavs = new List<MenuNav>();
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.Position == 1, true);
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

                //Add Localized
                IEnumerable<LocalizedProperty> ieLocalizedProperty = _localizedPropertyService.GetAll();
                List<LocalizedProperty> lst = ieLocalizedProperty.Cast<LocalizedProperty>().ToList();

                string languageSelected = "1", languageFallback = "2";

                var result = (from MenuNav mn in menuNavs
                              join LocalizedProperty lp in lst
                              on mn.MenuId equals lp.EntityId into o
                              from lp in o.DefaultIfEmpty()
                              select new MenuNav()
                              {
                                  MenuName = lp.LocaleValue
                              });

                query1 = (from MenuNav mn in menuNavs
                              join LocalizedProperty lp1 in lst
                                on new { Id = mn.MenuId, LanguageId = 1 } equals new { Id = lp1.EntityId, lp1.LanguageId } into LanguageSelected
                              join LocalizedProperty lp2 in lst
                                on new { Id = mn.MenuId, LanguageId = 2 } equals new { Id = lp2.EntityId, lp2.LanguageId } into LanguageFallback
                              from selected in LanguageSelected.DefaultIfEmpty()
                              from fallback in LanguageFallback.DefaultIfEmpty()                              
                              select new MenuNav()
                              {
                                  MenuName = selected != null ? selected.LocaleValue : mn.MenuName,
                                  MenuId = mn.MenuId,
                                  ParentId = mn.ParentId,                                 
                                  SeoUrl = mn.SeoUrl,
                                  OrderDisplay = mn.OrderDisplay,
                                  ImageUrl = mn.ImageUrl,
                                  CurrentVirtualId = mn.CurrentVirtualId,
                                  VirtualId = mn.VirtualId,
                                  TemplateType = mn.TemplateType,
                                  IconNav = mn.IconBar,
                                  IconBar = mn.IconBar

                              }).ToList();


                //foreach (LocalizedProperty item in lst)
                //{
                //    menuNavs.Add(new LocalizedPropertyViewModel
                //    {
                //        Id = item.Id,
                //        EntityId = item.EntityId,
                //        LanguageId = item.LanguageId,
                //        LocaleKeyGroup = item.LocaleKeyGroup,
                //        LocaleKey = item.LocaleKey,
                //        LocaleValue = item.LocaleValue
                //    });
                //}
            }
            return base.PartialView(query1);
        }


        /// <summary>
        /// Get danh mục cha bên trái ở trang chủ, 
        /// </summary>
        /// <param name="virtualId"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult GetCategoryHome(string virtualId)
        {
            List<MenuNav> menuNavs = new List<MenuNav>();
            virtualId = (virtualId != null && virtualId.Count(i => i.Equals('/')) > 0) ? virtualId.Split('/')[0] : virtualId;
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) =>
                                                    x.Status == 1
                                                    && x.VirtualId.Contains(virtualId)
                                                    , true);
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