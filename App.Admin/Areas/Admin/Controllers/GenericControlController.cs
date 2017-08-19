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

namespace App.Admin.Controllers
{
	public class GenericControlController : BaseAdminController
	{
		private readonly IGenericControlService _GenericControlService;

		public GenericControlController(IGenericControlService GenericControlService)
		{
			this._GenericControlService = GenericControlService;
		}

		[RequiredPermisson(Roles="CreateEditGenericControl")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditGenericControl")]
		public ActionResult Create(GenericControlViewModel model, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
                    String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                         .Select(v => v.ErrorMessage + " " + v.Exception));
                    base.ModelState.AddModelError("", messages);
                    return base.View(model);
                }
				else
				{
					App.Domain.Entities.GenericControl.GenericControl GenericControl = Mapper.Map<GenericControlViewModel, App.Domain.Entities.GenericControl.GenericControl>(model);
					this._GenericControlService.Create(GenericControl);

					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.GenericControl)));
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
				ExtentionUtils.Log(string.Concat("GenericControl.Create: ", exception.Message));
				return base.View(model);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteGenericControl")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<App.Domain.Entities.GenericControl.GenericControl> GenericControls = 
						from id in ids
						select this._GenericControlService.GetById(int.Parse(id));
					this._GenericControlService.BatchDelete(GenericControls);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("ServerGenericControl.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditGenericControl")]
		public ActionResult Edit(int Id)
		{
			GenericControlViewModel GenericControlViewModel = Mapper.Map<App.Domain.Entities.GenericControl.GenericControl, GenericControlViewModel>(this._GenericControlService.GetById(Id));
			return base.View(GenericControlViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditGenericControl")]
		public ActionResult Edit(GenericControlViewModel model, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
                    String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                         .Select(v => v.ErrorMessage + " " + v.Exception));
                    base.ModelState.AddModelError("", messages);
                    return base.View(model);
                }
				else
				{
					App.Domain.Entities.GenericControl.GenericControl GenericControl = Mapper.Map<GenericControlViewModel, App.Domain.Entities.GenericControl.GenericControl>(model);
					this._GenericControlService.Update(GenericControl);

					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.GenericControl)));
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
				ExtentionUtils.Log(string.Concat("GenericControl.Create: ", exception.Message));
				return base.View(model);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewGenericControl")]
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
			IEnumerable<App.Domain.Entities.GenericControl.GenericControl> GenericControls = this._GenericControlService.PagedList(sortingPagingBuilder, paging);
			if (GenericControls != null && GenericControls.Any<App.Domain.Entities.GenericControl.GenericControl>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(GenericControls);
		}
	}
}