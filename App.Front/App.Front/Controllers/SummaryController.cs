using App.Core.Common;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Location;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.Front.Models;
using App.Service.ContactInformation;
using App.Service.Locations;
using App.Service.Menu;
using App.Service.SeoSetting;
using App.Service.SystemApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class SummaryController : FrontBaseController
	{
		private readonly IMenuLinkService _menuLinkService;

		private readonly IProvinceService _provinceService;

		private readonly IDistrictService _districtService;

		private readonly ISystemSettingService _systemSettingService;

		private readonly IContactInfoService _contactInfoService;

		private readonly ISettingSeoGlobalService _settingSeoGlobal;

		public SummaryController(IMenuLinkService menuLinkService, IProvinceService provinceService, IDistrictService districtService, ISystemSettingService systemSettingService, IContactInfoService contactInfoService, ISettingSeoGlobalService settingSeoGlobal)
		{
			this._menuLinkService = menuLinkService;
			this._provinceService = provinceService;
			this._districtService = districtService;
			this._systemSettingService = systemSettingService;
			this._contactInfoService = contactInfoService;
			this._settingSeoGlobal = settingSeoGlobal;
		}

		private List<MenuNav> CreateMenuNav(int? parentId, IEnumerable<MenuNav> source)
		{
			return (
				from x in source
				orderby x.OrderDisplay descending
				select x).Where<MenuNav>((MenuNav x) => {
				int? nullable1 = x.ParentId;
				int? nullable = parentId;
				if (nullable1.GetValueOrDefault() != nullable.GetValueOrDefault())
				{
					return false;
				}
				return nullable1.HasValue == nullable.HasValue;
			}).Select<MenuNav, MenuNav>((MenuNav x) => new MenuNav()
			{
				MenuId = x.MenuId,
				ParentId = x.ParentId,
				MenuName = x.MenuName,
				SeoUrl = x.SeoUrl,
				OrderDisplay = x.OrderDisplay,
				ImageUrl = x.ImageUrl,
				CurrentVirtualId = x.CurrentVirtualId,
				VirtualId = x.VirtualId,
				ChildNavMenu = this.CreateMenuNav(new int?(x.MenuId), source)
			}).ToList<MenuNav>();
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult GetAddressInfo()
		{
			ContactInfomation contactInfomation = this._contactInfoService.Get((ContactInfomation x) => x.Status == 1 && x.Type == 1, true);
			return base.PartialView(contactInfomation);
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult GetCategorySearchBox()
		{
			List<MenuNav> menuNavs = new List<MenuNav>();
			IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType == 2, true);
			if (menuLinks.Any<MenuLink>())
			{
				IEnumerable<MenuNav> menuNav = 
					from x in menuLinks
					select new MenuNav()
					{
						MenuId = x.Id,
						ParentId = x.ParentId,
						MenuName = x.MenuName,
						SeoUrl = x.SeoUrl,
						OrderDisplay = x.OrderDisplay,
						ImageUrl = x.ImageUrl,
						CurrentVirtualId = x.CurrentVirtualId,
						VirtualId = x.VirtualId
					};
				menuNavs = this.CreateMenuNav(null, menuNav);
			}
			return base.PartialView(menuNavs);
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult GetContactHeader()
		{
			SystemSetting systemSetting = this._systemSettingService.Get((SystemSetting x) => x.Status == 1, false);
			return base.PartialView(systemSetting);
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult GetContactOrder()
		{
			ContactInfomation contactInfomation = this._contactInfoService.Get((ContactInfomation x) => x.Status == 1 && x.Type == 1, true);
			return base.PartialView(contactInfomation);
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ContentResult GetContentFooter()
		{
			SystemSetting systemSetting = this._systemSettingService.Get((SystemSetting x) => x.Status == 1, true);
			if (systemSetting == null)
			{
				return base.Content(string.Empty);
			}
			return base.Content(systemSetting.FooterContent);
		}

		[HttpPost]
		public JsonResult GetDistrictByProvinceId(int provinceId)
		{
			if (!base.Request.IsAjaxRequest())
			{
				return null;
			}
			var byProvinceId = 
				from x in this._districtService.GetByProvinceId(provinceId)
				select new { Id = x.Id, Name = x.Name };
			return base.Json(byProvinceId);
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult GetInformationFooter()
		{
			ContactInfomation contactInfomation = this._contactInfoService.Get((ContactInfomation x) => x.Status == 1 && x.Type == 1, true);
			return base.PartialView(contactInfomation);
		}

		
        
        public ActionResult GetLogo()
		{
			SystemSetting systemSetting = this._systemSettingService.Get((SystemSetting x) => x.Status == 1, false);
			return base.PartialView(systemSetting);
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult GetMetaTagsSeo()
		{
			SettingSeoGlobal settingSeoGlobal = this._settingSeoGlobal.Get((SettingSeoGlobal x) => x.Status == 1, false);
			return base.PartialView(settingSeoGlobal);
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult GetProvinceSearchBox()
		{
			IEnumerable<Province> provinces = this._provinceService.FindBy((Province x) => x.Status == 1, false);
			return base.PartialView(provinces);
		}

		[ChildActionOnly]
		[PartialCache("Short")]
		public ActionResult ScriptSippet()
		{
			SettingSeoGlobal settingSeoGlobal = this._settingSeoGlobal.Get((SettingSeoGlobal x) => x.Status == 1, false);
			return base.PartialView(settingSeoGlobal);
		}
	}
}