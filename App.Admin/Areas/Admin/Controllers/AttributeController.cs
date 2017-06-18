using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Attribute;
using App.Framework.Ultis;
using App.Service.Attribute;
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
	public class AttributeController : BaseAdminController
	{
		private readonly IAttributeService _attributeService;

		public AttributeController(IAttributeService attributeService)
		{
			this._attributeService = attributeService;
		}

		[RequiredPermisson(Roles="CreateEditAttribute")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditAttribute")]
		public ActionResult Create(AttributeViewModel attributeView, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(attributeView);
				}
				else
				{
					App.Domain.Entities.Attribute.Attribute attribute = Mapper.Map<AttributeViewModel, App.Domain.Entities.Attribute.Attribute>(attributeView);
					this._attributeService.Create(attribute);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Attribute)));
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
				ExtentionUtils.Log(string.Concat("Attribute.Create: ", exception.Message));
				return base.View(attributeView);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteAttribute")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<App.Domain.Entities.Attribute.Attribute> attributes = 
						from id in ids
						select this._attributeService.GetById(int.Parse(id));
					this._attributeService.BatchDelete(attributes);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("ServerAttribute.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditAttribute")]
		public ActionResult Edit(int Id)
		{
			AttributeViewModel attributeViewModel = Mapper.Map<App.Domain.Entities.Attribute.Attribute, AttributeViewModel>(this._attributeService.GetById(Id));
			return base.View(attributeViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditAttribute")]
		public ActionResult Edit(AttributeViewModel attributeView, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(attributeView);
				}
				else
				{
					App.Domain.Entities.Attribute.Attribute attribute = Mapper.Map<AttributeViewModel, App.Domain.Entities.Attribute.Attribute>(attributeView);
					this._attributeService.Update(attribute);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Attribute)));
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
				ExtentionUtils.Log(string.Concat("Attribute.Create: ", exception.Message));
				return base.View(attributeView);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewAttribute")]
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
			IEnumerable<App.Domain.Entities.Attribute.Attribute> attributes = this._attributeService.PagedList(sortingPagingBuilder, paging);
			if (attributes != null && attributes.Any<App.Domain.Entities.Attribute.Attribute>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(attributes);
		}
	}
}