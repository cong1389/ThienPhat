using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.Framework.Ultis;
using App.Front.Models;
using App.Service.Menu;
using App.Service.News;
using App.Service.Static;
using App.Utils;
using App.Utils.MVCHelper;
using System;
using System.Collections.Generic;
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

		public NewsController(INewsService newsService, IMenuLinkService menuLinkService, IStaticContentService staticContentService)
		{
			this._newsService = newsService;
			this._menuLinkService = menuLinkService;
			this._staticContentService = staticContentService;
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult BreadCrumNews(string virtualId)
		{
			((dynamic)base.ViewBag).VirtualId = virtualId;
			List<MenuLink> menuLinks = new List<MenuLink>();
			IEnumerable<MenuLink> menuLinks1 = this._menuLinkService.FindBy((MenuLink x) => x.TemplateType == 1 && x.Status == 1, true);
			if (menuLinks1.IsAny<MenuLink>())
			{
				menuLinks.AddRange(menuLinks1);
			}
			return base.PartialView(menuLinks);
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
			List<News> news = new List<News>();
			IEnumerable<News> top = this._newsService.GetTop<DateTime>(4, (News x) => x.HomeDisplay == true && x.Status == 1, (News x) => x.CreatedDate);
			if (top.IsAny<News>())
			{
				news.AddRange(top);
			}
            return base.PartialView(news);
		}

		public ActionResult GetNewsByCategory(string virtualCategoryId, int? menuId, int page)
		{
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
			IEnumerable<News> news = this._newsService.FindAndSort((News x) => !x.Video && x.Status == 1 
            && x.VirtualCategoryId.Contains(virtualCategoryId) && x.Language=="2"
            , sortBuilder, paging);
			if (news.IsAny<News>())
			{
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("GetContent", "Menu", new { page = i }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
				((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;                
            }
			return base.PartialView(news);
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
			dynamic viewBag = base.ViewBag;
			IStaticContentService staticContentService = this._staticContentService;
			Expression<Func<StaticContent, bool>> status = (StaticContent x) => x.Status == 1;
			viewBag.fixItems = staticContentService.GetTop<int>(3, status, (StaticContent x) => x.ViewCount);
			List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
			News news = this._newsService.Get((News x) => x.SeoUrl.Equals(seoUrl), true);
			if (news != null)
			{
				((dynamic)base.ViewBag).Title = news.MetaTitle;
				((dynamic)base.ViewBag).KeyWords = news.MetaKeywords;
				((dynamic)base.ViewBag).SiteUrl = base.Url.Action("NewsDetail", "News", new { seoUrl = seoUrl, area = "" });
				((dynamic)base.ViewBag).Description = news.MetaDescription;
				((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", news.ImageMediumSize));
				((dynamic)base.ViewBag).MenuId = news.MenuId;
				string[] strArrays = news.VirtualCategoryId.Split(new char[] { '/' });
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					string str = strArrays[i];
					MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str), false);
					breadCrumbs.Add(new BreadCrumb()
					{
						Title = menuLink.MenuName,
						Current = false,
						Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
					});
				}
				breadCrumbs.Add(new BreadCrumb()
				{
					Current = true,
					Title = news.Title
				});
				((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;
			}
			((dynamic)base.ViewBag).SeoUrl = news.MenuLink.SeoUrl;
			return base.View(news);
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