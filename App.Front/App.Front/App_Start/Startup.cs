using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace App.Front
{
    public class Startup
    {
        public Startup()
        {
        }

        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(new Action<HttpConfiguration>(WebApiConfig.Register));
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            app.MapSignalR();
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            string str = (new UrlHelper(HttpContext.Current.Request.RequestContext)).Action("Login", "User", new { area = "" });
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString(str)
            });
            app.UseExternalSignInCookie("ExternalCookie");
        }
    }
}