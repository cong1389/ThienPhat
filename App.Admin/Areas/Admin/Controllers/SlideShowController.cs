using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Entities.Slide;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Slide;
using App.Framework.Ultis;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Slide;
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
	public class SlideShowController : BaseAdminController
	{
		private readonly ISlideShowService _slideShowService;

        private readonly ILanguageService _languageService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        public SlideShowController(
            ISlideShowService slideShowService
            , ILanguageService languageService
            , ILocalizedPropertyService localizedPropertyService
            )
		{
			this._slideShowService = slideShowService;
            this._languageService = languageService;
            this._localizedPropertyService = localizedPropertyService;
        }

		public ActionResult Create()
        {
            var model = new SlideShowViewModel();

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
		}

		[HttpPost]
		public ActionResult Create(SlideShowViewModel model, string ReturnUrl)
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
					if (model.Image != null && model.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(model.Image.FileName);
						string extension = Path.GetExtension(model.Image.FileName);
						fileName = string.Concat(model.Title.NonAccent(), extension);
						string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.AdsFolder)), fileName);
						model.Image.SaveAs(str);
						model.ImgPath = string.Concat(Contains.AdsFolder, fileName);
					}

					SlideShow modelMap = Mapper.Map<SlideShowViewModel, SlideShow>(model);
					this._slideShowService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);                        
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);                        
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.SlideShow)));
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
				ExtentionUtils.Log(string.Concat("SlideShow.Create: ", exception.Message));
				base.ModelState.AddModelError("", exception.Message);
				return base.View(model);
			}
			return action;
		}

		public ActionResult Delete(int[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					int[] numArray = ids;
					for (int i = 0; i < (int)numArray.Length; i++)
					{
						int num = numArray[i];
						SlideShow slideShow = this._slideShowService.Get((SlideShow x) => x.Id == num, false);
						this._slideShowService.Delete(slideShow);

                        //Delete localize
                        IEnumerable<LocalizedProperty> ieLocalizedProperty
                             = _localizedPropertyService.GetLocalizedPropertyByEntityId(num);
                        this._localizedPropertyService.BatchDelete(ieLocalizedProperty);
                    }
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("SlideShow.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		public ActionResult Edit(int Id)
		{
			SlideShowViewModel modelMap = Mapper.Map<SlideShow, SlideShowViewModel>(this._slideShowService.Get((SlideShow x) => x.Id == Id, false));

            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.LocalesId = modelMap.Id;
                locale.Title = modelMap.GetLocalized(x => x.Title, Id, languageId, false, false);            
                locale.Description = modelMap.GetLocalized(x => x.Description, Id, languageId, false, false);                
            });

            return base.View(modelMap);
		}

		[HttpPost]
		public ActionResult Edit(SlideShowViewModel model, string ReturnUrl)
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
					SlideShow slideShow = this._slideShowService.Get((SlideShow x) => x.Id == model.Id, false);
					if (model.Image != null && model.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(model.Image.FileName);
						string extension = Path.GetExtension(model.Image.FileName);
						fileName = string.Concat(model.Title.NonAccent(), extension);
						string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.AdsFolder)), fileName);
						model.Image.SaveAs(str);
						model.ImgPath = string.Concat(Contains.AdsFolder, fileName);
					}

					SlideShow modelMap = Mapper.Map<SlideShowViewModel, SlideShow>(model, slideShow);
					this._slideShowService.Update(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.SlideShow)));
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
				ExtentionUtils.Log(string.Concat("SlideShow.Edit: ", exception.Message));
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
			IEnumerable<SlideShow> slideShows = this._slideShowService.PagedList(sortingPagingBuilder, paging);
			if (slideShows != null && slideShows.Any<SlideShow>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(slideShows);
		}
	}
}