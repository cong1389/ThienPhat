using System;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class ExtentionsController : BaseAdminController
	{
		public ExtentionsController()
		{
		}

		public ActionResult ResourceScript()
		{
			base.Response.ContentType = "text/javascript";
			return base.View();
		}
	}
}