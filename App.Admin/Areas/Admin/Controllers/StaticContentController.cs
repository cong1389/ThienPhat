using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using App.FakeEntity.Meu;
using App.FakeEntity.Static;
using App.Framework.Ultis;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Menu;
using App.Service.Static;
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
    public class StaticContentController : BaseAdminController
	{
		private readonly IMenuLinkService _menuLinkService;

		private readonly IStaticContentService _staticContentService;

        private readonly ILanguageService _languageService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        public StaticContentController(
            IStaticContentService staticContentService
            , IMenuLinkService menuLinkService
            , ILanguageService languageService
            , ILocalizedPropertyService localizedPropertyService)
		{
			this._staticContentService = staticContentService;
			this._menuLinkService = menuLinkService;
            this._languageService = languageService;
            this._localizedPropertyService = localizedPropertyService;
        }

		[RequiredPermisson(Roles="CreateEditStaticContent")]
		public ActionResult Create()
        {
            var model = new StaticContentViewModel();

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditStaticContent")]
		public ActionResult Create(StaticContentViewModel model, string ReturnUrl)
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
					string str = model.Title.NonAccent();
					IEnumerable<StaticContent> bySeoUrl = this._staticContentService.GetBySeoUrl(str);
					model.SeoUrl = model.Title.NonAccent();
					if (bySeoUrl.Any<StaticContent>((StaticContent x) => x.Id != model.Id))
					{
						StaticContentViewModel staticContentViewModel = model;
						staticContentViewModel.SeoUrl = string.Concat(staticContentViewModel.SeoUrl, "-", bySeoUrl.Count<StaticContent>());
					}
					if (model.Image != null && model.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(model.Image.FileName);
						string extension = Path.GetExtension(model.Image.FileName);
						fileName = string.Concat(model.Title.NonAccent(), extension);
						string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
						model.Image.SaveAs(str1);
						model.ImagePath = string.Concat(Contains.ImageFolder, fileName);
					}

					StaticContent modelMap = Mapper.Map<StaticContentViewModel, StaticContent>(model);
					this._staticContentService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ShortDesc, localized.ShortDesc, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.SeoUrl, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.StaticContent)));
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
				ExtentionUtils.Log(string.Concat("Post.Create: ", exception.Message));
				base.ModelState.AddModelError("", exception.Message);
				return base.View(model);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteStaticContent")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<StaticContent> staticContents = 
						from id in ids
						select this._staticContentService.GetById(int.Parse(id));
					this._staticContentService.BatchDelete(staticContents);

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
				ExtentionUtils.Log(string.Concat("Post.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditStaticContent")]
		public ActionResult Edit(int Id)
		{
			StaticContentViewModel modelMap = Mapper.Map<StaticContent, StaticContentViewModel>(this._staticContentService.GetById(Id));

            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.LocalesId = modelMap.Id;
                locale.Title = modelMap.GetLocalized(x => x.Title, Id, languageId, false, false);
                locale.ShortDesc = modelMap.GetLocalized(x => x.ShortDesc, Id, languageId, false, false);
                locale.Description = modelMap.GetLocalized(x => x.Description, Id, languageId, false, false);
                locale.MetaTitle = modelMap.GetLocalized(x => x.MetaTitle, Id, languageId, false, false);
                locale.MetaKeywords = modelMap.GetLocalized(x => x.MetaKeywords, Id, languageId, false, false);
                locale.MetaDescription = modelMap.GetLocalized(x => x.MetaDescription, Id, languageId, false, false);
                locale.SeoUrl = modelMap.GetLocalized(x => x.SeoUrl, Id, languageId, false, false);
            });

            return base.View(modelMap);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditStaticContent")]
		public ActionResult Edit(StaticContentViewModel model, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(model);
				}
				else
				{
					StaticContent byId = this._staticContentService.GetById(model.Id);
					string str = model.Title.NonAccent();
					IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetBySeoUrl(str);
					model.SeoUrl = model.Title.NonAccent();
					if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != model.Id))
					{
						StaticContentViewModel staticContentViewModel = model;
						staticContentViewModel.SeoUrl = string.Concat(staticContentViewModel.SeoUrl, "-", bySeoUrl.Count<MenuLink>());
					}
					if (model.Image != null && model.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(model.Image.FileName);
						string extension = Path.GetExtension(model.Image.FileName);
						fileName = string.Concat(model.Title.NonAccent(), extension);
						string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
						model.Image.SaveAs(str1);
						model.ImagePath = string.Concat(Contains.ImageFolder, fileName);
					}
					if (model.MenuId > 0)
					{
						MenuLink menuLink = this._menuLinkService.GetById(model.MenuId);
						model.MenuLink = Mapper.Map<MenuLink, MenuLinkViewModel>(menuLink);
					}
					StaticContent modelMap = Mapper.Map<StaticContentViewModel, StaticContent>(model, byId);
					this._staticContentService.Update(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ShortDesc, localized.ShortDesc, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.SeoUrl, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.StaticContent)));
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
				ExtentionUtils.Log(string.Concat("Post.Edit: ", exception.Message));
				return base.View(model);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewStaticContent")]
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
			IEnumerable<StaticContent> staticContents = this._staticContentService.PagedList(sortingPagingBuilder, paging);
			if (staticContents != null && staticContents.Any<StaticContent>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(staticContents);
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit"))
			{
				IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType == 5 || x.TemplateType == 6, false);
				((dynamic)base.ViewBag).MenuList = menuLinks;
			}
		}
	}
}