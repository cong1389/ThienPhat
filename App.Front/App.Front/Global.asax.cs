using App.Framework.Theme;
using App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace App.Front
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            Bootstrapper.Run();
            // MvcApplication.ConfigureViewEngines();
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

        }

        private static void ConfigureAntiForgeryTokens()
        {
            AntiForgeryConfig.CookieName = "f";
        }

        private static void ConfigureViewEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemeableRazorViewEngine());
        }
    }
}
