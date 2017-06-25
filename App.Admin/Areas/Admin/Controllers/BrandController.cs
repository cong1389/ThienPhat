using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.Brandes;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Brandes;
using App.Framework.Ultis;
using App.Service.Brandes;
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
	public class BrandController : BaseAdminController
	{
		private readonly IBrandService _BrandService;

		public BrandController(IBrandService BrandService)
		{
			this._BrandService = BrandService;
		}

		[RequiredPermisson(Roles="ViewBrand")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="ViewBrand")]
		public ActionResult Create(BrandViewModel Brand, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(Brand);
				}
				else
				{
					Brand Brand1 = Mapper.Map<BrandViewModel, Brand>(Brand);
					this._BrandService.Create(Brand1);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Brand)));
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
				ExtentionUtils.Log(string.Concat("Brand.Create: ", exception.Message));
				return base.View(Brand);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteBrand")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<Brand> Brands = 
						from id in ids
						select this._BrandService.GetById(int.Parse(id));
					this._BrandService.BatchDelete(Brands);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("Brand.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		
		public ActionResult Edit(int Id)
		{
			BrandViewModel BrandViewModel = Mapper.Map<Brand, BrandViewModel>(this._BrandService.GetById(Id));
			return base.View(BrandViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="ViewBrand")]
		public ActionResult Edit(BrandViewModel BrandView, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(BrandView);
				}
				else
				{
					Brand Brand = Mapper.Map<BrandViewModel, Brand>(BrandView);
					this._BrandService.Update(Brand);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Brand)));
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
				return base.View(BrandView);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewBrand")]
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
			IEnumerable<Brand> Brands = this._BrandService.PagedList(sortingPagingBuilder, paging);
			if (Brands != null && Brands.Any<Brand>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(Brands);
		}
	}
}