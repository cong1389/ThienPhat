using App.Admin.Helpers;
using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Other;
using App.Domain.Interfaces.Services;
using App.FileUtil;
using App.Framework.Ultis;
using App.Service.Other;
using App.Utils;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class LandingPageController : BaseAdminController
	{
		private readonly ILandingPageService _landingPageService;

		public LandingPageController(ILandingPageService landingPageService)
		{
			this._landingPageService = landingPageService;
		}

		[HttpPost]
		public ActionResult Approved(string[] ids, int value)
		{
			try
			{
				if (ids.Length != 0)
				{
					string[] strArrays = ids;
					for (int i = 0; i < (int)strArrays.Length; i++)
					{
						int num = int.Parse(strArrays[i]);
						LandingPage landingPage = this._landingPageService.Get((LandingPage x) => x.Id == num, false);
						landingPage.Status = 3;
						this._landingPageService.Update(landingPage);
					}
					base.Response.Cookies.Add(new HttpCookie("system_message", MessageUI.UpdateSuccess));
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.Response.Cookies.Add(new HttpCookie("system_message", "Cập nhật không thành công."));
				ExtentionUtils.Log(string.Concat("ContactInformation.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<LandingPage> list = 
						from id in (
							from id in ids
							select int.Parse(id)).ToList<int>()
						select this._landingPageService.Get((LandingPage x) => x.Id == id, false);
					this._landingPageService.BatchDelete(list);
					base.Response.Cookies.Add(new HttpCookie("system_message", FormUI.DeleteSuccess));
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.Response.Cookies.Add(new HttpCookie("system_message", FormUI.DeleteFail));
				ExtentionUtils.Log(string.Concat("ContactInformation.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		public ActionResult Export()
		{
			IEnumerable<LandingPage> all = this._landingPageService.GetAll();
			List<LandingPageExport> landingPageExports = new List<LandingPageExport>();
			if (all.IsAny<LandingPage>())
			{
				foreach (LandingPage landingPage in all)
				{
					LandingPageExport landingPageExport = new LandingPageExport()
					{
						FullName = landingPage.FullName,
						DateOfBith = landingPage.DateOfBith,
						Email = landingPage.Email,
						PhoneNumber = landingPage.PhoneNumber,
						Status = Common.GetStatusLanddingPage(landingPage.Status),
						PlaceOfGift = string.Concat(landingPage.ContactInformation.Title, " - ", landingPage.ContactInformation.Address)
					};
					landingPageExports.Add(landingPageExport);
				}
			}
			ExcelUtil.ListToExcel<LandingPageExport>(landingPageExports);
			return new EmptyResult();
		}

		[RequiredPermisson(Roles="ViewLandingPage")]
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
			IEnumerable<LandingPage> landingPages = this._landingPageService.PagedList(sortingPagingBuilder, paging);
			if (landingPages != null && landingPages.Any<LandingPage>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(landingPages);
		}

		[HttpPost]
		public ActionResult UnApproved(string[] ids, int value)
		{
			try
			{
				if (ids.Length != 0)
				{
					string[] strArrays = ids;
					for (int i = 0; i < (int)strArrays.Length; i++)
					{
						int num = int.Parse(strArrays[i]);
						LandingPage landingPage = this._landingPageService.Get((LandingPage x) => x.Id == num, false);
						landingPage.Status = 2;
						this._landingPageService.Update(landingPage);
					}
					base.Response.Cookies.Add(new HttpCookie("system_message", MessageUI.UpdateSuccess));
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				base.Response.Cookies.Add(new HttpCookie("system_message", "Cập nhật không thành công."));
				ExtentionUtils.Log(string.Concat("ContactInformation.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}
	}
}