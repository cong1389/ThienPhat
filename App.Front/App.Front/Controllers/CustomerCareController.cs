using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Framework.Ultis;
using App.Front.Models;
using App.Service.Menu;
using App.Service.News;
using App.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class CustomerCareController : FrontBaseController
	{
		private readonly INewsService _newsService;

		private readonly IMenuLinkService _menuLinkService;

		public CustomerCareController(INewsService newsService, IMenuLinkService menuLinkService)
		{
			this._newsService = newsService;
			this._menuLinkService = menuLinkService;
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult GetCustomerCareCategory(string virtualCategoryId, int page, string title)
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
	}
}