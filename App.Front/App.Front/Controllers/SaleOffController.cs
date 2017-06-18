using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Framework.Ultis;
using App.Service.News;
using App.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class SaleOffController : FrontBaseController
	{
		private readonly INewsService _newsService;

		public SaleOffController(INewsService newsService)
		{
			this._newsService = newsService;
		}

		public ActionResult GetSaleOffByCategory(string virtualCategoryId, int page, string title)
		{
			SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
			{
				Keywords = virtualCategoryId,
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
			IEnumerable<News> news = this._newsService.PagedListByMenu(sortingPagingBuilder, paging);
			if (news.IsAny<News>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("GetContent", "Menu", new { page = i }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
				((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;
			}
			((dynamic)base.ViewBag).Title = title;
			return base.PartialView(news);
		}
	}
}