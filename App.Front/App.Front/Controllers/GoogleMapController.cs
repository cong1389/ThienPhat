using App.Core.Common;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.Service.ContactInformation;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class GoogleMapController : Controller
	{
		private IContactInfoService _contactInfoService;

		public GoogleMapController(IContactInfoService contactInfoService)
		{
			this._contactInfoService = contactInfoService;
		}

		public ActionResult ShowGoogleMap(int Id)
		{
			ContactInfomation contactInfomation = this._contactInfoService.Get((ContactInfomation x) => x.Id == Id, false);
			return base.View(contactInfomation);
		}
	}
}