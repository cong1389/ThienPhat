using App.Admin.Helpers;
using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Meu;
using App.FakeEntity.News;
using App.Framework.Ultis;
using App.ImagePlugin;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Menu;
using App.Service.News;
using App.Utils;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Admin.Controllers
{
    public class NewsController : BaseAdminController
    {
        private readonly IMenuLinkService _menuLinkService;

        private readonly INewsService _newsService;

        private IImagePlugin _imagePlugin;

        private readonly ILanguageService _languageService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        public NewsController(
            INewsService newsService
            , IMenuLinkService menuLinkService
            , IImagePlugin imagePlugin
            , ILanguageService languageService
            , ILocalizedPropertyService localizedPropertyService)
        {
            this._newsService = newsService;
            this._menuLinkService = menuLinkService;
            this._imagePlugin = imagePlugin;
            this._languageService = languageService;
            this._localizedPropertyService = localizedPropertyService;
        }

        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Create()
        {
            var model = new NewsViewModel();

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Create(NewsViewModel model, string ReturnUrl)
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
                    string str = model.Title.NonAccent();
                    IEnumerable<News> bySeoUrl = this._newsService.GetBySeoUrl(str);
                    model.SeoUrl = model.Title.NonAccent();
                    if (bySeoUrl.Any<News>((News x) => x.Id != model.Id))
                    {
                        NewsViewModel newsViewModel = model;
                        newsViewModel.SeoUrl = string.Concat(newsViewModel.SeoUrl, "-", bySeoUrl.Count<News>());
                    }
                    string str1 = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string str2 = string.Concat(str, ".jpg");
                        string str3 = string.Format("{0}-{1}.jpg", str, Guid.NewGuid());
                        string str4 = string.Format("{0}-{1}.jpg", str, Guid.NewGuid());
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str2, new int?(ImageSize.WithBigSize), new int?(ImageSize.HeightBigSize), false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str3, new int?(ImageSize.WithMediumSize), new int?(ImageSize.HeightMediumSize), false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str4, new int?(ImageSize.WithSmallSize), new int?(ImageSize.HeightSmallSize), false);
                        model.ImageBigSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str2);
                        model.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str3);
                        model.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str4);
                    }
                    if (model.MenuId > 0)
                    {
                        MenuLink byId = this._menuLinkService.GetById(model.MenuId);
                        model.VirtualCatUrl = byId.VirtualSeoUrl;
                        model.VirtualCategoryId = byId.VirtualId;
                    }
                    News modelMap = Mapper.Map<NewsViewModel, News>(model);
                    this._newsService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ShortDesc, localized.ShortDesc, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.Title.NonAccent(), localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.News)));
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
                ExtentionUtils.Log(string.Concat("Post.Create: ", exception.Message));
                base.ModelState.AddModelError("", exception.Message);
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "DeleteNews")]
        public ActionResult Delete(int[] ids)
        {
            try
            {
                if (ids.Length != 0)
                {
                    IEnumerable<News> news =
                        from id in ids
                        select this._newsService.GetById(id);
                    this._newsService.BatchDelete(news);

                    //Delete localize
                    for (int i = 0; i < ids.Length; i++)
                    {
                        IEnumerable<LocalizedProperty> ieLocalizedProperty
                           = _localizedPropertyService.GetLocalizedPropertyByEntityId(ids[i]);
                        this._localizedPropertyService.BatchDelete(ieLocalizedProperty);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("Post.Delete: ", exception.Message));
            }
            return base.RedirectToAction("Index");
        }

        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Edit(int Id)
        {
            NewsViewModel modelMap = Mapper.Map<News, NewsViewModel>(this._newsService.GetById(Id));

            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.LocalesId = modelMap.Id;
                locale.Title = modelMap.GetLocalized(x => x.Title, Id, languageId, false, false);                
                locale.ShortDesc = modelMap.GetLocalized(x => x.ShortDesc, Id, languageId, false, false);
                locale.Description = modelMap.GetLocalized(x => x.Description, Id, languageId, false, false);                
                locale.MetaTitle = modelMap.GetLocalized(x => x.MetaTitle, Id, languageId, false, false);
                locale.MetaKeywords = modelMap.GetLocalized(x => x.MetaKeywords, Id, languageId, false, false);
                locale.MetaDescription = modelMap.GetLocalized(x => x.MetaDescription, Id, languageId, false, false);
                locale.SeoUrl = modelMap.GetLocalized(x => x.SeoUrl, Id, languageId, false, false);
            });

            return base.View(modelMap);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Edit(NewsViewModel model, string ReturnUrl)
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
                    News byId = this._newsService.GetById(model.Id);
                    string str = model.Title.NonAccent();
                    IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetBySeoUrl(str);
                    model.SeoUrl = model.Title.NonAccent();
                    string str1 = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
                    if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != model.Id))
                    {
                        NewsViewModel newsViewModel = model;
                        newsViewModel.SeoUrl = string.Concat(newsViewModel.SeoUrl, "-", bySeoUrl.Count<MenuLink>());
                    }
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string str2 = string.Concat(str, ".jpg");
                        string str3 = string.Format("{0}-{1}.jpg", str, Guid.NewGuid());
                        string str4 = string.Format("{0}-{1}.jpg", str, Guid.NewGuid());
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str2, new int?(ImageSize.WithBigSize), new int?(ImageSize.HeightBigSize), false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str3, new int?(ImageSize.WithMediumSize), new int?(ImageSize.HeightMediumSize), false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str4, new int?(ImageSize.WithSmallSize), new int?(ImageSize.HeightSmallSize), false);
                        model.ImageBigSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str2);
                        model.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str3);
                        model.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str4);
                    }
                    if (model.MenuId > 0)
                    {
                        MenuLink menuLink = this._menuLinkService.GetById(model.MenuId);
                        model.VirtualCatUrl = menuLink.VirtualSeoUrl;
                        model.VirtualCategoryId = menuLink.VirtualId;
                        model.MenuLink = Mapper.Map<MenuLink, MenuLinkViewModel>(menuLink);
                    }
                    News modelMap = Mapper.Map<NewsViewModel, News>(model, byId);
                    this._newsService.Update(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ShortDesc, localized.ShortDesc, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.Title.NonAccent(), localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.News)));
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
                ExtentionUtils.Log(string.Concat("Post.Edit: ", exception.Message));
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "ViewNews")]
        public ActionResult Index(int page = 1, string keywords = "")
        {
            ((dynamic)base.ViewBag).Keywords = keywords;
            SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
            {
                Keywords = keywords,
                Sorts = new SortBuilder()
                {
                    ColumnName = "CreatedDate",
                    ColumnOrder = SortBuilder.SortOrder.Descending
                }
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = base._pageSize,
                TotalRecord = 0
            };
            IEnumerable<News> news = this._newsService.PagedList(sortingPagingBuilder, paging);
            if (news != null && news.Any<News>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(news);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit") || filterContext.RouteData.Values["action"].ToString().ToLower().Equals("index"))
            {
                IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType == 1 || x.TemplateType == 6 || x.TemplateType == 7, true);
                ((dynamic)base.ViewBag).MenuList = menuLinks;
            }
        }
    }
}