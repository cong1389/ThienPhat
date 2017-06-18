using App.Domain.Entities.Identity;
using App.FakeEntity.User;
using Microsoft.AspNet.Identity;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class UserController : BaseAccessUserController
	{
		public UserController(UserManager<IdentityUser, Guid> UserManager) : base(UserManager)
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
			ActionResult action;
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
						action = this.RedirectToAction("PostManagement", "Account", new { area = "" });
						return action;
					}
				}
			}
			else if (this.ModelState.IsValid)
			{
				IdentityResult identityResult2 = await this.UserManager.ChangePasswordAsync(this.GetGuid(this.User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
				if (!identityResult2.Succeeded)
				{
					((dynamic)this.ViewBag).Error = "Mật khẩu cũ không chính xác.";
				}
				else
				{
					action = this.RedirectToAction("PostManagement", "Account", new { area = "" });
					return action;
				}
			}
			action = this.View();
			return action;
		}

		protected bool HasPassword()
		{
			IdentityUser identityUser = this.UserManager.FindById<IdentityUser, Guid>(base.GetGuid(base.User.Identity.GetUserId()));
			if (identityUser == null)
			{
				return false;
			}
			return identityUser.PasswordHash != null;
		}

		public ActionResult Login()
		{
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

		public ActionResult Registration()
		{
			return base.View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Registration(RegisterFormViewModel model)
		{
			ActionResult actionResult;
			if (this.ModelState.IsValid)
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
				if (identityResult.Succeeded)
				{
					((dynamic)this.ViewBag).Error = "Đăng ký tài khoản thành công.";
				}
				else
				{
					this.ModelState.AddModelError("", "Đăng ký tài khoản không thành công, Vui lòng thử lại.");
					actionResult = this.View(model);
					return actionResult;
				}
			}
			this.ModelState.AddModelError("", "Đăng ký tài khoản không thành công, Vui lòng thử lại.");
			actionResult = this.View();
			return actionResult;
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