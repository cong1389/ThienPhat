using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.Ads;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Ads;
using App.Framework.Ultis;
using App.Service.Ads;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class PageBannerController : BaseAdminController
	{
		private readonly IPageBannerService _pageBannerService;

		public PageBannerController(IPageBannerService pageBannerService)
		{
			this._pageBannerService = pageBannerService;
		}

		[RequiredPermisson(Roles="CreateEditPageBanner")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditPageBanner")]
		public ActionResult Create(PageBannerViewModel pageBannerModel, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(pageBannerModel);
				}
				else
				{
					PageBanner pageBanner = Mapper.Map<PageBannerViewModel, PageBanner>(pageBannerModel);
					this._pageBannerService.Create(pageBanner);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.PageBanner)));
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
				ExtentionUtils.Log(string.Concat("PageBanner.Create: ", exception.Message));
				base.ModelState.AddModelError("", exception.Message);
				return base.View(pageBannerModel);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeletePageBanner")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<PageBanner> pageBanners = 
						from id in ids
						select this._pageBannerService.GetById(int.Parse(id));
					this._pageBannerService.BatchDelete(pageBanners);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("PageBanner.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditPageBanner")]
		public ActionResult Edit(int Id)
		{
			PageBannerViewModel pageBannerViewModel = Mapper.Map<PageBanner, PageBannerViewModel>(this._pageBannerService.GetById(Id));
			return base.View(pageBannerViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditPageBanner")]
		public ActionResult Edit(PageBannerViewModel pageBannerModel, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(pageBannerModel);
				}
				else
				{
					PageBanner byId = this._pageBannerService.GetById(pageBannerModel.Id);
					PageBanner pageBanner = Mapper.Map<PageBannerViewModel, PageBanner>(pageBannerModel, byId);
					this._pageBannerService.Update(pageBanner);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.PageBanner)));
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
				ExtentionUtils.Log(string.Concat("PageBanner.Edit: ", exception.Message));
				return base.View(pageBannerModel);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewPageBanner")]
		public ActionResult Index(int page = 1, string keywords = "")
		{
			((dynamic)base.ViewBag).Keywords = keywords;
			SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
			{
				Keywords = keywords,
				Sorts = new SortBuilder()
				{
					ColumnName = "PageName",
					ColumnOrder = SortBuilder.SortOrder.Descending
				}
			};
			Paging paging = new Paging()
			{
				PageNumber = page,
				PageSize = base._pageSize,
				TotalRecord = 0
			};
			IEnumerable<PageBanner> pageBanners = this._pageBannerService.PagedList(sortingPagingBuilder, paging);
			if (pageBanners != null && pageBanners.Any<PageBanner>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(pageBanners);
		}
	}
}