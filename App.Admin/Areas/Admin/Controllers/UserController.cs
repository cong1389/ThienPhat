using App.Domain.Entities.Identity;
using App.FakeEntity.User;
using Microsoft.AspNet.Identity;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Owin.Security;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class UserController : BaseDefaultIdentity
	{
		public UserController(UserManager<IdentityUser, Guid> userManager) : base(userManager)
		{
		}

		public ActionResult ChangePassword()
		{
			return base.View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			ActionResult actionResult;
			bool flag = this.HasPassword();
			((dynamic)this.ViewBag).HasLocalPassword = flag;
			((dynamic)this.ViewBag).ReturnUrl = this.Url.Action("Index", "Home");
			if (!flag)
			{
				System.Web.Mvc.ModelState item = this.ModelState["OldPassword"];
				if (item != null)
				{
					item.Errors.Clear();
				}
				if (this.ModelState.IsValid)
				{
					IdentityResult identityResult = await this.UserManager.AddPasswordAsync(this.GetGuid(this.User.Identity.GetUserId()), model.NewPassword);
					IdentityResult identityResult1 = identityResult;
					if (!identityResult1.Succeeded)
					{
						this.AddErrors(identityResult1);
					}
					else
					{
						this.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Password)));
						actionResult = this.View();
						return actionResult;
					}
				}
			}
			else if (this.ModelState.IsValid)
			{
				IdentityResult identityResult2 = await this.UserManager.ChangePasswordAsync(this.GetGuid(this.User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
				IdentityResult identityResult3 = identityResult2;
				if (!identityResult3.Succeeded)
				{
					this.AddErrors(identityResult3);
				}
				else
				{
					this.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Password)));
					actionResult = this.View();
					return actionResult;
				}
			}
			actionResult = this.View();
			return actionResult;
		}

		public ActionResult Login(string ReturnUrl)
		{
			((dynamic)base.ViewBag).ReturnUrl = ReturnUrl;
			return base.View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel login, string ReturnUrl)
		{
			ActionResult action;
			if (this.ModelState.IsValid)
			{
				IdentityUser identityUser = await this.UserManager.FindAsync(login.UserName, login.Password);
				IdentityUser identityUser1 = identityUser;
				if (identityUser1 == null)
				{
					this.ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
				}
				else
				{
					await this.SignInAsync(identityUser1, login.Remember);
					if (!this.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
					{
						action = this.RedirectToAction("Index", "Home");
						return action;
					}
					else
					{
						action = this.Redirect(ReturnUrl);
						return action;
					}
				}
			}
			action = this.View();
			return action;
		}

		public ActionResult LogOff()
		{
			base.AuthenticationManager.SignOut(new string[0]);
			return base.RedirectToAction("Login");
		}

		private async Task SignInAsync(IdentityUser user, bool isPersistent)
		{
			this.AuthenticationManager.SignOut(new string[] { "ExternalCookie" });
			ClaimsIdentity claimsIdentity = await this.UserManager.CreateIdentityAsync(user, "ApplicationCookie");
			IAuthenticationManager authenticationManager = this.AuthenticationManager;
			AuthenticationProperties authenticationProperty = new AuthenticationProperties()
			{
				IsPersistent = isPersistent
			};
			authenticationManager.SignIn(authenticationProperty, new ClaimsIdentity[] { claimsIdentity });
		}
	}
}