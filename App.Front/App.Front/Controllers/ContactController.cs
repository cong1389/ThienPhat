using App.Core.Common;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.Service.ContactInformation;
using App.Service.Menu;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class ContactController : FrontBaseController
	{
		private readonly IContactInfoService _contactInfoService;

		private readonly IMenuLinkService _menuLinkService;

		public ContactController(IContactInfoService contactInfoService, IMenuLinkService menuLinkService)
		{
			this._contactInfoService = contactInfoService;
			this._menuLinkService = menuLinkService;
		}

		[ChildActionOnly]
		public ActionResult ContactUs(int Id)
		{
			MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.Id == Id, true);
			ContactInfomation contactInfomation = this._contactInfoService.Get((ContactInfomation x) => x.Type == 1 && x.Status == 1, true);
			((dynamic)base.ViewBag).Contact = contactInfomation;
			return base.PartialView(menuLink);
		}
	}
}