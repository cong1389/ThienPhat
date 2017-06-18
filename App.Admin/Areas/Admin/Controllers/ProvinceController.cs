using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.Location;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Location;
using App.Framework.Ultis;
using App.Service.Locations;
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
	public class ProvinceController : BaseAdminController
	{
		private readonly IProvinceService _provinceService;

		public ProvinceController(IProvinceService provinceService)
		{
			this._provinceService = provinceService;
		}

		[RequiredPermisson(Roles="ViewProvince")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="ViewProvince")]
		public ActionResult Create(ProvinceViewModel province, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(province);
				}
				else
				{
					Province province1 = Mapper.Map<ProvinceViewModel, Province>(province);
					this._provinceService.Create(province1);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Provinces)));
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
				ExtentionUtils.Log(string.Concat("Province.Create: ", exception.Message));
				return base.View(province);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteProvince")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<Province> provinces = 
						from id in ids
						select this._provinceService.GetById(int.Parse(id));
					this._provinceService.BatchDelete(provinces);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("Province.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="ViewProvince")]
		public ActionResult Edit(int Id)
		{
			ProvinceViewModel provinceViewModel = Mapper.Map<Province, ProvinceViewModel>(this._provinceService.GetById(Id));
			return base.View(provinceViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="ViewProvince")]
		public ActionResult Edit(ProvinceViewModel provinceView, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(provinceView);
				}
				else
				{
					Province province = Mapper.Map<ProvinceViewModel, Province>(provinceView);
					this._provinceService.Update(province);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Provinces)));
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
				ExtentionUtils.Log(string.Concat("MailSetting.Create: ", exception.Message));
				return base.View(provinceView);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewProvince")]
		public ActionResult Index(int page = 1, string keywords = "")
		{
			((dynamic)base.ViewBag).Keywords = keywords;
			SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
			{
				Keywords = keywords,
				Sorts = new SortBuilder()
				{
					ColumnName = "Name",
					ColumnOrder = SortBuilder.SortOrder.Descending
				}
			};
			Paging paging = new Paging()
			{
				PageNumber = page,
				PageSize = base._pageSize,
				TotalRecord = 0
			};
			IEnumerable<Province> provinces = this._provinceService.PagedList(sortingPagingBuilder, paging);
			if (provinces != null && provinces.Any<Province>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(provinces);
		}
	}
}