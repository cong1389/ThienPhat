using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.FakeEntity.ServerMail;
using App.Framework.Ultis;
using App.Service.MailSetting;
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
	public class MailSettingController : BaseAdminController
	{
		private readonly IMailSettingService _mailSettingService;

		public MailSettingController(IMailSettingService mailSettingService)
		{
			this._mailSettingService = mailSettingService;
		}

		[RequiredPermisson(Roles="CreateEditMailSetting")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditMailSetting")]
		public ActionResult Create(ServerMailSettingViewModel serverMail, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(serverMail);
				}
				else
				{
					ServerMailSetting serverMailSetting = Mapper.Map<ServerMailSettingViewModel, ServerMailSetting>(serverMail);
					this._mailSettingService.Create(serverMailSetting);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.ServerMailSetting)));
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
				return base.View(serverMail);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteMailSetting")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<ServerMailSetting> serverMailSettings = 
						from id in ids
						select this._mailSettingService.GetById(int.Parse(id));
					this._mailSettingService.BatchDelete(serverMailSettings);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("ServerMailSetting.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditMailSetting")]
		public ActionResult Edit(int Id)
		{
			ServerMailSettingViewModel serverMailSettingViewModel = Mapper.Map<ServerMailSetting, ServerMailSettingViewModel>(this._mailSettingService.GetById(Id));
			return base.View(serverMailSettingViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditMailSetting")]
		public ActionResult Edit(ServerMailSettingViewModel serverMail, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(serverMail);
				}
				else
				{
					ServerMailSetting serverMailSetting = Mapper.Map<ServerMailSettingViewModel, ServerMailSetting>(serverMail);
					this._mailSettingService.Update(serverMailSetting);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.ServerMailSetting)));
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
				return base.View(serverMail);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewMailSetting")]
		public ActionResult Index(int page = 1, string keywords = "")
		{
			((dynamic)base.ViewBag).Keywords = keywords;
			SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
			{
				Keywords = keywords,
				Sorts = new SortBuilder()
				{
					ColumnName = "FromAddress",
					ColumnOrder = SortBuilder.SortOrder.Descending
				}
			};
			Paging paging = new Paging()
			{
				PageNumber = page,
				PageSize = base._pageSize,
				TotalRecord = 0
			};
			IEnumerable<ServerMailSetting> serverMailSettings = this._mailSettingService.PagedList(sortingPagingBuilder, paging);
			if (serverMailSettings != null && serverMailSettings.Any<ServerMailSetting>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(serverMailSettings);
		}
	}
}