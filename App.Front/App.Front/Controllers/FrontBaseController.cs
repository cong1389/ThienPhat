using System;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class FrontBaseController : Controller
	{
		public int _pageSize
		{
			get
			{
				return 20;
			}
		}

		public FrontBaseController()
		{
		}
	}
}