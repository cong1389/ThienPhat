using App.Core.Common;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.Service.Common;
using App.Service.ContactInformation;
using App.Service.Language;
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

        private readonly IWorkContext _workContext;

        public ContactController(IContactInfoService contactInfoService, IMenuLinkService menuLinkService
            , IWorkContext workContext)
		{
			this._contactInfoService = contactInfoService;
			this._menuLinkService = menuLinkService;
            this._workContext = workContext;
        }

		[ChildActionOnly]
		public ActionResult ContactUs(int Id)
		{
            int languageId = _workContext.WorkingLanguage.Id;

            MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.Id == Id, true);

			ContactInformation contactInformation = this._contactInfoService.Get((ContactInformation x) => x.Type == 1 && x.Status == 1, true);
            if (contactInformation == null)
                return HttpNotFound();

            ContactInformation contactInformationLocalize = new ContactInformation
            {
                Lag = contactInformation.Lag,
                Lat = contactInformation.Lat,
                Type = contactInformation.Type,
                Status = contactInformation.Status,
                Email = contactInformation.Email,
                Hotline = contactInformation.Hotline,
                MobilePhone = contactInformation.MobilePhone,
                Fax = contactInformation.Fax,
                NumberOfStore = contactInformation.NumberOfStore,
                ProvinceId = contactInformation.ProvinceId,
                Title = contactInformation.GetLocalizedByLocaleKey(contactInformation.Title, contactInformation.Id, languageId, "ContactInformation", "Title"),
                Address = contactInformation.GetLocalizedByLocaleKey(contactInformation.Address, contactInformation.Id, languageId, "ContactInformation", "Address"),

            };

            ((dynamic)base.ViewBag).Contact = contactInformationLocalize;
			return base.PartialView(menuLink);
		}
	}
}