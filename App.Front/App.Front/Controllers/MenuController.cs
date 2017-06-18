using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Entities.Location;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.Front.Models;
using App.Service.Locations;
using App.Service.Menu;
using App.Service.Static;
using App.Utils;
using App.Utils.MVCHelper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class MenuController : FrontBaseController
	{
        private readonly IMenuLinkService _menuLinkService;

        private readonly IProvinceService _provinceService;

        private readonly IDistrictService _isDistrictService;

        private IStaticContentService _staticContentService;

        public MenuController(IMenuLinkService menuLinkService, IProvinceService provinceService, IDistrictService isDistrictService, IStaticContentService staticContentService)
        {
            this._menuLinkService = menuLinkService;
            this._provinceService = provinceService;
            this._isDistrictService = isDistrictService;
            this._staticContentService = staticContentService;
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetAccesssories()
        {
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.TemplateType == 8 && x.DisplayOnHomePage && x.Status == 1, true);
            return base.PartialView(menuLinks);
        }

        public ActionResult GetContent(string menu, int page)
        {
            MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.SeoUrl.Equals(menu), true);
            if (menuLink != null)
            {
                ((dynamic)base.ViewBag).Title = menuLink.MetaTitle;
                ((dynamic)base.ViewBag).KeyWords = menuLink.MetaKeywords;
                ((dynamic)base.ViewBag).SiteUrl = base.Url.Action("GetContent", "Menu", new { menu = menu, page = page, area = "" });
                ((dynamic)base.ViewBag).Description = menuLink.MetaDescription;
                ((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", menuLink.ImageUrl));
            }
            if (menuLink.TemplateType == 1)
            {
                dynamic viewBag = base.ViewBag;
                IMenuLinkService menuLinkService = this._menuLinkService;
                viewBag.MenuList = menuLinkService.FindBy((MenuLink x) => x.TemplateType == 1, false);
            }
            ((dynamic)base.ViewBag).ParentId = menuLink.ParentId;
            ((dynamic)base.ViewBag).Attrs = base.Request["attribute"];
            ((dynamic)base.ViewBag).Prices = base.Request["price"];
            ((dynamic)base.ViewBag).KeyWords = base.Request["keywords"];
            ((dynamic)base.ViewBag).ProAttrs = base.Request["proattribute"];
            ((dynamic)base.ViewBag).TemplateType = menuLink.TemplateType;
            ((dynamic)base.ViewBag).MenuId = menuLink.Id;
            ((dynamic)base.ViewBag).ImgePath = menuLink.ImageUrl;
            ((dynamic)base.ViewBag).PageNumber = page;
            ((dynamic)base.ViewBag).VirtualId = menuLink.VirtualId;
            return base.View();
        }

        public ActionResult GetFixItemContent(int Id)
        {
            MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.Id == Id, false);
            ((dynamic)base.ViewBag).ImgUrl = menuLink.ImageUrl;
            ((dynamic)base.ViewBag).TitleFix = menuLink.MenuName;
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.ParentId == (int?)Id && x.Status == 1 && x.DisplayOnHomePage, false);
            if (!menuLinks.IsAny<MenuLink>())
            {
                return base.Json(new { success = true, data = "" }, JsonRequestBehavior.AllowGet);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_PartialFixItemContent", menuLinks), success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLeftFixItem(int Id)
        {
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.ParentId == (int?)Id && x.Status == 1 && x.DisplayOnHomePage, false);
            if (!menuLinks.IsAny<MenuLink>())
            {
                return base.Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_PartialLeftFixItemHome", menuLinks), success = true }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetProductTab()
        {
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.TemplateType == 2 && x.DisplayOnHomePage && x.Status == 1, true);
            return base.PartialView(menuLinks);
        }

        [ChildActionOnly]
        public ActionResult GetStaticContent(int MenuId, string virtualId, string title)
        {
            //MenuController.<> c__DisplayClass7_1 variable = null;
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            string[] strArrays = virtualId.Split(new char[] { '/' });
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                string str = strArrays[i];
                MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str) && x.Id != MenuId, false);
                if (menuLink != null)
                {
                    breadCrumbs.Add(new BreadCrumb()
                    {
                        Title = menuLink.MenuName,
                        Current = false,
                        Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                    });
                }
            }
            breadCrumbs.Add(new BreadCrumb()
            {
                Current = true,
                Title = title
            });
            ((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;        
            StaticContent staticContent = this._staticContentService.Get((StaticContent x) => x.MenuId == MenuId, true);
            if (staticContent != null)
            {
                StaticContent viewCount = staticContent;
                viewCount.ViewCount = viewCount.ViewCount + 1;
                this._staticContentService.Update(staticContent);
            }
            return base.PartialView(staticContent);
        }

        public ActionResult GetStaticContentParent(int menuId, string title, string virtualId)
        {
            //MenuController.<> c__DisplayClass13_0 variable = null;
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            string[] strArrays = virtualId.Split(new char[] { '/' });
            StaticContent staticContent = this._staticContentService.Get((StaticContent x) => x.MenuId == menuId && x.Status == 1, false);
            dynamic viewBag = base.ViewBag;
            IMenuLinkService menuLinkService = this._menuLinkService;
            viewBag.ListItems = menuLinkService.FindBy((MenuLink x) => x.ParentId == (int?)menuId && x.Status == 1, false);
            string[] strArrays1 = strArrays;
            for (int i = 0; i < (int)strArrays1.Length; i++)
            {
                string str = strArrays1[i];
                MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str) && !x.MenuName.Equals(title), false);
                if (menuLink != null)
                {
                    breadCrumbs.Add(new BreadCrumb()
                    {
                        Title = menuLink.MenuName,
                        Current = false,
                        Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                    });
                }
            }
            breadCrumbs.Add(new BreadCrumb()
            {
                Current = true,
                Title = title
            });
            ((dynamic)base.ViewBag).TitleNews = title;
            ((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;
            ((dynamic)base.ViewBag).Title = title;
            return base.PartialView(staticContent);
        }

        [ChildActionOnly]
        public ActionResult GetStaticHot(string virtualId)
        {
            string str = virtualId;
            string[] strArrays = str.Split(new char[] { '/' });
            if ((int)strArrays.Length >= 3)
            {
                str = string.Format("{0}/{1}", strArrays[0], strArrays[1]);
            }
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.VirtualId.Contains(str) && x.TemplateType == 6, false);
            return base.PartialView(menuLinks);
        }

        //public ActionResult Search(SeachConditions conditions)
        //{

        //    Current member / type: System.Web.Mvc.ActionResult App.Front.Controllers.MenuController::Search(App.Front.Models.SeachConditions)
        //     File path: D:\FreeLance\2016\Suachuathongminh\Hosting\bin\App.Front.dll

        //     Product version: 2017.2.502.1
        //     Exception in: System.Web.Mvc.ActionResult Search(App.Front.Models.SeachConditions)

        //     Stack overflow while traversing code tree in visit.
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 348
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..(BinaryExpression ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 544
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 141
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 512
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 123
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 838
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 327
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 356
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 276
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 518
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 126
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 390
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 72
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 423
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 84
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 385
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 69
        //        at ..(IfStatement ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 405
        //        at ..(IfStatement ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\MergeUnaryAndBinaryExpression.cs:line 49
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 78
        //        at ..Visit(IEnumerable ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 380
        //        at ..( ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 385
        //        at ..Visit(ICodeNode ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:line 69
        //        at ..(DecompilationContext ,  ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\MergeUnaryAndBinaryExpression.cs:line 42
        //        at ..(MethodBody ,  , ILanguage ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:line 88
        //        at ..(MethodBody , ILanguage ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:line 70
        //        at Telerik.JustDecompiler.Decompiler.Extensions.( , ILanguage , MethodBody , DecompilationContext& ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:line 95
        //        at Telerik.JustDecompiler.Decompiler.Extensions.(MethodBody , ILanguage , DecompilationContext& ,  ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:line 58
        //        at ..(ILanguage , MethodDefinition ,  ) in C:\Builds\556\Behemoth\ReleaseBrand Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 117
             
        //     mailto: JustDecompilePublicFeedback@telerik.com

        //}
    }
}