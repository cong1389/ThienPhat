using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Front
{
    public class RouteConfig
    {
        public RouteConfig()
        {
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("content/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("scripts/{*pathInfo}");
            routes.IgnoreRoute("fonts/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.MapRoute(null, "sitemap.xml", new { controller = "SiteMap", action = "SiteMapXml" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "getprice.html", new { controller = "Post", action = "GetPriceProduct" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "sitemap-images.xml", new { controller = "SiteMap", action = "SiteMapImage" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "gallery-images.html", new { controller = "Post", action = "GetGallery" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "left-fixitem.html", new { controller = "Menu", action = "GetLeftFixItem" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "fixitem-content.html", new { controller = "Post", action = "GetProductHome" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "thanh-vien/quan-ly-tin-rao.html", new { controller = "Account", action = "PostManagement", page = 1 }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "danh-sach-cua-hang.html", new { controller = "StoreList", action = "GetStoreListByProvince" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "ban-do/{Id}.html", new { controller = "GoogleMap", action = "ShowGoogleMap", Id = UrlParameter.Optional }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "gui-lien-he.html", new { controller = "Home", action = "SendContact" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "thanh-vien/dang-ky.html", new { controller = "User", action = "Registration" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "thanh-vien/dang-tin-rao.html", new { controller = "Account", action = "CreatePost" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "thanh-vien/thay-doi-thong-tin-ca-nhan.html", new { controller = "Account", action = "ChangeInfo" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "thanh-vien/sua-tin-rao/{Id}.html", new { controller = "Account", action = "EditPost", Id = 1 }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "filter.html", new { controller = "Post", action = "FillterProduct" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "thanh-vien/quan-ly-tin-rao/trang-{page}.html", new { controller = "Account", action = "PostManagement", page = UrlParameter.Optional }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "thanh-vien/tim-tin-rao.html", new { controller = "Account", action = "SearchPost" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "thanh-vien/thoat.html", new { controller = "Account", action = "LogOff" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "thanh-vien/doi-mat-khau.html", new { controller = "User", action = "ChangePassword" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "thanh-vien/xoa-anh.html", new { controller = "User", action = "DeleteGallery" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "callmeback.html", new { controller = "Home", action = "SendSMS" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "404.html", new { controller = "Home", action = "Error" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "dang-nhap.html", new { controller = "User", action = "Login" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "tin-cho-ban.html", new { controller = "Post", action = "GetPostForYou" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "tin-moi-nhat.html", new { controller = "Post", action = "GetPostLatest" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "tin-theo-chu-de.html", new { controller = "News", action = "GetContentTabsNewsHome" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "quan-huyen", new { controller = "Summary", action = "GetDistrictByProvinceId" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "tim-kiem", new { controller = "Menu", action = "Search" }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "{seoUrl}-tts.html", new { controller = "News", action = "NewsDetail", seoUrl = UrlParameter.Optional }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "{seoUrl}-prs.html", new { controller = "Post", action = "PostDetail", seoUrl = UrlParameter.Optional }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "{menu}.html", new { controller = "Menu", action = "GetContent", menu = UrlParameter.Optional, page = 1 }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "{menu}/trang-{page}.html", new { controller = "Menu", action = "GetContent", menu = UrlParameter.Optional, page = UrlParameter.Optional }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "{catUrl}/{parameters}.html", new { controller = "Post", action = "SearchResult", catUrl = UrlParameter.Optional, parameters = UrlParameter.Optional, page = 1 }, new string[] { "App.Front.Controllers" });
            routes.MapRoute(null, "{catUrl}/{parameters}/trang-{page}.html", new { controller = "Post", action = "SearchResult", catUrl = UrlParameter.Optional, parameters = UrlParameter.Optional, page = UrlParameter.Optional }, new string[] { "App.Front.Controllers" });

            routes.MapRoute("Default", "{controller}/{action}/{id}"
              , new { controller = "Home", action = "Index", id = UrlParameter.Optional }
              , new string[] { "App.Front.Controllers" });

        }
    }
}