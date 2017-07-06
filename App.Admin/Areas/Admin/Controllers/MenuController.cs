using App.Admin.Helpers;
using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Language;
using App.FakeEntity.Meu;
using App.Framework.Ultis;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Menu;
using App.Utils;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Admin.Controllers
{
    public class MenuController : BaseAdminController
    {
        private readonly IMenuLinkService _menuLinkService;

        private readonly ILocalizedPropertyService _localizedPropertyService;
        private readonly ILanguageService _languageService;

        public MenuController(IMenuLinkService menuLinkService, ILocalizedPropertyService localizedPropertyService, ILanguageService languageService)
        {
            this._menuLinkService = menuLinkService;
            this._localizedPropertyService = localizedPropertyService;
            this._languageService = languageService;
        }

        [RequiredPermisson(Roles = "CreateEditMenu")]
        public ActionResult Create()
        {
            var model = new MenuLinkViewModel();

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditMenu")]
        public ActionResult Create(MenuLinkViewModel model, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(model);
                }
                else
                {
                    string str = model.MenuName.NonAccent();
                    IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetBySeoUrl(str);
                    model.SeoUrl = model.MenuName.NonAccent();
                    if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != model.Id))
                    {
                        MenuLinkViewModel menuLinkViewModel = model;
                        string seoUrl = menuLinkViewModel.SeoUrl;
                        int num = bySeoUrl.Count<MenuLink>();
                        menuLinkViewModel.SeoUrl = string.Concat(seoUrl, "-", num.ToString());
                    }
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(model.Image.FileName);
                        string extension = Path.GetExtension(model.Image.FileName);
                        fileName = string.Concat(model.MenuName.NonAccent(), extension);
                        string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
                        model.Image.SaveAs(str1);
                        model.ImageUrl = string.Concat(Contains.ImageFolder, fileName);
                    }
                    if (model.ImageIcon1 != null && model.ImageIcon1.ContentLength > 0)
                    {
                        string fileName1 = Path.GetFileName(model.ImageIcon1.FileName);
                        string extension1 = Path.GetExtension(model.ImageIcon1.FileName);
                        fileName1 = string.Concat(string.Concat(model.MenuName, "-icon").NonAccent(), extension1);
                        string str2 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName1);
                        model.ImageIcon1.SaveAs(str2);
                        model.Icon1 = string.Concat(Contains.ImageFolder, fileName1);
                    }
                    if (model.ImageIcon2 != null && model.ImageIcon2.ContentLength > 0)
                    {
                        string fileName2 = Path.GetFileName(model.ImageIcon2.FileName);
                        string extension2 = Path.GetExtension(model.ImageIcon2.FileName);
                        fileName2 = string.Concat(string.Concat(model.MenuName, "-iconbar").NonAccent(), extension2);
                        string str3 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName2);
                        model.ImageIcon2.SaveAs(str3);
                        model.Icon2 = string.Concat(Contains.ImageFolder, fileName2);
                    }
                    if (model.ParentId.HasValue)
                    {
                        string str4 = Guid.NewGuid().ToString();
                        model.CurrentVirtualId = str4;
                        MenuLink byId = this._menuLinkService.GetById(model.ParentId.Value);
                        model.VirtualId = string.Format("{0}/{1}", byId.VirtualId, str4);
                        model.VirtualSeoUrl = string.Format("{0}/{1}", byId.SeoUrl, model.SeoUrl);
                    }
                    else
                    {
                        string str5 = Guid.NewGuid().ToString();
                        model.VirtualId = str5;
                        model.CurrentVirtualId = str5;
                    }                    

                    MenuLink menuLink1 = Mapper.Map<MenuLinkViewModel, MenuLink>(model);
                    this._menuLinkService.Create(menuLink1);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(menuLink1, x => x.MenuName, localized.MenuName, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(menuLink1, x => x.SeoUrl, localized.SeoUrl, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(menuLink1, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(menuLink1, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(menuLink1, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.MenuLink)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
                    }
                    else
                    {
                        action = this.Redirect(ReturnUrl);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("MailSetting.Create: ", exception.Message));
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "DeleteMenu")]
        public ActionResult Delete(string[] ids)
        {
            try
            {
                if (ids.Length != 0)
                {
                    IEnumerable<MenuLink> menuLinks =
                        from id in ids
                        select this._menuLinkService.GetById(int.Parse(id));
                    this._menuLinkService.BatchDelete(menuLinks);

                    //Delete localize
                    for (int i = 0; i < ids.Length; i++)
                    {
                        IEnumerable<LocalizedProperty> ieLocalizedProperty
                            = _localizedPropertyService.GetLocalizedPropertyByEntityId(int.Parse(ids[i]));
                        this._localizedPropertyService.BatchDelete(ieLocalizedProperty);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("MenuLink.Delete: ", exception.Message));
            }
            return base.RedirectToAction("Index");
        }

        [RequiredPermisson(Roles = "CreateEditMenu")]
        public ActionResult Edit(int Id)
        {
            MenuLinkViewModel menuLinkViewModel = Mapper.Map<MenuLink, MenuLinkViewModel>(this._menuLinkService.GetById(Id));

            //Add Locales to model
            AddLocales(_languageService, menuLinkViewModel.Locales, (locale, languageId) =>
            {
                locale.Id = menuLinkViewModel.Id;
                locale.LocalesId = menuLinkViewModel.Id;
                locale.MenuName = menuLinkViewModel.GetLocalized(x => x.MenuName, Id, languageId, false, false);
                locale.MetaTitle = menuLinkViewModel.GetLocalized(x => x.MetaTitle, Id, languageId, false, false);
                locale.MetaKeywords = menuLinkViewModel.GetLocalized(x => x.MetaKeywords, Id, languageId, false, false);
                locale.MetaDescription = menuLinkViewModel.GetLocalized(x => x.MetaDescription, Id, languageId, false, false);
                locale.SeoUrl = menuLinkViewModel.GetLocalized(x => x.SeoUrl, Id, languageId, false, false);
            });

            return base.View(menuLinkViewModel);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditMenu")]
        public ActionResult Edit(MenuLinkViewModel model, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(model);
                }
                else
                {
                    MenuLink byId = this._menuLinkService.GetById(model.Id);
                    string str = model.MenuName.NonAccent();
                    IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetBySeoUrl(str);
                    model.SeoUrl = model.MenuName.NonAccent();
                    if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != model.Id))
                    {
                        MenuLinkViewModel menuLinkViewModel = model;
                        string seoUrl = menuLinkViewModel.SeoUrl;
                        int num = bySeoUrl.Count<MenuLink>();
                        menuLinkViewModel.SeoUrl = string.Concat(seoUrl, "-", num.ToString());
                    }
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(model.Image.FileName);
                        string extension = Path.GetExtension(model.Image.FileName);
                        fileName = string.Concat(model.MenuName.NonAccent(), extension);
                        string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
                        model.Image.SaveAs(str1);
                        model.ImageUrl = string.Concat(Contains.ImageFolder, fileName);
                    }
                    if (model.ImageIcon1 != null && model.ImageIcon1.ContentLength > 0)
                    {
                        string fileName1 = Path.GetFileName(model.ImageIcon1.FileName);
                        string extension1 = Path.GetExtension(model.ImageIcon1.FileName);
                        fileName1 = string.Concat(string.Concat(model.MenuName, "-icon").NonAccent(), extension1);
                        string str2 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName1);
                        model.ImageIcon1.SaveAs(str2);
                        model.Icon1 = string.Concat(Contains.ImageFolder, fileName1);
                    }
                    if (model.ImageIcon2 != null && model.ImageIcon2.ContentLength > 0)
                    {
                        string fileName2 = Path.GetFileName(model.ImageIcon2.FileName);
                        string extension2 = Path.GetExtension(model.ImageIcon2.FileName);
                        fileName2 = string.Concat(string.Concat(model.MenuName, "-iconbar").NonAccent(), extension2);
                        string str3 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName2);
                        model.ImageIcon2.SaveAs(str3);
                        model.Icon2 = string.Concat(Contains.ImageFolder, fileName2);
                    }
                    int? parentId = model.ParentId;
                    if (!parentId.HasValue)
                    {
                        parentId = null;
                        model.ParentId = parentId;
                        model.VirtualId = byId.CurrentVirtualId;
                        model.VirtualSeoUrl = null;
                    }
                    else
                    {
                        IMenuLinkService menuLinkService = this._menuLinkService;
                        parentId = model.ParentId;
                        MenuLink byId1 = menuLinkService.GetById(parentId.Value);
                        model.VirtualId = string.Format("{0}/{1}", byId1.VirtualId, byId.CurrentVirtualId);
                        model.VirtualSeoUrl = string.Format("{0}/{1}", byId1.SeoUrl, model.SeoUrl);
                    }
                    MenuLink menuLink1 = Mapper.Map<MenuLinkViewModel, MenuLink>(model, byId);
                    this._menuLinkService.Update(byId);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(menuLink1, x => x.MenuName, localized.MenuName, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(menuLink1, x => x.SeoUrl, localized.SeoUrl, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(menuLink1, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(menuLink1, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(menuLink1, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.MenuLink)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
                    }
                    else
                    {
                        action = this.Redirect(ReturnUrl);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                base.ModelState.AddModelError("", exception.Message);
                ExtentionUtils.Log(string.Concat("MailSetting.Create: ", exception.Message));
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "ViewMenu")]
        public ActionResult Index(int page = 1, string keywords = "")
        {
            ((dynamic)base.ViewBag).Keywords = keywords;
            SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
            {
                Keywords = keywords,
                Sorts = new SortBuilder()
                {
                    ColumnName = "MenuName",
                    ColumnOrder = SortBuilder.SortOrder.Descending
                }
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = base._pageSize,
                TotalRecord = 0
            };
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.PagedList(sortingPagingBuilder, paging);
            if (menuLinks != null && menuLinks.Any<MenuLink>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(menuLinks);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit"))
            {
                IEnumerable<MenuLink> all = this._menuLinkService.GetAll();
                ((dynamic)base.ViewBag).MenuList = all;
            }
        }

        //[NonAction]
        //protected void UpdateLocales(MenuLink category, MenuLinkViewModel model)
        //{
        //    //    foreach (var localized in model.Locales)
        //    //    {
        //    _localizedPropertyService.Update(category);

        //    //        _localizedEntityService.SaveLocalizedValue(category, x => x.FullName, localized.FullName, localized.LanguageId);

        //    //        _localizedEntityService.SaveLocalizedValue(category, x => x.Description, localized.Description, localized.LanguageId);

        //    //        _localizedEntityService.SaveLocalizedValue(category, x => x.BottomDescription, localized.BottomDescription, localized.LanguageId);

        //    //        _localizedEntityService.SaveLocalizedValue(category, x => x.BadgeText, localized.BadgeText, localized.LanguageId);

        //    //        _localizedEntityService.SaveLocalizedValue(category, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);

        //    //        _localizedEntityService.SaveLocalizedValue(category, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);

        //    //        _localizedEntityService.SaveLocalizedValue(category, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);

        //    //        //search engine name
        //    //        var seName = category.ValidateSeName(localized.SeName, localized.Name, false, localized.LanguageId);
        //    //        _urlRecordService.SaveSlug(category, seName, localized.LanguageId);
        //    //    }
        //}
    }
}