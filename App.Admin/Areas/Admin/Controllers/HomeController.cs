using App.Admin.Helpers;
using System;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	[AuthorizeCustom]
	public class HomeController : BaseAdminController
	{
		public HomeController()
		{
		}

		public ActionResult Index()
		{
			return base.View();
		}
	}
}