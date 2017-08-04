using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
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
using System.Web;
using System.Web.Mvc;

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
                    String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                            .Select(v => v.ErrorMessage + " " + v.Exception));
                    base.ModelState.AddModelError("", messages);
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

                    MenuLink modelMap = Mapper.Map<MenuLinkViewModel, MenuLink>(model);
                    this._menuLinkService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MenuName, localized.MenuName, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.MenuName.NonAccent(), localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
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
            MenuLinkViewModel modelMap = Mapper.Map<MenuLink, MenuLinkViewModel>(this._menuLinkService.GetById(Id));

            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.LocalesId = modelMap.Id;
                locale.MenuName = modelMap.GetLocalized(x => x.MenuName, Id, languageId, false, false);
                locale.MetaTitle = modelMap.GetLocalized(x => x.MetaTitle, Id, languageId, false, false);
                locale.MetaKeywords = modelMap.GetLocalized(x => x.MetaKeywords, Id, languageId, false, false);
                locale.MetaDescription = modelMap.GetLocalized(x => x.MetaDescription, Id, languageId, false, false);
                locale.SeoUrl = modelMap.GetLocalized(x => x.SeoUrl, Id, languageId, false, false);
            });

            return base.View(modelMap);
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
                    String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                             .Select(v => v.ErrorMessage + " " + v.Exception));
                    base.ModelState.AddModelError("", messages);
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
                    MenuLink modelMap = Mapper.Map<MenuLinkViewModel, MenuLink>(model, byId);
                    this._menuLinkService.Update(byId);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MenuName, localized.MenuName, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.MenuName.NonAccent(), localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
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
            List<MenuNavViewModel> lstMenuNav = new List<MenuNavViewModel>();

            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetAll();
            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNavViewModel> menuNav =
                    from x in menuLinks
                    select new MenuNavViewModel()
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
                lstMenuNav = this.CreateMenuNav(null, menuNav);
            }
            return base.View(lstMenuNav);
        }

        private List<MenuNavViewModel> CreateMenuNav(int? parentId, IEnumerable<MenuNavViewModel> source)
        {
            List<MenuNavViewModel> ieMenuNavViewModel = (from x in source
                                       orderby x.OrderDisplay descending
                                       select x).Where<MenuNavViewModel>((MenuNavViewModel x) =>
                                       {
                                           int? nullable1 = x.ParentId;
                                           int? nullable = parentId;
                                           if (nullable1.GetValueOrDefault() != nullable.GetValueOrDefault())
                                           {
                                               return false;
                                           }
                                           return nullable1.HasValue == nullable.HasValue;
                                       }).Select<MenuNavViewModel, MenuNavViewModel>((MenuNavViewModel x) => new MenuNavViewModel()
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
                                       }).ToList<MenuNavViewModel>();

            return ieMenuNavViewModel;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit"))
            {
                IEnumerable<MenuLink> all = this._menuLinkService.GetAll();
                ((dynamic)base.ViewBag).MenuList = all;
            }
        }        
    }
}