using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Location;
using App.Domain.Interfaces.Services;
using App.FakeEntity.ContactInformation;
using App.Framework.Ultis;
using App.Service.ContactInformation;
using App.Service.Locations;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Admin.Controllers
{
	public class ContactInfomationController : BaseAdminController
	{
		private readonly IContactInfoService _contactInfoService;

		private readonly IProvinceService _provinceService;

		public ContactInfomationController(IContactInfoService contactInfoService, IProvinceService provinceService)
		{
			this._contactInfoService = contactInfoService;
			this._provinceService = provinceService;
		}

		[RequiredPermisson(Roles="CreateEditContactInfomation")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditContactInfomation")]
		public ActionResult Create(ContactInformationViewModel contact, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(contact);
				}
				else
				{
					ContactInfomation contactInfomation = Mapper.Map<ContactInformationViewModel, ContactInfomation>(contact);
					this._contactInfoService.Create(contactInfomation);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.ContactInfomation)));
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
				return base.View(contact);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteContactInfomation")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<ContactInfomation> contactInfomations = 
						from id in ids
						select this._contactInfoService.GetById(int.Parse(id));
					this._contactInfoService.BatchDelete(contactInfomations);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("ContactInformation.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditContactInfomation")]
		public ActionResult Edit(int Id)
		{
			ContactInformationViewModel contactInformationViewModel = Mapper.Map<ContactInfomation, ContactInformationViewModel>(this._contactInfoService.GetById(Id));
			return base.View(contactInformationViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditContactInfomation")]
		public ActionResult Edit(ContactInformationViewModel contact, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(contact);
				}
				else
				{
					ContactInfomation contactInfomation = Mapper.Map<ContactInformationViewModel, ContactInfomation>(contact);
					this._contactInfoService.Update(contactInfomation);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.ContactInfomation)));
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
				return base.View(contact);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewContactInfomation")]
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
			IEnumerable<ContactInfomation> contactInfomations = this._contactInfoService.PagedList(sortingPagingBuilder, paging);
			if (contactInfomations != null && contactInfomations.Any<ContactInfomation>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(contactInfomations);
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