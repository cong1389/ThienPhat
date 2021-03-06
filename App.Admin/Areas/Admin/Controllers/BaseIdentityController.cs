using App.Domain.Entities.Identity;
using App.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public abstract class BaseIdentityController : BaseAdminController
	{
		protected readonly UserManager<IdentityUser, Guid> UserManager;

		protected string XsrfKey = AccountUtils.XsrfKey;

		protected IAuthenticationManager AuthenticationManager
		{
			get
			{
				return base.HttpContext.GetOwinContext().Authentication;
			}
		}

		protected BaseIdentityController()
		{
		}

		protected BaseIdentityController(UserManager<IdentityUser, Guid> userManager)
		{
			this.UserManager = userManager;
		}

		protected void AddErrors(IdentityResult result)
		{
			foreach (string error in result.Errors)
			{
				base.ModelState.AddModelError("", error);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.UserManager != null)
			{
				this.UserManager.Dispose();
			}
			base.Dispose(disposing);
		}

		protected Guid GetGuid(string value)
		{
			Guid guid = new Guid();
			Guid.TryParse(value, out guid);
			return guid;
		}

		protected IList<AuthenticationDescription> GetUnassignedExternalLogins(IList<UserLoginInfo> userLogins)
		{
			return (
				from auth in this.AuthenticationManager.GetAuthenticationTypes()
				where userLogins.All<UserLoginInfo>((UserLoginInfo ul) => auth.AuthenticationType != ul.LoginProvider)
				select auth).ToList<AuthenticationDescription>();
		}
	}
}