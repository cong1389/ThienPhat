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

        private readonly ILanguageService _langService;

        public NewsController(INewsService newsService, IMenuLinkService menuLinkService, IImagePlugin imagePlugin, ILanguageService langService)
        {
            this._newsService = newsService;
            this._menuLinkService = menuLinkService;
            this._imagePlugin = imagePlugin;
            this._langService = langService;
        }

        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Create()
        {
            return base.View();
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Create(NewsViewModel post, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(post);
                }
                else
                {
                    string str = post.Title.NonAccent();
                    IEnumerable<News> bySeoUrl = this._newsService.GetBySeoUrl(str);
                    post.SeoUrl = post.Title.NonAccent();
                    if (bySeoUrl.Any<News>((News x) => x.Id != post.Id))
                    {
                        NewsViewModel newsViewModel = post;
                        newsViewModel.SeoUrl = string.Concat(newsViewModel.SeoUrl, "-", bySeoUrl.Count<News>());
                    }
                    string str1 = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
                    if (post.Image != null && post.Image.ContentLength > 0)
                    {
                        string str2 = string.Concat(str, ".jpg");
                        string str3 = string.Format("{0}-{1}.jpg", str, Guid.NewGuid());
                        string str4 = string.Format("{0}-{1}.jpg", str, Guid.NewGuid());
                        this._imagePlugin.CropAndResizeImage(post.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str2, new int?(ImageSize.WithBigSize), new int?(ImageSize.HeightBigSize), false);
                        this._imagePlugin.CropAndResizeImage(post.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str3, new int?(ImageSize.WithMediumSize), new int?(ImageSize.HeightMediumSize), false);
                        this._imagePlugin.CropAndResizeImage(post.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str4, new int?(ImageSize.WithSmallSize), new int?(ImageSize.HeightSmallSize), false);
                        post.ImageBigSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str2);
                        post.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str3);
                        post.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str4);
                    }
                    if (post.MenuId > 0)
                    {
                        MenuLink byId = this._menuLinkService.GetById(post.MenuId);
                        post.VirtualCatUrl = byId.VirtualSeoUrl;
                        post.VirtualCategoryId = byId.VirtualId;
                    }
                    News news = Mapper.Map<NewsViewModel, News>(post);
                    this._newsService.Create(news);
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
                return base.View(post);
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
            NewsViewModel newsViewModel = Mapper.Map<News, NewsViewModel>(this._newsService.GetById(Id));
            return base.View(newsViewModel);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Edit(NewsViewModel postView, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(postView);
                }
                else
                {
                    News byId = this._newsService.GetById(postView.Id);
                    string str = postView.Title.NonAccent();
                    IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetBySeoUrl(str);
                    postView.SeoUrl = postView.Title.NonAccent();
                    string str1 = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
                    if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != postView.Id))
                    {
                        NewsViewModel newsViewModel = postView;
                        newsViewModel.SeoUrl = string.Concat(newsViewModel.SeoUrl, "-", bySeoUrl.Count<MenuLink>());
                    }
                    if (postView.Image != null && postView.Image.ContentLength > 0)
                    {
                        string str2 = string.Concat(str, ".jpg");
                        string str3 = string.Format("{0}-{1}.jpg", str, Guid.NewGuid());
                        string str4 = string.Format("{0}-{1}.jpg", str, Guid.NewGuid());
                        this._imagePlugin.CropAndResizeImage(postView.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str2, new int?(ImageSize.WithBigSize), new int?(ImageSize.HeightBigSize), false);
                        this._imagePlugin.CropAndResizeImage(postView.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str3, new int?(ImageSize.WithMediumSize), new int?(ImageSize.HeightMediumSize), false);
                        this._imagePlugin.CropAndResizeImage(postView.Image, string.Format("{0}{1}/", Contains.NewsFolder, str1), str4, new int?(ImageSize.WithSmallSize), new int?(ImageSize.HeightSmallSize), false);
                        postView.ImageBigSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str2);
                        postView.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str3);
                        postView.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, str1, str4);
                    }
                    if (postView.MenuId > 0)
                    {
                        MenuLink menuLink = this._menuLinkService.GetById(postView.MenuId);
                        postView.VirtualCatUrl = menuLink.VirtualSeoUrl;
                        postView.VirtualCategoryId = menuLink.VirtualId;
                        postView.MenuLink = Mapper.Map<MenuLink, MenuLinkViewModel>(menuLink);
                    }
                    News news = Mapper.Map<NewsViewModel, News>(postView, byId);
                    this._newsService.Update(news);
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
                return base.View(postView);
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

                IEnumerable<Language> language = this._langService.GetAll();
                ((dynamic)base.ViewBag).AllLanguage = language;
            }
        }
    }
}