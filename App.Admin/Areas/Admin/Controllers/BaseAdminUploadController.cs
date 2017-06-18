using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class BaseAdminUploadController : Controller
	{
		public int _pageSize
		{
			get
			{
				return int.Parse(ConfigurationManager.AppSettings["ItemsPerPage"] ?? "10");
			}
		}

		protected string Key
		{
			get
			{
				return base.ViewData["Key"].ToString();
			}
			set
			{
				base.ViewData["Key"] = value;
			}
		}

		public BaseAdminUploadController()
		{
		}
	}
}