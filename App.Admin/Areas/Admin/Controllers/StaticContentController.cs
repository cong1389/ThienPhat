using App.Admin.Helpers;
using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Meu;
using App.FakeEntity.Static;
using App.Framework.Ultis;
using App.Service.Menu;
using App.Service.Static;
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
using System.Web.Routing;

namespace App.Admin.Controllers
{
	public class StaticContentController : BaseAdminController
	{
		private readonly IMenuLinkService _menuLinkService;

		private readonly IStaticContentService _staticContentService;

		public StaticContentController(IStaticContentService staticContentService, IMenuLinkService menuLinkService)
		{
			this._staticContentService = staticContentService;
			this._menuLinkService = menuLinkService;
		}

		[RequiredPermisson(Roles="CreateEditStaticContent")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditStaticContent")]
		public ActionResult Create(StaticContentViewModel post, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(post);
				}
				else
				{
					string str = post.Title.NonAccent();
					IEnumerable<StaticContent> bySeoUrl = this._staticContentService.GetBySeoUrl(str);
					post.SeoUrl = post.Title.NonAccent();
					if (bySeoUrl.Any<StaticContent>((StaticContent x) => x.Id != post.Id))
					{
						StaticContentViewModel staticContentViewModel = post;
						staticContentViewModel.SeoUrl = string.Concat(staticContentViewModel.SeoUrl, "-", bySeoUrl.Count<StaticContent>());
					}
					if (post.Image != null && post.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(post.Image.FileName);
						string extension = Path.GetExtension(post.Image.FileName);
						fileName = string.Concat(post.Title.NonAccent(), extension);
						string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
						post.Image.SaveAs(str1);
						post.ImagePath = string.Concat(Contains.ImageFolder, fileName);
					}
					StaticContent staticContent = Mapper.Map<StaticContentViewModel, StaticContent>(post);
					this._staticContentService.Create(staticContent);
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
				return base.View(post);
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
			StaticContentViewModel staticContentViewModel = Mapper.Map<StaticContent, StaticContentViewModel>(this._staticContentService.GetById(Id));
			return base.View(staticContentViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditStaticContent")]
		public ActionResult Edit(StaticContentViewModel postView, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
					base.ModelState.AddModelError("", MessageUI.ErrorMessage);
					return base.View(postView);
				}
				else
				{
					StaticContent byId = this._staticContentService.GetById(postView.Id);
					string str = postView.Title.NonAccent();
					IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetBySeoUrl(str);
					postView.SeoUrl = postView.Title.NonAccent();
					if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != postView.Id))
					{
						StaticContentViewModel staticContentViewModel = postView;
						staticContentViewModel.SeoUrl = string.Concat(staticContentViewModel.SeoUrl, "-", bySeoUrl.Count<MenuLink>());
					}
					if (postView.Image != null && postView.Image.ContentLength > 0)
					{
						string fileName = Path.GetFileName(postView.Image.FileName);
						string extension = Path.GetExtension(postView.Image.FileName);
						fileName = string.Concat(postView.Title.NonAccent(), extension);
						string str1 = Path.Combine(base.Server.MapPath(string.Concat("~/", Contains.ImageFolder)), fileName);
						postView.Image.SaveAs(str1);
						postView.ImagePath = string.Concat(Contains.ImageFolder, fileName);
					}
					if (postView.MenuId > 0)
					{
						MenuLink menuLink = this._menuLinkService.GetById(postView.MenuId);
						postView.MenuLink = Mapper.Map<MenuLink, MenuLinkViewModel>(menuLink);
					}
					StaticContent staticContent = Mapper.Map<StaticContentViewModel, StaticContent>(postView, byId);
					this._staticContentService.Update(staticContent);
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
				return base.View(postView);
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