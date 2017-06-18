using App.Admin.Helpers;
using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Meu;
using App.Framework.Ultis;
using App.Service.Menu;
using App.Utils;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Admin.Controllers
{
	public class MenuController : BaseAdminController
	{
		private readonly IMenuLinkService _menuLinkService;

		public MenuController(IMenuLinkService menuLinkService)
		{
			this._menuLinkService = menuLinkService;
		}

		[RequiredPermisson(Roles="CreateEditMenu")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditMenu")]
		public ActionResult Create(MenuLinkViewModel menuLink, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(menuLink);
				}
				else
				{
					string str = menuLink.MenuName.NonAccent();
					IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetBySeoUrl(str);
					menuLink.SeoUrl = menuLink.MenuName.NonAccent();
					if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != menuLink.Id))
					{
						MenuLinkViewModel menuLinkViewModel = menuLink;
						string seoUrl = menuLinkViewModel.SeoUrl;
						int num = bySeoUrl.Count<MenuLink>();
						menuLinkViewModel.SeoUrl = string.Concat(seoUrl, "-", num.ToString());
					}
					if (menuLink.Image != null && menuLink.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(menuLink.Image.FileName);
						string extension = Path.GetExtension(menuLink.Image.FileName);
						fileName = string.Concat(menuLink.MenuName.NonAccent(), extension);
						string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
						menuLink.Image.SaveAs(str1);
						menuLink.ImageUrl = string.Concat(Contains.ImageFolder, fileName);
					}
					if (menuLink.ImageIcon1 != null && menuLink.ImageIcon1.ContentLength > 0)
					{
						string fileName1 = Path.GetFileName(menuLink.ImageIcon1.FileName);
						string extension1 = Path.GetExtension(menuLink.ImageIcon1.FileName);
						fileName1 = string.Concat(string.Concat(menuLink.MenuName, "-icon").NonAccent(), extension1);
						string str2 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName1);
						menuLink.ImageIcon1.SaveAs(str2);
						menuLink.Icon1 = string.Concat(Contains.ImageFolder, fileName1);
					}
					if (menuLink.ImageIcon2 != null && menuLink.ImageIcon2.ContentLength > 0)
					{
						string fileName2 = Path.GetFileName(menuLink.ImageIcon2.FileName);
						string extension2 = Path.GetExtension(menuLink.ImageIcon2.FileName);
						fileName2 = string.Concat(string.Concat(menuLink.MenuName, "-iconbar").NonAccent(), extension2);
						string str3 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName2);
						menuLink.ImageIcon2.SaveAs(str3);
						menuLink.Icon2 = string.Concat(Contains.ImageFolder, fileName2);
					}
					if (menuLink.ParentId.HasValue)
					{
						string str4 = Guid.NewGuid().ToString();
						menuLink.CurrentVirtualId = str4;
						MenuLink byId = this._menuLinkService.GetById(menuLink.ParentId.Value);
						menuLink.VirtualId = string.Format("{0}/{1}", byId.VirtualId, str4);
						menuLink.VirtualSeoUrl = string.Format("{0}/{1}", byId.SeoUrl, menuLink.SeoUrl);
					}
					else
					{
						string str5 = Guid.NewGuid().ToString();
						menuLink.VirtualId = str5;
						menuLink.CurrentVirtualId = str5;
					}
					MenuLink menuLink1 = Mapper.Map<MenuLinkViewModel, MenuLink>(menuLink);
					this._menuLinkService.Create(menuLink1);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.MenuLink)));
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
				return base.View(menuLink);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeleteMenu")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<MenuLink> menuLinks = 
						from id in ids
						select this._menuLinkService.GetById(int.Parse(id));
					this._menuLinkService.BatchDelete(menuLinks);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("MenuLink.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditMenu")]
		public ActionResult Edit(int Id)
		{
			MenuLinkViewModel menuLinkViewModel = Mapper.Map<MenuLink, MenuLinkViewModel>(this._menuLinkService.GetById(Id));
			return base.View(menuLinkViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditMenu")]
		public ActionResult Edit(MenuLinkViewModel menuLink, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(menuLink);
				}
				else
				{
					MenuLink byId = this._menuLinkService.GetById(menuLink.Id);
					string str = menuLink.MenuName.NonAccent();
					IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetBySeoUrl(str);
					menuLink.SeoUrl = menuLink.MenuName.NonAccent();
					if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != menuLink.Id))
					{
						MenuLinkViewModel menuLinkViewModel = menuLink;
						string seoUrl = menuLinkViewModel.SeoUrl;
						int num = bySeoUrl.Count<MenuLink>();
						menuLinkViewModel.SeoUrl = string.Concat(seoUrl, "-", num.ToString());
					}
					if (menuLink.Image != null && menuLink.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(menuLink.Image.FileName);
						string extension = Path.GetExtension(menuLink.Image.FileName);
						fileName = string.Concat(menuLink.MenuName.NonAccent(), extension);
						string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
						menuLink.Image.SaveAs(str1);
						menuLink.ImageUrl = string.Concat(Contains.ImageFolder, fileName);
					}
					if (menuLink.ImageIcon1 != null && menuLink.ImageIcon1.ContentLength > 0)
					{
						string fileName1 = Path.GetFileName(menuLink.ImageIcon1.FileName);
						string extension1 = Path.GetExtension(menuLink.ImageIcon1.FileName);
						fileName1 = string.Concat(string.Concat(menuLink.MenuName, "-icon").NonAccent(), extension1);
						string str2 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName1);
						menuLink.ImageIcon1.SaveAs(str2);
						menuLink.Icon1 = string.Concat(Contains.ImageFolder, fileName1);
					}
					if (menuLink.ImageIcon2 != null && menuLink.ImageIcon2.ContentLength > 0)
					{
						string fileName2 = Path.GetFileName(menuLink.ImageIcon2.FileName);
						string extension2 = Path.GetExtension(menuLink.ImageIcon2.FileName);
						fileName2 = string.Concat(string.Concat(menuLink.MenuName, "-iconbar").NonAccent(), extension2);
						string str3 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName2);
						menuLink.ImageIcon2.SaveAs(str3);
						menuLink.Icon2 = string.Concat(Contains.ImageFolder, fileName2);
					}
					int? parentId = menuLink.ParentId;
					if (!parentId.HasValue)
					{
						parentId = null;
						menuLink.ParentId = parentId;
						menuLink.VirtualId = byId.CurrentVirtualId;
						menuLink.VirtualSeoUrl = null;
					}
					else
					{
						IMenuLinkService menuLinkService = this._menuLinkService;
						parentId = menuLink.ParentId;
						MenuLink byId1 = menuLinkService.GetById(parentId.Value);
						menuLink.VirtualId = string.Format("{0}/{1}", byId1.VirtualId, byId.CurrentVirtualId);
						menuLink.VirtualSeoUrl = string.Format("{0}/{1}", byId1.SeoUrl, menuLink.SeoUrl);
					}
					Mapper.Map<MenuLinkViewModel, MenuLink>(menuLink, byId);
					this._menuLinkService.Update(byId);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.MenuLink)));
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
				ExtentionUtils.Log(string.Concat("MailSetting.Create: ", exception.Message));
				return base.View(menuLink);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewMenu")]
		public ActionResult Index(int page = 1, string keywords = "")
		{
			((dynamic)base.ViewBag).Keywords = keywords;
			SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
			{
				Keywords = keywords,
				Sorts = new SortBuilder()
				{
					ColumnName = "MenuName",
					ColumnOrder = SortBuilder.SortOrder.Descending
				}
			};
			Paging paging = new Paging()
			{
				PageNumber = page,
				PageSize = base._pageSize,
				TotalRecord = 0
			};
			IEnumerable<MenuLink> menuLinks = this._menuLinkService.PagedList(sortingPagingBuilder, paging);
			if (menuLinks != null && menuLinks.Any<MenuLink>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(menuLinks);
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit"))
			{
				IEnumerable<MenuLink> all = this._menuLinkService.GetAll();
				((dynamic)base.ViewBag).MenuList = all;
			}
		}
	}
}