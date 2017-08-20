using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Language;
using App.Domain.Entities.Location;
using App.FakeEntity.ContactInformation;
using App.Framework.Ultis;
using App.Service.ContactInformation;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Locations;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class ContactInformationController : BaseAdminController
    {
        private readonly IContactInfoService _contactInfoService;

        private readonly IProvinceService _provinceService;

        private readonly ILanguageService _languageService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        public ContactInformationController(
            IContactInfoService contactInfoService
            , IProvinceService provinceService
            , ILanguageService languageService
            , ILocalizedPropertyService localizedPropertyService
            )
        {
            this._contactInfoService = contactInfoService;
            this._provinceService = provinceService;
            this._languageService = languageService;
            this._localizedPropertyService = localizedPropertyService;
        }

        [RequiredPermisson(Roles = "CreateEditContactInformation")]
        public ActionResult Create()
        {
            var model = new ContactInformationViewModel();

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditContactInformation")]
        public ActionResult Create(ContactInformationViewModel model, string ReturnUrl)
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
                    ContactInformation modelMap = Mapper.Map<ContactInformationViewModel, ContactInformation>(model);
                    this._contactInfoService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Address, localized.Address, localized.LanguageId);                        
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.ContactInformation)));
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
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "DeleteContactInformation")]
        public ActionResult Delete(string[] ids)
        {
            try
            {
                if (ids.Length != 0)
                {
                    IEnumerable<ContactInformation> ContactInformations =
                        from id in ids
                        select this._contactInfoService.GetById(int.Parse(id));
                    this._contactInfoService.BatchDelete(ContactInformations);

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
                ExtentionUtils.Log(string.Concat("ContactInformation.Delete: ", exception.Message));
            }
            return base.RedirectToAction("Index");
        }

        [RequiredPermisson(Roles = "CreateEditContactInformation")]
        public ActionResult Edit(int Id)
        {
            ContactInformationViewModel modelMap = Mapper.Map<ContactInformation, ContactInformationViewModel>(this._contactInfoService.GetById(Id));

            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.Language = modelMap.Language;
                locale.Title = modelMap.GetLocalized(x => x.Title, Id, languageId, false, false);
                locale.Lag = modelMap.Lag;
                locale.Lat = modelMap.Lat;
                locale.Type = modelMap.Type;
                locale.OrderDisplay = modelMap.OrderDisplay;
                locale.Status = modelMap.Status;
                locale.Email = modelMap.Email;
                locale.Hotline = modelMap.Hotline;
                locale.MobilePhone = modelMap.MobilePhone;
                locale.Address = modelMap.GetLocalized(x => x.Address, Id, languageId, false, false);
                locale.Fax = modelMap.Fax;
                locale.NumberOfStore = modelMap.NumberOfStore;
                locale.ProvinceId = modelMap.ProvinceId;
            });

            return base.View(modelMap);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditContactInformation")]
        public ActionResult Edit(ContactInformationViewModel model, string ReturnUrl)
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
                    ContactInformation modelMap = Mapper.Map<ContactInformationViewModel, ContactInformation>(model);
                    this._contactInfoService.Update(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Address, localized.Address, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.ContactInformation)));
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
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "ViewContactInformation")]
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
            IEnumerable<ContactInformation> contactInformations = this._contactInfoService.PagedList(sortingPagingBuilder, paging);
            if (contactInformations != null && contactInformations.Any<ContactInformation>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(contactInformations);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit"))
            {
                IEnumerable<Province> all = this._provinceService.GetAll();
                ((dynamic)base.ViewBag).Provinces = all;
            }
        }
    }
}