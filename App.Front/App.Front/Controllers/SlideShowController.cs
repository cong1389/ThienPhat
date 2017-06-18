using App.Domain.Entities.Slide;
using App.Domain.Interfaces.Services;
using App.Service.Slide;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class SlideShowController : FrontBaseController
	{
		private readonly ISlideShowService _slideShowService;

		public SlideShowController(ISlideShowService slideShowService)
		{
			this._slideShowService = slideShowService;
		}

		public ActionResult GetSlideShow()
		{
			IEnumerable<SlideShow> slideShows = this._slideShowService.FindBy((SlideShow x) => x.Status == 1, true);
			return base.PartialView(slideShows);
		}
	}
}