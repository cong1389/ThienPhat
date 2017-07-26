using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.Framework.Ultis;
using App.Front.Models;
using App.Service.Common;
using App.Service.Language;
using App.Service.Menu;
using App.Service.News;
using App.Service.Static;
using App.Utils;
using App.Utils.MVCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class NewsController : FrontBaseController
	{
		private readonly INewsService _newsService;

		private readonly IMenuLinkService _menuLinkService;

		private IStaticContentService _staticContentService;

        private readonly IWorkContext _workContext;

        public NewsController(
            INewsService newsService
            , IMenuLinkService menuLinkService
            , IStaticContentService staticContentService
            , IWorkContext workContext)
		{
			this._newsService = newsService;
			this._menuLinkService = menuLinkService;
			this._staticContentService = staticContentService;
            this._workContext = workContext;
        }

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult BreadCrumNews(string virtualId)
		{
			((dynamic)base.ViewBag).VirtualId = virtualId;
			List<MenuLink> lstMenuLink = new List<MenuLink>();
			IEnumerable<MenuLink> menuLinks1 = this._menuLinkService.FindBy((MenuLink x) => x.TemplateType == 1 && x.Status == 1, true);
			if (menuLinks1.IsAny<MenuLink>())
			{
                lstMenuLink.AddRange(menuLinks1);
                ((dynamic)base.ViewBag).TitleNews = menuLinks1.ElementAt(0).MenuName;
            }
			return base.PartialView(lstMenuLink);
		}

		public ActionResult GetCareerByCategory(string virtualCategoryId, int page, string title)
		{
			SortBuilder sortBuilder = new SortBuilder()
			{
				ColumnName = "CreatedDate",
				ColumnOrder = SortBuilder.SortOrder.Descending
			};
			Paging paging = new Paging()
			{
				PageNumber = page,
				PageSize = base._pageSize,
				TotalRecord = 0
			};
			IEnumerable<News> news = this._newsService.FindAndSort((News x) => !x.Video && x.Status == 1 && x.VirtualCategoryId.Contains(virtualCategoryId), sortBuilder, paging);
			if (news.IsAny<News>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("GetContent", "Menu", new { page = i }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
				((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;
			}
			((dynamic)base.ViewBag).Title = title;
			((dynamic)base.ViewBag).virtualCategoryId = virtualCategoryId;
			return base.PartialView(news);
		}

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetHomeNews(int? Id)
		{
            int languageId = _workContext.WorkingLanguage.Id;

            List<News> news = new List<News>();
			IEnumerable<News> top = this._newsService.GetTop<DateTime>(4, (News x) => x.HomeDisplay == true && x.Status == 1, (News x) => x.CreatedDate);

            if (top == null)
                return HttpNotFound();

            if (top.IsAny<News>())
			{
                IEnumerable<News> ieNews = from x in top
                                           select new News()
                                           {
                                               Id = x.Id,
                                               MenuId = x.MenuId,
                                               VirtualCategoryId = x.VirtualCategoryId,
                                               Language = x.Language,
                                               Status = x.Status,
                                               SeoUrl = x.SeoUrl,
                                               ImageBigSize= x.ImageBigSize,
                                               ImageMediumSize = x.ImageMediumSize,
                                               ImageSmallSize = x.ImageSmallSize,
                                               CreatedDate = x.CreatedDate,

                                               Title = x.GetLocalizedByLocaleKey(x.Title, x.Id, languageId, "News", "Title"),
                                               ShortDesc = x.GetLocalizedByLocaleKey(x.ShortDesc, x.Id, languageId, "News", "ShortDesc"),
                                               Description = x.GetLocalizedByLocaleKey(x.Description, x.Id, languageId, "News", "Description"),
                                               MetaTitle = x.GetLocalizedByLocaleKey(x.MetaTitle, x.Id, languageId, "News", "MetaTitle"),
                                               MetaKeywords = x.GetLocalizedByLocaleKey(x.MetaKeywords, x.Id, languageId, "News", "MetaKeywords"),
                                               MetaDescription = x.GetLocalizedByLocaleKey(x.MetaDescription, x.Id, languageId, "News", "MetaDescription")                                               
                                           };

                 news.AddRange(ieNews);
			}
            return base.PartialView(news);
		}

		public ActionResult GetNewsByCategory(string virtualCategoryId, int? menuId,string title, int page)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            ((dynamic)base.ViewBag).MenuId = menuId;
            ((dynamic)base.ViewBag).VirtualId = virtualCategoryId;
            dynamic viewBag = base.ViewBag;

			IStaticContentService staticContentService = this._staticContentService;
			Expression<Func<StaticContent, bool>> status = (StaticContent x) => x.Status == 1;
			viewBag.fixItems = staticContentService.GetTop<int>(3, status, (StaticContent x) => x.ViewCount);

			SortBuilder sortBuilder = new SortBuilder()
			{
				ColumnName = "CreatedDate",
				ColumnOrder = SortBuilder.SortOrder.Descending
			};
			Paging paging = new Paging()
			{
				PageNumber = page,
				PageSize = base._pageSize,
				TotalRecord = 0
			};            

            IEnumerable<News> news = this._newsService.FindAndSort((News x) => !x.Video && x.Status == 1 && x.VirtualCategoryId.Contains(virtualCategoryId), sortBuilder, paging);

            if (news == null)
                return HttpNotFound();           

            IEnumerable<News> newsLocalized = null;
            if (news.IsAny<News>())
			{
               newsLocalized = from x in news
                                           select new News()
                                           {
                                               Id = x.Id,
                                               MenuId = x.MenuId,
                                               VirtualCategoryId = x.VirtualCategoryId,
                                               Language = x.Language,
                                               Status = x.Status,
                                               SeoUrl = x.SeoUrl,
                                               ImageBigSize = x.ImageBigSize,
                                               ImageMediumSize = x.ImageMediumSize,
                                               ImageSmallSize = x.ImageSmallSize,
                                               CreatedDate = x.CreatedDate,

                                               Title = x.GetLocalizedByLocaleKey(x.Title, x.Id, languageId, "News", "Title"),
                                               ShortDesc = x.GetLocalizedByLocaleKey(x.ShortDesc, x.Id, languageId, "News", "ShortDesc"),
                                               Description = x.GetLocalizedByLocaleKey(x.Description, x.Id, languageId, "News", "Description"),
                                               MetaTitle = x.GetLocalizedByLocaleKey(x.MetaTitle, x.Id, languageId, "StaticContent", "MetaTitle"),
                                               MetaKeywords = x.GetLocalizedByLocaleKey(x.MetaKeywords, x.Id, languageId, "News", "MetaKeywords"),
                                               MetaDescription = x.GetLocalizedByLocaleKey(x.MetaDescription, x.Id, languageId, "News", "MetaDescription")
                                           };

                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("GetContent", "Menu", new { page = i }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
				((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;

                MenuLink menuLink = null;
                List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
                string[] strArrays2 = virtualCategoryId.Split(new char[] { '/' });
                for (int i1 = 0; i1 < (int)strArrays2.Length; i1++)
                {
                    string str = strArrays2[i1];
                    menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str) && x.Id != menuId, false);

                    if (menuLink != null)
                    {
                        breadCrumbs.Add(new BreadCrumb()
                        {
                            Title = menuLink.GetLocalizedByLocaleKey(menuLink.MenuName, menuLink.Id, languageId, "MenuLink", "MenuName"),
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
            }

            ((dynamic)base.ViewBag).Title = title;

            return base.PartialView(newsLocalized);
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult GetRelativeNews(string virtualId, int newsId)
		{
			List<News> news = new List<News>();
			IEnumerable<News> top = this._newsService.GetTop<int>(4, (News x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId) && x.Id != newsId && !x.Video, (News x) => x.ViewCount);
			if (top.IsAny<News>())
			{
				news.AddRange(top);
			}
			return base.PartialView(news);
		}

		[ChildActionOnly]
		public ActionResult GetVideoSlide(string virtualCategoryId)
		{
			List<News> news = new List<News>();
			IEnumerable<News> news1 = this._newsService.FindBy((News x) => x.Video && x.Status == 1, true);
			if (news1.IsAny<News>())
			{
				news.AddRange(news1);
			}
			return base.PartialView(news);
		}

		[OutputCache(CacheProfile="Medium")]
		public ActionResult NewsDetail(string seoUrl)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            dynamic viewBag = base.ViewBag;

			IStaticContentService staticContentService = this._staticContentService;
			Expression<Func<StaticContent, bool>> status = (StaticContent x) => x.Status == 1;
			viewBag.fixItems = staticContentService.GetTop<int>(3, status, (StaticContent x) => x.ViewCount);

			List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
			News news = this._newsService.Get((News x) => x.SeoUrl.Equals(seoUrl), true);
            if (news == null)
                return HttpNotFound();

            News newsLocalized = new News();
            if (news != null)
			{
                newsLocalized = new News
                {
                    Id = news.Id,
                    MenuId = news.MenuId,
                    VirtualCategoryId = news.VirtualCategoryId,
                    Language = news.Language,
                    Status = news.Status,
                    SeoUrl = news.SeoUrl,
                    ImageBigSize = news.ImageBigSize,
                    ImageMediumSize = news.ImageMediumSize,
                    ImageSmallSize = news.ImageSmallSize,
                    MenuLink = news.MenuLink,
                    CreatedDate = news.CreatedDate,

                    Title = news.GetLocalizedByLocaleKey(news.Title, news.Id, languageId, "News", "Title"),
                    ShortDesc = news.GetLocalizedByLocaleKey(news.ShortDesc, news.Id, languageId, "News", "ShortDesc"),
                    Description = news.GetLocalizedByLocaleKey(news.Description, news.Id, languageId, "News", "Description"),
                    MetaTitle = news.GetLocalizedByLocaleKey(news.MetaTitle, news.Id, languageId, "News", "MetaTitle"),
                    MetaKeywords = news.GetLocalizedByLocaleKey(news.MetaKeywords, news.Id, languageId, "News", "MetaKeywords"),
                    MetaDescription = news.GetLocalizedByLocaleKey(news.MetaDescription, news.Id, languageId, "News", "MetaDescription")
                };

                ((dynamic)base.ViewBag).Title = newsLocalized.MetaTitle;
				((dynamic)base.ViewBag).KeyWords = newsLocalized.MetaKeywords;
				((dynamic)base.ViewBag).SiteUrl = base.Url.Action("NewsDetail", "News", new { seoUrl = seoUrl, area = "" });
				((dynamic)base.ViewBag).Description = newsLocalized.MetaDescription;
				((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", newsLocalized.ImageMediumSize));
				((dynamic)base.ViewBag).MenuId = newsLocalized.MenuId;
				string[] strArrays = newsLocalized.VirtualCategoryId.Split(new char[] { '/' });
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					string str = strArrays[i];
					MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str), false);
					breadCrumbs.Add(new BreadCrumb()
					{
						Title = menuLink.GetLocalizedByLocaleKey(menuLink.MenuName, menuLink.Id, languageId, "MenuLink", "MenuName"),
						Current = false,
						Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
					});
				}
				breadCrumbs.Add(new BreadCrumb()
				{
					Current = true,
					Title = newsLocalized.Title
				});
				((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;
			}
			((dynamic)base.ViewBag).SeoUrl = newsLocalized.MenuLink.SeoUrl;

			return base.View(newsLocalized);
		}

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (filterContext.RouteData.Values["action"].Equals("edit") || filterContext.RouteData.Values["action"].Equals("create"))
        //    {
        //        IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && (x.TemplateType == 2 || x.TemplateType == 8) && x.ParentId.HasValue, true);
        //        ((dynamic)base.ViewBag).MenuList = menuLinks;
        //        IEnumerable<App.Domain.Entities.Attribute.Attribute> attributes = this._attributeService.FindBy((App.Domain.Entities.Attribute.Attribute x) => x.Status == 1, false);
        //        if (attributes.IsAny<App.Domain.Entities.Attribute.Attribute>())
        //        {
        //            ((dynamic)base.ViewBag).Attributes = attributes;
        //        }
        //    }
        //}
    }
}