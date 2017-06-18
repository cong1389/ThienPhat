using App.Domain.Entities.Ads;
using App.Domain.Interfaces.Services;
using App.Service.Ads;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class BannerController : FrontBaseController
	{
		private readonly IBannerService _bannerService;

		public BannerController(IBannerService bannerService)
		{
			this._bannerService = bannerService;
		}

		[ChildActionOnly]
		public ActionResult BannerHomeProduct()
		{
			IEnumerable<Banner> banners = this._bannerService.FindBy((Banner x) => !x.MenuId.HasValue && x.Status == 1 && x.PageBanner.Position == 10 && (!x.FromDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) >= (int?)0) && (!x.ToDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) <= (int?)0), false);
			return base.PartialView(banners);
		}

		[ChildActionOnly]
		public ActionResult BannerTop(int? menuId)
		{
			IEnumerable<Banner> banners = this._bannerService.FindBy((Banner x) => x.MenuId == menuId && x.Status == 1 && x.PageBanner.Position == 1 && (!x.FromDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) >= (int?)0) && (!x.ToDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) <= (int?)0), false);
			return base.PartialView(banners);
		}

		[ChildActionOnly]
		public ActionResult BannerTopOfNewsPage(int? menuId)
		{
            Banner banners = this._bannerService.Get((Banner x) => x.MenuId == menuId && x.Status == 1 && x.PageBanner.Position == 1 && (!x.FromDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) >= (int?)0) && (!x.ToDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) <= (int?)0), false);
			return base.PartialView(banners);
		}

		[ChildActionOnly]
		public ActionResult GetBannerBootom(int? menuId)
		{
			IEnumerable<Banner> banners = this._bannerService.FindBy((Banner x) => x.PageBanner.Position == 9 && x.Status == 1 && (!x.FromDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) >= (int?)0) && (!x.ToDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) <= (int?)0), false);
			return base.PartialView(banners);
		}

		[ChildActionOnly]
		public ActionResult GetBannerFooter()
		{
			IEnumerable<Banner> banners = this._bannerService.FindBy((Banner x) => x.PageBanner.Position == 2 && x.Status == 1 && (!x.FromDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) >= (int?)0) && (!x.ToDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) <= (int?)0), false);
			return base.PartialView(banners);
		}

		[ChildActionOnly]
		public ActionResult GetBannerLeft()
		{
			IEnumerable<Banner> banners = this._bannerService.FindBy((Banner x) => x.PageBanner.Position == 3 && x.Status == 1 && (!x.FromDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) >= (int?)0) && (!x.ToDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) <= (int?)0), false);
			return base.PartialView(banners);
		}

		[ChildActionOnly]
		public ActionResult GetBannerMiddle(int? menuId)
		{
			IEnumerable<Banner> banners = this._bannerService.FindBy((Banner x) => x.MenuId == menuId && x.PageBanner.Position == 6 && x.Status == 1 && (!x.FromDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) >= (int?)0) && (!x.ToDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) <= (int?)0), false);
			return base.PartialView(banners);
		}

		[ChildActionOnly]
		public ActionResult GetBannerOnMenu()
		{
			IEnumerable<Banner> banners = this._bannerService.FindBy((Banner x) => x.PageBanner.Position == 8 && x.Status == 1 && (!x.FromDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) >= (int?)0) && (!x.ToDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) <= (int?)0), false);
			return base.PartialView(banners);
		}

		[ChildActionOnly]
		public ActionResult GetBannerRight()
		{
			IEnumerable<Banner> banners = this._bannerService.FindBy((Banner x) => x.PageBanner.Position == 4 && x.Status == 1 && (!x.FromDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) >= (int?)0) && (!x.ToDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) <= (int?)0), false);
			return base.PartialView(banners);
		}

		[ChildActionOnly]
		public ActionResult GetBannerSideBar(int? menuId)
		{
			IEnumerable<Banner> banners = this._bannerService.FindBy((Banner x) => x.MenuId == menuId && x.PageBanner.Position == 5 && x.Status == 1 && (!x.FromDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) >= (int?)0) && (!x.ToDate.HasValue || DbFunctions.DiffHours((TimeSpan?)x.ToDate.Value, (TimeSpan?)DateTimeOffset.UtcNow.Offset) <= (int?)0), false);
			return base.PartialView(banners);
		}
	}
}