using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Slide;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Slide;
using App.Framework.Ultis;
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

		public SlideShowController(ISlideShowService slideShowService)
		{
			this._slideShowService = slideShowService;
		}

		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		public ActionResult Create(SlideShowViewModel SlideShowView, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(SlideShowView);
				}
				else
				{
					if (SlideShowView.Image != null && SlideShowView.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(SlideShowView.Image.FileName);
						string extension = Path.GetExtension(SlideShowView.Image.FileName);
						fileName = string.Concat(SlideShowView.Title.NonAccent(), extension);
						string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.AdsFolder)), fileName);
						SlideShowView.Image.SaveAs(str);
						SlideShowView.ImgPath = string.Concat(Contains.AdsFolder, fileName);
					}
					SlideShow slideShow = Mapper.Map<SlideShowViewModel, SlideShow>(SlideShowView);
					this._slideShowService.Create(slideShow);
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
				return base.View(SlideShowView);
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
			SlideShowViewModel slideShowViewModel = Mapper.Map<SlideShow, SlideShowViewModel>(this._slideShowService.Get((SlideShow x) => x.Id == Id, false));
			return base.View(slideShowViewModel);
		}

		[HttpPost]
		public ActionResult Edit(SlideShowViewModel SlideShowView, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(SlideShowView);
				}
				else
				{
					SlideShow slideShow = this._slideShowService.Get((SlideShow x) => x.Id == SlideShowView.Id, false);
					if (SlideShowView.Image != null && SlideShowView.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(SlideShowView.Image.FileName);
						string extension = Path.GetExtension(SlideShowView.Image.FileName);
						fileName = string.Concat(SlideShowView.Title.NonAccent(), extension);
						string str = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.AdsFolder)), fileName);
						SlideShowView.Image.SaveAs(str);
						SlideShowView.ImgPath = string.Concat(Contains.AdsFolder, fileName);
					}
					SlideShow slideShow1 = Mapper.Map<SlideShowViewModel, SlideShow>(SlideShowView, slideShow);
					this._slideShowService.Update(slideShow1);
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
				return base.View(SlideShowView);
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