using App.Core.Utils;
using App.Domain.Entities.Language;
using App.FakeEntity.Language;
using App.Framework.Ultis;
using App.Service.Language;
using App.Utils;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class LanguageController : BaseAdminController
    {
        #region Language

        private readonly ILanguageService _langService;

        //private readonly ICommonServices _services;

        public LanguageController(ILanguageService langService)
        {
            this._langService = langService;
            //this._services = services;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return base.View();
        }

        [HttpPost]
        public ActionResult Create(LanguageFormViewModel model, string returnUrl)
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
                    Language modelMap = Mapper.Map<LanguageFormViewModel, Language>(model);
                    this._langService.CreateLanguage(modelMap);
                    string empty = string.Empty;
                    if (model.File != null && model.File.ContentLength > 0)
                    {
                        empty = Path.GetFileName(model.File.FileName);
                        string extension = Path.GetExtension(model.File.FileName);
                        empty = string.Concat(empty.NonAccent(), extension);
                        string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.FolderLanguage)), empty);
                        model.File.SaveAs(str);
                    }
                    if (this._langService.SaveLanguage() > 0)
                    {
                        base.Response.Cookies.Add(new HttpCookie("system_message", MessageUI.SuccessLanguage));
                        if (!base.Url.IsLocalUrl(returnUrl) || returnUrl.Length <= 1 || !returnUrl.StartsWith("/") || returnUrl.StartsWith("//") || returnUrl.StartsWith("/\\"))
                        {
                            action = base.RedirectToAction("Index");
                            return action;
                        }
                        else
                        {
                            action = this.Redirect(returnUrl);
                            return action;
                        }
                    }
                }
                base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                return base.View(model);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("Language.Create: ", exception.Message));
                return base.View(model);
            }
            return action;
        }

        public ActionResult Edit(int Id)
        {
            LanguageFormViewModel languageViewModel = Mapper.Map<Language, LanguageFormViewModel>(_langService.GetLanguageById(Id));
            return base.View(languageViewModel);
        }

        [HttpPost]
        public ActionResult Edit(LanguageFormViewModel model, string ReturnUrl)
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
                    Language modelMap = Mapper.Map<LanguageFormViewModel, Language>(model);
                    this._langService.Update(modelMap);
                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Language)));
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
                ExtentionUtils.Log(string.Concat("Language.Edit: ", exception.Message));
                return base.View(model);
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
                    ColumnName = "LanguageCode",
                    ColumnOrder = SortBuilder.SortOrder.Descending
                }
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = base._pageSize,
                TotalRecord = 0
            };
            IEnumerable<Language> languages = this._langService.PagedList(sortingPagingBuilder, paging);
            if (languages != null && languages.Any<Language>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(languages);
        }

        #endregion

    }
}