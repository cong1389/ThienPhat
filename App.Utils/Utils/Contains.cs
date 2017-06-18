using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Web;

namespace App.Utils
{
	public static class Contains
	{
		public static string AdsFolder
		{
			get
			{
				string item = ConfigurationManager.AppSettings["AdsFolder"] ?? "images/Ads/";
				if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
				{
					Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
				}
				return ConfigurationManager.AppSettings["AdsFolder"] ?? "images/Ads/";
			}
		}

		public static bool EnableSendEmai
		{
			get
			{
				return bool.Parse(ConfigurationManager.AppSettings["EnableSendEmail"] ?? "false");
			}
		}

		public static bool EnableSendSMS
		{
			get
			{
				return bool.Parse(ConfigurationManager.AppSettings["EnableSendSMS"] ?? "false");
			}
		}

		public static string FolderLanguage
		{
			get
			{
				string item = ConfigurationManager.AppSettings["LanguageFolder"] ?? "images/language/";
				if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
				{
					Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
				}
				return ConfigurationManager.AppSettings["LanguageFolder"] ?? "images/language/";
			}
		}

		public static string ImageFolder
		{
			get
			{
				string item = ConfigurationManager.AppSettings["ImageFolder"] ?? "images/";
				if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
				{
					Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
				}
				return ConfigurationManager.AppSettings["ImageFolder"] ?? "images/";
			}
		}

		public static string NewsFolder
		{
			get
			{
				string item = ConfigurationManager.AppSettings["NewsFolder"] ?? "images/news/";
				if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
				{
					Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
				}
				return ConfigurationManager.AppSettings["NewsFolder"] ?? "images/news/";
			}
		}

		public static string PostFolder
		{
			get
			{
				string item = ConfigurationManager.AppSettings["PostFolder"] ?? "images/post/";
				if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
				{
					Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
				}
				return ConfigurationManager.AppSettings["PostFolder"] ?? "images/post/";
			}
		}

        public static string AssessmentFolder
        {
            get
            {
                string item = ConfigurationManager.AppSettings["AssessmentFolder"] ?? "images/assessment/";
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
                }
                return ConfigurationManager.AppSettings["AssessmentFolder"] ?? "images/assessment/";
            }
        }

        public static bool RequiredActiveAccount
		{
			get
			{
				return bool.Parse(ConfigurationManager.AppSettings["RequiredActiveAccount"] ?? "false");
			}
		}
	}
}