using System;
using System.IO;
using System.Net;

namespace App.Front.Models
{
	public class SpeedSMSAPI
	{
		public const int TYPE_QC = 1;

		public const int TYPE_CSKH = 2;

		public const int TYPE_BRANDNAME = 3;

		private const string rootURL = "http://api.speedsms.vn/index.php";

		private string accessToken = "Dz9QCXHaRUvVHw8k_gXapFDvVlR83Ps6";

		public SpeedSMSAPI()
		{
		}

		public SpeedSMSAPI(string token)
		{
			this.accessToken = token;
		}

		public string getUserInfo()
		{
			string str = "http://api.speedsms.vn/index.php/user/info";
			NetworkCredential networkCredential = new NetworkCredential(this.accessToken, ":x");
			return (new StreamReader((new WebClient()
			{
				Credentials = networkCredential
			}).OpenRead(str))).ReadToEnd();
		}

		public string sendSMS(string phone, string content, int type, string brandname)
		{
			string str = "http://api.speedsms.vn/index.php/sms/send";
			if (phone.Length <= 0 || phone.Length < 10 || phone.Length > 11)
			{
				return "";
			}
			if (content.Equals(""))
			{
				return "";
			}
			if (type < 1 || type > 3)
			{
				return "";
			}
			if (type == 3 && brandname.Equals(""))
			{
				return "";
			}
			if (!brandname.Equals("") && brandname.Length > 11)
			{
				return "";
			}
			NetworkCredential networkCredential = new NetworkCredential(this.accessToken, ":x");
			WebClient webClient = new WebClient()
			{
				Credentials = networkCredential
			};
			webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
			return webClient.UploadString(str, string.Concat(new object[] { "{\"to\":[\"", phone, "\"], \"content\": \"", content, "\", \"type\":", type, ", \"brandname\": \"", brandname, "\"}" }));
		}
	}
}