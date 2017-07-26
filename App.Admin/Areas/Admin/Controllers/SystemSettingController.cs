using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.FakeEntity.System;
using App.Framework.Ultis;
using App.Service.Language;
using App.Service.LocalizedProperty;
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

        private readonly ILanguageService _languageService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        public SystemSettingController(
            ISystemSettingService systemSettingService
            , ILanguageService languageService
            , ILocalizedPropertyService localizedPropertyService
            )
		{
			this._systemSettingService = systemSettingService;
            this._languageService = languageService;
            this._localizedPropertyService = localizedPropertyService;
        }

		[RequiredPermisson(Roles="CreateEditSystemSetting")]
		public ActionResult Create()
        {
            var model = new SystemSettingViewModel();

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditSystemSetting")]
		public ActionResult Create(SystemSettingViewModel model, string ReturnUrl)
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
					if (model.Status == 1)
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
					if (model.Icon != null && model.Icon.ContentLength > 0)
					{
						string fileName = Path.GetFileName(model.Icon.FileName);
						string extension = Path.GetExtension(model.Icon.FileName);
						fileName = string.Concat("favicon", extension);
						string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
						model.Icon.SaveAs(str);
						model.Favicon = string.Concat(Contains.ImageFolder, fileName);
					}
					if (model.Logo != null && model.Logo.ContentLength > 0)
					{
						string fileName1 = Path.GetFileName(model.Logo.FileName);
						string extension1 = Path.GetExtension(model.Logo.FileName);
						fileName1 = string.Concat("logo", extension1);
						string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName1);
						model.Logo.SaveAs(str1);
						model.LogoImage = string.Concat(Contains.ImageFolder, fileName1);
					}

					SystemSetting modelMap = Mapper.Map<SystemSettingViewModel, SystemSetting>(model);
					this._systemSettingService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.FooterContent, localized.FooterContent, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.TimeWork, localized.TimeWork, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Slogan, localized.Slogan, localized.LanguageId);
                    }

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
				return base.View(model);
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
			SystemSettingViewModel modelMap = Mapper.Map<SystemSetting, SystemSettingViewModel>(this._systemSettingService.GetById(Id));

            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.LocalesId = modelMap.Id;
                locale.Language = modelMap.Language;
                locale.Status = modelMap.Status;
                locale.Favicon = modelMap.Favicon;
                locale.LogoImage = modelMap.LogoImage;
                locale.MaintanceSite = modelMap.MaintanceSite;
                locale.Hotline = modelMap.Hotline;
                locale.Email = modelMap.Email;
                locale.TimeWork = modelMap.TimeWork;

                locale.Title = modelMap.GetLocalized(x => x.Title, Id, languageId, false, false);
                locale.FooterContent = modelMap.GetLocalized(x => x.FooterContent, Id, languageId, false, false);
                locale.Description = modelMap.GetLocalized(x => x.Description, Id, languageId, false, false);
                locale.MetaTitle = modelMap.GetLocalized(x => x.MetaTitle, Id, languageId, false, false);
                locale.MetaKeywords = modelMap.GetLocalized(x => x.MetaKeywords, Id, languageId, false, false);
                locale.MetaDescription = modelMap.GetLocalized(x => x.MetaDescription, Id, languageId, false, false);
                locale.Slogan = modelMap.GetLocalized(x => x.Slogan, Id, languageId, false, false);
            });

            return base.View(modelMap);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditSystemSetting")]
		public ActionResult Edit(SystemSettingViewModel model, string ReturnUrl)
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
					SystemSetting byId = this._systemSettingService.GetById(model.Id);
					if (model.Status == 1 && model.Status != byId.Status)
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
					if (model.Icon != null && model.Icon.ContentLength > 0)
					{
						string fileName = Path.GetFileName(model.Icon.FileName);
						string extension = Path.GetExtension(model.Icon.FileName);
						fileName = string.Concat("favicon", extension);
						string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
						model.Icon.SaveAs(str);
						model.Favicon = string.Concat(Contains.ImageFolder, fileName);
					}
					if (model.Logo != null && model.Logo.ContentLength > 0)
					{
						string fileName1 = Path.GetFileName(model.Logo.FileName);
						string extension1 = Path.GetExtension(model.Logo.FileName);
						fileName1 = string.Concat("logo", extension1);
						string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName1);
						model.Logo.SaveAs(str1);
						model.LogoImage = string.Concat(Contains.ImageFolder, fileName1);
					}
					SystemSetting modelMap = Mapper.Map<SystemSettingViewModel, SystemSetting>(model, byId);
					this._systemSettingService.Update(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.FooterContent, localized.FooterContent, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.TimeWork, localized.TimeWork, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Slogan, localized.Slogan, localized.LanguageId);
                    }

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
				return base.View(model);
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