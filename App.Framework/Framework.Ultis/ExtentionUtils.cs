using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace App.Framework.Ultis
{
	public static class ExtentionUtils
	{
		public static string ApplicationName
		{
			get
			{
				return Membership.ApplicationName.ToLower();
			}
		}

		public static int PageSize
		{
			get
			{
				return int.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
			}
		}

		public static bool Log(string txt)
		{
			bool flag;
			try
			{
				HttpContext current = HttpContext.Current;
				string str = "";
				if (current != null)
				{
					str = current.Request.Url.ToString();
				}
				if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Logs")))
				{
					Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Logs"));
				}
				DateTime now = DateTime.Now;
				string str1 = string.Format("{0}/{1}.txt", "Logs", now.ToString("yyyyMMdd"));
				if (!File.Exists(string.Concat(current.Server.MapPath("~/"), str1)))
				{
					File.Create(string.Concat(current.Server.MapPath("~/"), str1));
				}
				FileStream fileStream = File.Open(string.Concat(current.Server.MapPath("~/"), str1), FileMode.Open);
				File.ReadAllLines(string.Concat(current.Server.MapPath("~/"), str1));
				List<string> strs = new List<string>()
				{
					string.Format("{0}\t{1}\t{2}\r\n", DateTime.Now, txt, str)
				};
				File.AppendAllText(string.Concat(current.Server.MapPath("~/"), str1), string.Join(Environment.NewLine, strs));
				fileStream.Close();
				flag = true;
				return flag;
			}
			catch (Exception exception)
			{
			}
			flag = false;
			return flag;
		}

		//public static string NonAccent(this string txt)
		//{
		//	if (string.IsNullOrWhiteSpace(txt))
		//	{
		//		txt = string.Empty;
		//	}
		//	else
		//	{
		//		string[] strArrays = new string[] { "aAeEoOuUiIdDyY", "áàảãạăắằẳẵặâấầẩẫậ", "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ", "éèẻẽẹêếềểễệ", "ÉÈẺẼẸÊẾỀỂỄỆ", "óòỏõọôốồổỗộơớờởỡợ", "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ", "úùủũụưứừửữự", "ÚÙỦŨỤƯỨỪỬỮỰ", "íìỉĩị", "ÍÌỈĨỊ", "đ", "Đ", "ýỳỷỹỵ", "ÝỲỶỸỴ" };
		//		string[] strArrays1 = new string[] { "aAeEoOuUiIdDyY", "áàảãạăắằẳẵặâấầẩẫậ", "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ", "éèẻẽẹêếềểễệ", "ÉÈẺẼẸÊẾỀỂỄỆ", "óòỏõọôốồổỗộơớờởỡợ", "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ", "úùủũụưứừửữự", "ÚÙỦŨỤƯỨỪỬỮỰ", "íìỉĩị", "ÍÌỈĨỊ", "đ", "Đ", "ýỳỷỹỵ", "ÝỲỶỸỴ" };
		//		for (int i = 1; i < (int)strArrays.Length; i++)
		//		{
		//			for (int j = 0; j < strArrays[i].Length; j++)
		//			{
		//				txt = txt.Replace(strArrays1[i][j], strArrays1[0][i - 1]).Replace(strArrays[i][j], strArrays[0][i - 1]);
		//			}
		//		}
		//		txt = Regex.Replace(Regex.Replace(txt, "[^a-zA-Z0-9_-]", "-", RegexOptions.Compiled), "-+", "-", RegexOptions.Compiled).Trim(new char[] { '-' });
		//	}
		//	return txt;
		//}

		public static string ToDisplay(this Enum value)
		{
			string empty;
			if (value != null)
			{
				FieldInfo field = value.GetType().GetField(value.ToString());
				if (field == null)
				{
					empty = string.Empty;
				}
				else
				{
					DisplayAttribute[] customAttributes = (DisplayAttribute[])field.GetCustomAttributes(typeof(DisplayAttribute), false);
					empty = (customAttributes.Length != 0 ? customAttributes[0].GetName() : value.ToString());
				}
			}
			else
			{
				empty = "";
			}
			return empty;
		}

		public static string TrimVietNamMark(this string txt)
		{
			if (string.IsNullOrWhiteSpace(txt))
			{
				txt = string.Empty;
			}
			else
			{
				string[] strArrays = new string[] { "aAeEoOuUiIdDyY", "áàảãạăắằẳẵặâấầẩẫậ", "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ", "éèẻẽẹêếềểễệ", "ÉÈẺẼẸÊẾỀỂỄỆ", "óòỏõọôốồổỗộơớờởỡợ", "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ", "úùủũụưứừửữự", "ÚÙỦŨỤƯỨỪỬỮỰ", "íìỉĩị", "ÍÌỈĨỊ", "đ", "Đ", "ýỳỷỹỵ", "ÝỲỶỸỴ" };
				string[] strArrays1 = new string[] { "aAeEoOuUiIdDyY", "áàảãạăắằẳẵặâấầẩẫậ", "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ", "éèẻẽẹêếềểễệ", "ÉÈẺẼẸÊẾỀỂỄỆ", "óòỏõọôốồổỗộơớờởỡợ", "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ", "úùủũụưứừửữự", "ÚÙỦŨỤƯỨỪỬỮỰ", "íìỉĩị", "ÍÌỈĨỊ", "đ", "Đ", "ýỳỷỹỵ", "ÝỲỶỸỴ" };
				for (int i = 1; i < (int)strArrays.Length; i++)
				{
					for (int j = 0; j < strArrays[i].Length; j++)
					{
						txt = txt.Replace(strArrays1[i][j], strArrays1[0][i - 1]).Replace(strArrays[i][j], strArrays[0][i - 1]);
					}
				}
			}
			return txt;
		}
	}
}