using App.Domain.Entities.Attribute;
using App.Domain.Interfaces.Services;
using App.Service.Attribute;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class SearchBoxController : Controller
	{
		private IAttributeService _attributeService;

		public SearchBoxController(IAttributeService attributeService)
		{
			this._attributeService = attributeService;
		}

		public ActionResult GetAttributeSearchBox(List<int> attributes = null)
		{
			((dynamic)base.ViewBag).Attributes = attributes;
			IEnumerable<App.Domain.Entities.Attribute.Attribute> attributes1 = this._attributeService.FindBy((App.Domain.Entities.Attribute.Attribute x) => x.Status == 1, false);
			return base.PartialView(attributes1);
		}

		public ActionResult SearchBoxSideBar(List<int> attributes = null, List<double> prices = null, List<int> proids = null)
		{
			((dynamic)base.ViewBag).Attributes = attributes;
			((dynamic)base.ViewBag).ProAttrs = proids;
			((dynamic)base.ViewBag).Prices = prices;
			return base.PartialView();
		}
	}
}