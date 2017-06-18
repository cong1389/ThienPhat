using App.Domain.Entities.Data;
using App.Service.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public class StaticContentController : Controller
    {
        private IStaticContentService _staticContentService;

        public StaticContentController(IStaticContentService staticContentService)
        {
            this._staticContentService = staticContentService;
        }

        // GET: StaticContent
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult GetSlogan(int MenuId)
        {
            StaticContent staticContent = this._staticContentService.Get((StaticContent x) => x.Id == MenuId, true);
            return base.PartialView(staticContent);
        }

        [ChildActionOnly]
        public ActionResult GetHomeIntro(int MenuId)
        {
            StaticContent staticContent = this._staticContentService.Get((StaticContent x) => x.Id == MenuId, true);           
            return base.PartialView(staticContent);
        }

        [ChildActionOnly]
        public ActionResult GetHomeProduct(int MenuId)
        {
            StaticContent staticContent = this._staticContentService.Get((StaticContent x) => x.Id == MenuId, true);
            return base.PartialView(staticContent);
        }
    }
}