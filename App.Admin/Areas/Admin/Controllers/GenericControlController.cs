using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Services;
using App.FakeEntity.GenericControl;
using App.Framework.Ultis;
using App.Service.GenericControl;
using App.Service.Language;
using App.Service.LocalizedProperty;
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
		private readonly IGenericControlService _genericControlService;

        private readonly ILanguageService _languageService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        public GenericControlController(IGenericControlService genericControlService
              , ILanguageService languageService
             , ILocalizedPropertyService localizedPropertyService)
		{
			_genericControlService = genericControlService;
            _languageService = languageService;
            _localizedPropertyService = localizedPropertyService;
        }

		[RequiredPermisson(Roles="CreateEditGenericControl")]
		public ActionResult Create()
        {
            var model = new GenericControlViewModel
            {
                OrderDisplay = 0,
                Status = 1
            };

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
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
                    GenericControl modelMap = Mapper.Map<GenericControlViewModel, App.Domain.Entities.GenericControl.GenericControl>(model);
					this._genericControlService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Name, localized.Name, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                    }

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
					IEnumerable<GenericControl> GenericControls = 
						from id in ids
						select this._genericControlService.GetById(int.Parse(id));
					this._genericControlService.BatchDelete(GenericControls);

                    //Delete localize
                    for (int i = 0; i < ids.Length; i++)
                    {
                        IEnumerable<LocalizedProperty> ieLocalizedProperty
                           = _localizedPropertyService.GetLocalizedPropertyByEntityId(int.Parse(ids[i]));
                        this._localizedPropertyService.BatchDelete(ieLocalizedProperty);
                    }
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
			GenericControlViewModel modelMap = Mapper.Map<App.Domain.Entities.GenericControl.GenericControl, GenericControlViewModel>(this._genericControlService.GetById(Id));

            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.LocalesId = modelMap.Id;
                locale.Name = modelMap.GetLocalized(x => x.Name, Id, languageId, false, false);
                locale.Description = modelMap.GetLocalized(x => x.Description, Id, languageId, false, false);
            });

            return base.View(modelMap);
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
					App.Domain.Entities.GenericControl.GenericControl modelMap = Mapper.Map<GenericControlViewModel, App.Domain.Entities.GenericControl.GenericControl>(model);
					this._genericControlService.Update(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Name, localized.Name, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                    }

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
			IEnumerable<App.Domain.Entities.GenericControl.GenericControl> GenericControls = this._genericControlService.PagedList(sortingPagingBuilder, paging);
			if (GenericControls != null && GenericControls.Any<App.Domain.Entities.GenericControl.GenericControl>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(GenericControls);
		}
	}
}