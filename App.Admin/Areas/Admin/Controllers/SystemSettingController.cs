using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.FakeEntity.System;
using App.Framework.Ultis;
using App.Service.SystemApp;
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

namespace App.Admin.Controllers
{
	public class SystemSettingController : BaseAdminController
	{
		private readonly ISystemSettingService _systemSettingService;

		public SystemSettingController(ISystemSettingService systemSettingService)
		{
			this._systemSettingService = systemSettingService;
		}

		[RequiredPermisson(Roles="CreateEditSystemSetting")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditSystemSetting")]
		public ActionResult Create(SystemSettingViewModel systemSetting, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(systemSetting);
				}
				else
				{
					if (systemSetting.Status == 1)
					{
						IEnumerable<SystemSetting> systemSettings = this._systemSettingService.FindBy((SystemSetting x) => x.Status == 1, false);
						if (systemSettings.IsAny<SystemSetting>())
						{
							foreach (SystemSetting systemSetting1 in systemSettings)
							{
								systemSetting1.Status = 0;
								this._systemSettingService.Update(systemSetting1);
							}
						}
					}
					if (systemSetting.Icon != null && systemSetting.Icon.ContentLength > 0)
					{
						string fileName = Path.GetFileName(systemSetting.Icon.FileName);
						string extension = Path.GetExtension(systemSetting.Icon.FileName);
						fileName = string.Concat("favicon", extension);
						string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
						systemSetting.Icon.SaveAs(str);
						systemSetting.Favicon = string.Concat(Contains.ImageFolder, fileName);
					}
					if (systemSetting.Logo != null && systemSetting.Logo.ContentLength > 0)
					{
						string fileName1 = Path.GetFileName(systemSetting.Logo.FileName);
						string extension1 = Path.GetExtension(systemSetting.Logo.FileName);
						fileName1 = string.Concat("logo", extension1);
						string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName1);
						systemSetting.Logo.SaveAs(str1);
						systemSetting.LogoImage = string.Concat(Contains.ImageFolder, fileName1);
					}
					SystemSetting systemSetting2 = Mapper.Map<SystemSettingViewModel, SystemSetting>(systemSetting);
					this._systemSettingService.Create(systemSetting2);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.SystemSetting)));
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
				ExtentionUtils.Log(string.Concat("SystemSetting.Create: ", exception.Message));
				base.ModelState.AddModelError("", exception.Message);
				return base.View(systemSetting);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteSystemSetting")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<SystemSetting> systemSettings = 
						from id in ids
						select this._systemSettingService.GetById(int.Parse(id));
					this._systemSettingService.BatchDelete(systemSettings);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("SystemSetting.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditSystemSetting")]
		public ActionResult Edit(int Id)
		{
			SystemSettingViewModel systemSettingViewModel = Mapper.Map<SystemSetting, SystemSettingViewModel>(this._systemSettingService.GetById(Id));
			return base.View(systemSettingViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditSystemSetting")]
		public ActionResult Edit(SystemSettingViewModel systemSetting, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(systemSetting);
				}
				else
				{
					SystemSetting byId = this._systemSettingService.GetById(systemSetting.Id);
					if (systemSetting.Status == 1 && systemSetting.Status != byId.Status)
					{
						IEnumerable<SystemSetting> systemSettings = this._systemSettingService.FindBy((SystemSetting x) => x.Status == 1, false);
						if (systemSettings.IsAny<SystemSetting>())
						{
							foreach (SystemSetting systemSetting1 in systemSettings)
							{
								systemSetting1.Status = 0;
								this._systemSettingService.Update(systemSetting1);
							}
						}
					}
					if (systemSetting.Icon != null && systemSetting.Icon.ContentLength > 0)
					{
						string fileName = Path.GetFileName(systemSetting.Icon.FileName);
						string extension = Path.GetExtension(systemSetting.Icon.FileName);
						fileName = string.Concat("favicon", extension);
						string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
						systemSetting.Icon.SaveAs(str);
						systemSetting.Favicon = string.Concat(Contains.ImageFolder, fileName);
					}
					if (systemSetting.Logo != null && systemSetting.Logo.ContentLength > 0)
					{
						string fileName1 = Path.GetFileName(systemSetting.Logo.FileName);
						string extension1 = Path.GetExtension(systemSetting.Logo.FileName);
						fileName1 = string.Concat("logo", extension1);
						string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName1);
						systemSetting.Logo.SaveAs(str1);
						systemSetting.LogoImage = string.Concat(Contains.ImageFolder, fileName1);
					}
					SystemSetting systemSetting2 = Mapper.Map<SystemSettingViewModel, SystemSetting>(systemSetting, byId);
					this._systemSettingService.Update(systemSetting2);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.SystemSetting)));
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
				ExtentionUtils.Log(string.Concat("SystemSetting.Edit: ", exception.Message));
				return base.View(systemSetting);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewSystemSetting")]
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
			IEnumerable<SystemSetting> systemSettings = this._systemSettingService.PagedList(sortingPagingBuilder, paging);
			if (systemSettings != null && systemSettings.Any<SystemSetting>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(systemSettings);
		}
	}
}