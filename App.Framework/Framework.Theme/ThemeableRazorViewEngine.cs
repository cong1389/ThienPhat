using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Framework.Theme
{
    public class ThemeableRazorViewEngine : ThemeableVirtualPathProviderViewEngine
    {
        public ThemeableRazorViewEngine()
        {
            base.AreaViewLocationFormats = new string[] { "~/Areas/Views/{1}/{0}.cshtml", "~/Areas/Views/Shared/{0}.cshtml", "~/Areas/Views/{1}/{0}.cshtml", "~/Areas/Views/Shared/{0}.cshtml" };
            base.AreaMasterLocationFormats = new string[] { "~/Areas/Views/{1}/{0}.cshtml", "~/Areas/Views/Shared/{0}.cshtml", "~/Areas/Views/{1}/{0}.cshtml", "~/Areas/Views/Shared/{0}.cshtml" };
            base.AreaPartialViewLocationFormats = new string[] { "~/Areas/Views/{1}/{0}.cshtml", "~/Areas/Views/Shared/{0}.cshtml", "~/Areas/Views/{1}/{0}.cshtml", "~/Areas/Views/Shared/{0}.cshtml" };
            base.ViewLocationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml", "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml", "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml", "~/Areas/Admin/Views/{1}/{0}.cshtml", "~/Areas/Admin/Views/Shared/{0}.cshtml" };
            base.MasterLocationFormats = new string[] { "~/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.cshtml", "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml" };
            base.PartialViewLocationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.cshtml", "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml", "~/Areas/Admin/Views/{1}/{0}.cshtml", "~/Areas/Admin/Views/Shared/{0}.cshtml" };
            base.FileExtensions = new string[] { "cshtml" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new RazorView(controllerContext, partialPath, null, false, base.FileExtensions);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new RazorView(controllerContext, viewPath, masterPath, true, base.FileExtensions);
        }
    }
}