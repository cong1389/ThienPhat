using System;
using System.Collections.Specialized;
using System.Configuration;

namespace App.Utils
{
	public static class AccountUtils
	{
		public static string XsrfKey
		{
			get
			{
				return ConfigurationManager.AppSettings["XsrfKey"] ?? "XsrfKey";
			}
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			Error
		}
	}
}