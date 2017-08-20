using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Services;
using App.FakeEntity.GenericControl;
using App.Framework.Ultis;
using App.Service.GenericControl;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Admin.Controllers
{
	public class GenericControlValueController : BaseAdminController
	{
		private readonly IGenericControlValueService _GenericControlValueService;

		private readonly IGenericControlService _GenericControlService;

		public GenericControlValueController(IGenericControlValueService GenericControlValueService, IGenericControlService GenericControlService)
		{
			this._GenericControlValueService = GenericControlValueService;
			this._GenericControlService = GenericControlService;
		}

		[RequiredPermisson(Roles="CreateEditDistrict")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditDistrict")]
		public ActionResult Create(GenericControlValueViewModel GenericControlValue, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(GenericControlValue);
				}
				else
				{
					GenericControlValue GenericControlValue1 = Mapper.Map<GenericControlValueViewModel, GenericControlValue>(GenericControlValue);
					this._GenericControlValueService.Create(GenericControlValue1);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Name)));
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
				ExtentionUtils.Log(string.Concat("District.Create: ", exception.Message));
				return base.View(GenericControlValue);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteDistrict")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<GenericControlValue> GenericControlValues = 
						from id in ids
						select this._GenericControlValueService.GetById(int.Parse(id));
					this._GenericControlValueService.BatchDelete(GenericControlValues);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("District.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditDistrict")]
		public ActionResult Edit(int Id)
		{
			GenericControlValueViewModel GenericControlValueViewModel = Mapper.Map<GenericControlValue, GenericControlValueViewModel>(this._GenericControlValueService.GetById(Id));
			return base.View(GenericControlValueViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditDistrict")]
		public ActionResult Edit(GenericControlValueViewModel GenericControlValue, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(GenericControlValue);
				}
				else
				{
					GenericControlValue GenericControlValue1 = Mapper.Map<GenericControlValueViewModel, GenericControlValue>(GenericControlValue);
					this._GenericControlValueService.Update(GenericControlValue1);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Name)));
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
				ExtentionUtils.Log(string.Concat("District.Edit: ", exception.Message));
				return base.View(GenericControlValue);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewDistrict")]
		public ActionResult Index(int page = 1, string keywords = "")
		{
			((dynamic)base.ViewBag).Keywords = keywords;
			SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
			{
				Keywords = keywords,
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
			List<GenericControlValue> list = this._GenericControlValueService.PagedList(sortingPagingBuilder, paging).ToList<GenericControlValue>();
			list.ForEach((GenericControlValue item) => item.GenericControl = this._GenericControlService.GetById(item.GenericControlId));
			if (list != null && list.Any<GenericControlValue>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(list);
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit"))
			{
				IEnumerable<App.Domain.Entities.GenericControl.GenericControl> all = this._GenericControlService.GetAll();
				((dynamic)base.ViewBag).GenericControls = all;
			}
		}
	}
}