using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.Identity;
using App.FakeEntity.User;
using App.Framework.Ultis;
using App.Service.Account;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class AccountController : BaseIdentityController
	{
		private readonly IUserService _userService;

		private readonly RoleManager<IdentityRole, Guid> _roleManager;

		public AccountController(IUserService userService, UserManager<IdentityUser, Guid> userManager, RoleManager<IdentityRole, Guid> roleManager) : base(userManager)
		{
			this._userService = userService;
			this._roleManager = roleManager;
		}

		[HttpGet]
		[RequiredPermisson(Roles="CreateEditAccount")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditAccount")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(RegisterFormViewModel model, string ReturnUrl)
		{
			ActionResult action;
			if (!this.ModelState.IsValid)
			{
				action = this.View();
			}
			else
			{
				IdentityUser identityUser = new IdentityUser()
				{
					UserName = model.UserName,
					Address = model.Address,
					FirstName = model.FirstName,
					LastName = model.LastName,
					MiddleName = model.MiddleName,
					Phone = model.Phone,
					Email = model.Email,
					City = model.City,
					State = model.State,
					IsLockedOut = false,
					IsSuperAdmin = false,
					Created = DateTime.UtcNow
				};
				IdentityUser identityUser1 = identityUser;
				IdentityResult identityResult = await this.UserManager.CreateAsync(identityUser1, model.Password);
				IdentityResult identityResult1 = identityResult;
				if (identityResult1.Succeeded)
				{
					if (!model.IsSuperAdmin)
					{
						string item = this.Request["roles"];
						if (!string.IsNullOrEmpty(item))
						{
							string[] strArrays = item.Split(new char[] { ',' });
							await this.UserManager.AddToRolesAsync(identityUser1.Id, strArrays);
						}
					}
					this.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Account)));
					if (!this.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
					{
						action = this.RedirectToAction("Index");
					}
					else
					{
						action = this.Redirect(ReturnUrl);
					}
				}
				else
				{
					this.AddErrors(identityResult1);
					action = this.View(model);
				}
			}
			return action;
		}

		[HttpGet]
		[RequiredPermisson(Roles="CreateEditAccount")]
		public async Task<ActionResult> Edit(string Id)
		{
			Guid guid = this.GetGuid(Id);
			IdentityUser identityUser = await this.UserManager.FindByIdAsync(guid);
			return this.View(Mapper.Map<RegisterFormViewModel>(identityUser));
		}

		[HttpPost]
		public async Task<ActionResult> Edit(RegisterFormViewModel model, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				model.Created = null;
				IdentityUser identityUser = this.UserManager.FindById<IdentityUser, Guid>(model.Id);
				identityUser = Mapper.Map<RegisterFormViewModel, IdentityUser>(model, identityUser);
				IdentityResult identityResult = await this.UserManager.UpdateAsync(identityUser);
				if (identityResult.Succeeded)
				{
					if (model.IsSuperAdmin)
					{
						IList<string> roles = this.UserManager.GetRoles<IdentityUser, Guid>(model.Id);
						this.UserManager.RemoveFromRoles<IdentityUser, Guid>(model.Id, roles.ToArray<string>());
					}
					else
					{
						string item = this.Request["roles"];
						if (!string.IsNullOrEmpty(item))
						{
							IList<string> strs = this.UserManager.GetRoles<IdentityUser, Guid>(model.Id);
							this.UserManager.RemoveFromRoles<IdentityUser, Guid>(model.Id, strs.ToArray<string>());
							string[] strArrays = item.Split(new char[] { ',' });
							this.UserManager.AddToRoles<IdentityUser, Guid>(model.Id, strArrays);
						}
					}
					this.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Account)));
					if (!this.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
					{
						action = this.RedirectToAction("Index");
						return action;
					}
					else
					{
						action = this.Redirect(ReturnUrl);
						return action;
					}
				}
				else
				{
					this.AddErrors(identityResult);
					action = this.View(model);
					return action;
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("Account.Update: ", exception.Message));
			}
			action = this.View();
			return action;
		}

		[RequiredPermisson(Roles="ViewAccount")]
		public async Task<ActionResult> Index(int page = 1, string keywords = "")
		{
			((dynamic)this.ViewBag).Keywords = keywords;
			SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
			{
				Keywords = keywords
			};
			SortBuilder sortBuilder = new SortBuilder()
			{
				ColumnName = "UserName",
				ColumnOrder = SortBuilder.SortOrder.Descending
			};
			sortingPagingBuilder.Sorts = sortBuilder;
			SortingPagingBuilder sortingPagingBuilder1 = sortingPagingBuilder;
			Paging paging = new Paging()
			{
				PageNumber = page,
				PageSize = this._pageSize,
				TotalRecord = 0
			};
			Paging paging1 = paging;
			IEnumerable<App.Domain.Entities.Account.User> users = await this._userService.PagedList(sortingPagingBuilder1, paging1);
			if (users != null && users.Any<App.Domain.Entities.Account.User>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging1.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)this.ViewBag).PageInfo = pageInfo;
			}
			return this.View(users);
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit"))
			{
				List<IdentityRole> list = this._roleManager.Roles.ToList<IdentityRole>();
				((dynamic)base.ViewBag).Roles = list;
			}
		}
	}
}