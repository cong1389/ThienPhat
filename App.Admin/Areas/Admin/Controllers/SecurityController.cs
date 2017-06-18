using System;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class SecurityController : Controller
	{
		public SecurityController()
		{
		}

		public ActionResult AccessDined(string ReturnUrl)
		{
			((dynamic)base.ViewBag).ReturnUrl = ReturnUrl;
			return base.View();
		}
	}
}