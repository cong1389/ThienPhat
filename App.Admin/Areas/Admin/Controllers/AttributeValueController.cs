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
using System.Web.Routing;

namespace App.Admin.Controllers
{
	public class AttributeValueController : BaseAdminController
	{
		private readonly IAttributeValueService _attributeValueService;

		private readonly IAttributeService _attributeService;

		public AttributeValueController(IAttributeValueService attributeValueService, IAttributeService attributeService)
		{
			this._attributeValueService = attributeValueService;
			this._attributeService = attributeService;
		}

		[RequiredPermisson(Roles="CreateEditDistrict")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditDistrict")]
		public ActionResult Create(AttributeValueViewModel attributeValue, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(attributeValue);
				}
				else
				{
					AttributeValue attributeValue1 = Mapper.Map<AttributeValueViewModel, AttributeValue>(attributeValue);
					this._attributeValueService.Create(attributeValue1);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.AttributeValue)));
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
				ExtentionUtils.Log(string.Concat("District.Create: ", exception.Message));
				return base.View(attributeValue);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteDistrict")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<AttributeValue> attributeValues = 
						from id in ids
						select this._attributeValueService.GetById(int.Parse(id));
					this._attributeValueService.BatchDelete(attributeValues);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("District.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditDistrict")]
		public ActionResult Edit(int Id)
		{
			AttributeValueViewModel attributeValueViewModel = Mapper.Map<AttributeValue, AttributeValueViewModel>(this._attributeValueService.GetById(Id));
			return base.View(attributeValueViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditDistrict")]
		public ActionResult Edit(AttributeValueViewModel attributeValue, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(attributeValue);
				}
				else
				{
					AttributeValue attributeValue1 = Mapper.Map<AttributeValueViewModel, AttributeValue>(attributeValue);
					this._attributeValueService.Update(attributeValue1);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.AttributeValue)));
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
				ExtentionUtils.Log(string.Concat("District.Edit: ", exception.Message));
				return base.View(attributeValue);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewDistrict")]
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
			List<AttributeValue> list = this._attributeValueService.PagedList(sortingPagingBuilder, paging).ToList<AttributeValue>();
			list.ForEach((AttributeValue item) => item.Attribute = this._attributeService.GetById(item.AttributeId));
			if (list != null && list.Any<AttributeValue>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(list);
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit"))
			{
				IEnumerable<App.Domain.Entities.Attribute.Attribute> all = this._attributeService.GetAll();
				((dynamic)base.ViewBag).Attributes = all;
			}
		}
	}
}