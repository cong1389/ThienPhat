using App.Domain.Entities.Slide;
using App.Domain.Interfaces.Services;
using App.Service.Common;
using App.Service.Language;
using App.Service.Slide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public class SlideShowController : FrontBaseController
    {
        private readonly ISlideShowService _slideShowService;

        private readonly IWorkContext _workContext;

        public SlideShowController(ISlideShowService slideShowService, IWorkContext workContext)
        {
            this._slideShowService = slideShowService;
            this._workContext = workContext;
        }

        public ActionResult GetSlideShow()
        {
            int languageId = _workContext.WorkingLanguage.Id;

            IEnumerable<SlideShow> slideShows = this._slideShowService.FindBy((SlideShow x) => x.Status == 1, true);

            if (slideShows == null)
                return HttpNotFound();

            IEnumerable<SlideShow> ieSlideShowLocalized = from x in slideShows
                                                 select new SlideShow()
                                                 {
                                                     Id = x.Id,
                                                     Status = x.Status,
                                                     WebsiteLink = x.WebsiteLink,
                                                     ImgPath = x.ImgPath,
                                                     Video = x.Video,
                                                     Width = x.Width,
                                                     Height = x.Height,
                                                     Target = x.Target,
                                                     FromDate = x.FromDate,
                                                     ToDate = x.ToDate,
                                                     OrderDisplay = x.OrderDisplay,
                                                     Title = x.GetLocalizedByLocaleKey(x.Title, x.Id, languageId, "SlideShow", "Title"),
                                                     Description = x.GetLocalizedByLocaleKey(x.Description, x.Id, languageId, "SlideShow", "Title"),
                                                 };

            return base.PartialView(ieSlideShowLocalized);
        }
    }
}