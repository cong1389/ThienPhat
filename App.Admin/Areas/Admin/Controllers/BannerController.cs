using App.Core.Utils;
using App.Domain.Entities.Ads;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Ads;
using App.Framework.Ultis;
using App.Service.Ads;
using App.Service.Menu;
using App.Utils;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Admin.Controllers
{
	public class BannerController : BaseAdminController
	{
		private readonly IMenuLinkService _menuLinkService;

		private readonly IBannerService _bannerService;

		private readonly IPageBannerService _pageBannerService;

		public BannerController(IBannerService bannerService, IMenuLinkService menuLinkService, IPageBannerService pageBannerService)
		{
			this._bannerService = bannerService;
			this._menuLinkService = menuLinkService;
			this._pageBannerService = pageBannerService;
		}

		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		public ActionResult Create(BannerViewModel bannerView, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(bannerView);
				}
				else
				{
					if (bannerView.Image != null && bannerView.Image.ContentLength > 0)
					{
                        //string fileName = "Test";
                        //string extension = "Test";
                        string fileName = Path.GetFileName(bannerView.Image.FileName);
                        string extension = Path.GetExtension(bannerView.Image.FileName);
                        //fileName = string.Concat(bannerView.FullName.NonAccent(), extension);
                        string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.AdsFolder)), fileName);
						bannerView.Image.SaveAs(str);
						bannerView.ImgPath = string.Concat(Contains.AdsFolder, fileName);
					}

					Banner banner = Mapper.Map<BannerViewModel, Banner>(bannerView);
					this._bannerService.Create(banner);

					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Banner)));
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
				ExtentionUtils.Log(string.Concat("Banner.Create: ", exception.Message));
				base.ModelState.AddModelError("", exception.Message);
				return base.View(bannerView);
			}
			return action;
		}

		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<Banner> banners = 
						from id in ids
						select this._bannerService.GetById(int.Parse(id));
					this._bannerService.BatchDelete(banners);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("Banner.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		public ActionResult Edit(int Id)
		{
			BannerViewModel bannerViewModel = Mapper.Map<Banner, BannerViewModel>(this._bannerService.GetById(Id));
			return base.View(bannerViewModel);
		}

		[HttpPost]
		public ActionResult Edit(BannerViewModel model, string ReturnUrl)
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
					Banner byId = this._bannerService.GetById(model.Id);
					if (model.Image != null && model.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(model.Image.FileName);
						string extension = Path.GetExtension(model.Image.FileName);
						//fileName = string.Concat(bannerView.FullName.NonAccent(""), extension);
						string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.AdsFolder)), fileName);
						model.Image.SaveAs(str);
						model.ImgPath = string.Concat(Contains.AdsFolder, fileName);
					}
					Banner banner = Mapper.Map<BannerViewModel, Banner>(model, byId);
					this._bannerService.Update(banner);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Banner)));
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
				ExtentionUtils.Log(string.Concat("Banner.Edit: ", exception.Message));
				return base.View(model);
			}
			return action;
		}

		public ActionResult Index(int page = 1, string keywords = "")
		{
			((dynamic)base.ViewBag).Keywords = keywords;
			SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
			{
				Keywords = keywords,
				Sorts = new SortBuilder()
				{
					ColumnName = "Title",
					ColumnOrder = SortBuilder.SortOrder.Descending
				}
			};
			Paging paging = new Paging()
			{
				PageNumber = page,
				PageSize = base._pageSize,
				TotalRecord = 0
			};
			IEnumerable<Banner> banners = this._bannerService.PagedList(sortingPagingBuilder, paging);
			if (banners != null && banners.Any<Banner>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(banners);
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit"))
			{
				IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType != 5, true);
				((dynamic)base.ViewBag).MenuList = menuLinks;
				IEnumerable<PageBanner> pageBanners = this._pageBannerService.FindBy((PageBanner x) => x.Status == 1, false);
				((dynamic)base.ViewBag).PageBanners = pageBanners;
			}
		}
	}
}