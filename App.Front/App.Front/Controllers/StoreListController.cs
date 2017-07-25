using App.Core.Common;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Location;
using App.Domain.Interfaces.Services;
using App.Front.Models;
using App.Service.ContactInformation;
using App.Service.Locations;
using App.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class StoreListController : FrontBaseController
	{
		private readonly IProvinceService _provinceService;

		private readonly IContactInfoService _contactInfoService;

		public StoreListController(IProvinceService provinceService, IContactInfoService contactInfoService)
		{
			this._provinceService = provinceService;
			this._contactInfoService = contactInfoService;
		}

		public ActionResult GetStoreListByProvince(int Id)
		{
			if (!base.Request.IsAjaxRequest())
			{
				return base.Json(new { success = false });
			}
			IEnumerable<ContactInformation> ContactInformations = this._contactInfoService.FindBy((ContactInformation x) => x.Status == 1 && x.ProvinceId == (int?)Id, true);
			List<StoreList> storeLists = new List<StoreList>();
			if (ContactInformations.IsAny<ContactInformation>())
			{
				storeLists.AddRange(
					from item in ContactInformations
					select new StoreList()
					{
						Address = item.Address,
						Lat = item.Lat,
						Lng = item.Lag,
						Phone = item.MobilePhone,
						Title = item.Title
					});
			}
			return base.Json(new { data = storeLists, success = true });
		}

		public ActionResult Index(int Id)
		{
			Province province = this._provinceService.GetTop<int>(1, (Province x) => x.Status == 1, (Province x) => x.OrderDisplay).FirstOrDefault<Province>();
			IEnumerable<Province> top = this._provinceService.GetTop<int>(2147483647, (Province x) => x.Status == 1, (Province x) => x.OrderDisplay);
			((dynamic)base.ViewBag).Provinces = top;
			IEnumerable<ContactInformation> ContactInformations = this._contactInfoService.FindBy((ContactInformation x) => x.Status == 1 && x.ProvinceId == (int?)province.Id, true);
			List<StoreList> storeLists = new List<StoreList>();
			if (ContactInformations.IsAny<ContactInformation>())
			{
				storeLists.AddRange(
					from item in ContactInformations
					select new StoreList()
					{
						Address = item.Address,
						Lat = item.Lat,
						Lng = item.Lag,
						Phone = item.MobilePhone,
						Title = item.Title,
						NumberOfStore = item.NumberOfStore
					});
				((dynamic)base.ViewBag).Data = JsonConvert.SerializeObject(storeLists);
			}
			return base.PartialView(ContactInformations);
		}
	}
}