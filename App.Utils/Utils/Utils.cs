using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;

namespace App.Utils
{
	public static class Utils
	{
		public static string CurrentHost
		{
			get
			{
				return string.Concat("http://", HttpContext.Current.Request.Url.Authority);
			}
		}

		public static bool ExistsCokiee(this HttpCookieCollection cookieCollection, string name)
		{
			if (cookieCollection == null)
			{
				throw new ArgumentNullException("cookieCollection");
			}
			return cookieCollection[name] != null;
		}

		public static bool ExistsFile(this HttpPostedFileBase file)
		{
			if (file != null && file.ContentLength > 0)
			{
				return true;
			}
			return false;
		}

		public static Guid GetGuid(string value)
		{
			Guid guid = new Guid();
			Guid.TryParse(value, out guid);
			return guid;
		}

		public static long GetTime()
		{
			DateTime dateTime = new DateTime(1970, 1, 1);
			TimeSpan universalTime = DateTime.Now.ToUniversalTime() - dateTime;
			return (long)(universalTime.TotalMilliseconds + 0.5);
		}

		public static bool IsAny<T>(this IEnumerable<T> data)
		{
			if (data == null)
			{
				return false;
			}
			return data.Any<T>();
		}

        public static string NonAccent(this string txt)
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
                txt = Regex.Replace(Regex.Replace(txt, "[^a-zA-Z0-9_-]", "-", RegexOptions.Compiled), "-+", "-", RegexOptions.Compiled).Trim(new char[] { '-' });
            }
            return txt.ToLower();
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
			return txt.ToLower();
		}
	}
}