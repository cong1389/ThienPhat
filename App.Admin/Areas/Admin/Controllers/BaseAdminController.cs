using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class BaseAdminController : Controller
	{
		public int _pageSize
		{
			get
			{
				return int.Parse(ConfigurationManager.AppSettings["ItemsPerPage"] ?? "10");
			}
		}

		public BaseAdminController()
		{
		}

		protected string RenderRazorViewToString(string viewName, object model)
		{
			string str;
			base.ViewData.Model = model;
			using (StringWriter stringWriter = new StringWriter())
			{
				ViewEngineResult viewEngineResult = ViewEngines.Engines.FindPartialView(base.ControllerContext, viewName);
				ViewContext viewContext = new ViewContext(base.ControllerContext, viewEngineResult.View, base.ViewData, base.TempData, stringWriter);
				viewEngineResult.View.Render(viewContext, stringWriter);
				viewEngineResult.ViewEngine.ReleaseView(base.ControllerContext, viewEngineResult.View);
				str = stringWriter.GetStringBuilder().ToString();
			}
			return str;
		}
	}
}