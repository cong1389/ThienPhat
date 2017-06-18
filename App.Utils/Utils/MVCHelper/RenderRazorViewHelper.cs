using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Mvc;

namespace App.Utils.MVCHelper
{
	public static class RenderRazorViewHelper
	{
		public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
		{
			string str;
			controller.ViewData.Model = model;
			using (StringWriter stringWriter = new StringWriter())
			{
				ViewEngineResult viewEngineResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
				ViewContext viewContext = new ViewContext(controller.ControllerContext, viewEngineResult.View, controller.ViewData, controller.TempData, stringWriter);
				viewEngineResult.View.Render(viewContext, stringWriter);
				viewEngineResult.ViewEngine.ReleaseView(controller.ControllerContext, viewEngineResult.View);
				str = stringWriter.GetStringBuilder().ToString();
			}
			return str;
		}
	}
}