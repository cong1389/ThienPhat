using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.FakeEntity.SeoGlobal;
using App.Framework.Ultis;
using App.Service.SeoSetting;
using App.Utils;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class SeoSettingController : BaseAdminController
	{
		private readonly ISettingSeoGlobalService _settingSeoGlobal;

		public SeoSettingController(ISettingSeoGlobalService settingSeoGlobal)
		{
			this._settingSeoGlobal = settingSeoGlobal;
		}

		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(SettingSeoGlobalViewModel seoSetting, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(seoSetting);
				}
				else
				{
					if (seoSetting.Status == 1)
					{
						IEnumerable<SettingSeoGlobal> settingSeoGlobals = this._settingSeoGlobal.FindBy((SettingSeoGlobal x) => x.Status == 1, false);
						if (settingSeoGlobals.IsAny<SettingSeoGlobal>())
						{
							foreach (SettingSeoGlobal settingSeoGlobal in settingSeoGlobals)
							{
								settingSeoGlobal.Status = 0;
								this._settingSeoGlobal.Update(settingSeoGlobal);
							}
						}
					}
					SettingSeoGlobal settingSeoGlobal1 = Mapper.Map<SettingSeoGlobalViewModel, SettingSeoGlobal>(seoSetting);
					this._settingSeoGlobal.Create(settingSeoGlobal1);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.SettingSeoGlobal)));
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
				ExtentionUtils.Log(string.Concat("SeoGlobal.Create: ", exception.Message));
				base.ModelState.AddModelError("", exception.Message);
				return base.View(seoSetting);
			}
			return action;
		}

		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<SettingSeoGlobal> settingSeoGlobals = 
						from id in ids
						select this._settingSeoGlobal.GetById(int.Parse(id));
					this._settingSeoGlobal.BatchDelete(settingSeoGlobals);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("SeoGlobal.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		public ActionResult Edit(int Id)
		{
			SettingSeoGlobalViewModel settingSeoGlobalViewModel = Mapper.Map<SettingSeoGlobal, SettingSeoGlobalViewModel>(this._settingSeoGlobal.GetById(Id));
			return base.View(settingSeoGlobalViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(SettingSeoGlobalViewModel seoSetting, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(seoSetting);
				}
				else
				{
					SettingSeoGlobal byId = this._settingSeoGlobal.GetById(seoSetting.Id);
					if (seoSetting.Status == 1 && seoSetting.Status != byId.Status)
					{
						IEnumerable<SettingSeoGlobal> settingSeoGlobals = this._settingSeoGlobal.FindBy((SettingSeoGlobal x) => x.Status == 1, false);
						if (settingSeoGlobals.IsAny<SettingSeoGlobal>())
						{
							foreach (SettingSeoGlobal settingSeoGlobal in settingSeoGlobals)
							{
								settingSeoGlobal.Status = 0;
								this._settingSeoGlobal.Update(settingSeoGlobal);
							}
						}
					}
					SettingSeoGlobal settingSeoGlobal1 = Mapper.Map<SettingSeoGlobalViewModel, SettingSeoGlobal>(seoSetting, byId);
					this._settingSeoGlobal.Update(settingSeoGlobal1);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.SettingSeoGlobal)));
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
				ExtentionUtils.Log(string.Concat("SeoGlobal.Edit: ", exception.Message));
				return base.View(seoSetting);
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
					ColumnName = "Id",
					ColumnOrder = SortBuilder.SortOrder.Descending
				}
			};
			Paging paging = new Paging()
			{
				PageNumber = page,
				PageSize = base._pageSize,
				TotalRecord = 0
			};
			IEnumerable<SettingSeoGlobal> settingSeoGlobals = this._settingSeoGlobal.PagedList(sortingPagingBuilder, paging);
			if (settingSeoGlobals != null && settingSeoGlobals.Any<SettingSeoGlobal>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(settingSeoGlobals);
		}
	}
}