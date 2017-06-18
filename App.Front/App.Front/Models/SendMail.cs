using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web;
using System.Xml;

namespace App.Front.Models
{
	public class SendMail
	{
		private string FromAddress;

		private string Password;

		private string ReplyTo;

		private int SMTPPort;

		private string UserID;

		private bool EnableSSL;

		private string strSmtpClient;

		private string strToAddress;

		public SendMail()
		{
		}

		public void InitMail(string fromAddress, string smtpClient, string userId, string password, string sMTPPort, bool enableSSL)
		{
			try
			{
				this.FromAddress = fromAddress;
				this.strSmtpClient = smtpClient;
				this.UserID = userId;
				this.Password = password;
				this.SMTPPort = int.Parse(sMTPPort);
				this.EnableSSL = enableSSL;
			}
			catch (Exception exception)
			{
			}
		}

		public void SendToMail(string messageId, string toAddress, string[] param)
		{
			XmlDocument xmlDocument = new XmlDocument();
			string str = string.Concat(HttpContext.Current.Server.MapPath("\\"), "Mailformat.xml");
			string innerText = "";
			string innerText1 = "";
			int i = 0;
			if (File.Exists(str))
			{
				xmlDocument.Load(str);
				XmlNode xmlNodes = xmlDocument.SelectSingleNode(string.Concat("MailFormats/MailFormat[@Id='", messageId, "']"));
				innerText = xmlNodes.SelectSingleNode("Subject").InnerText;
				innerText1 = xmlNodes.SelectSingleNode("Body").InnerText;
				if (param == null)
				{
					throw new Exception("Mail format file not found.");
				}
				for (i = 0; i <= (int)param.Length - 1; i++)
				{
					innerText1 = innerText1.Replace(string.Concat(i.ToString(), "?"), param[i]);
					innerText = innerText.Replace(string.Concat(i.ToString(), "?"), param[i]);
				}
				dynamic mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(this.FromAddress);
				mailMessage.To.Add(toAddress);
				mailMessage.Subject = innerText;
				mailMessage.IsBodyHtml = true;
				mailMessage.Body = innerText1;
				SmtpClient smtpClient = new SmtpClient()
				{
					Host = this.strSmtpClient,
					EnableSsl = this.EnableSSL,
					Port = Convert.ToInt32(this.SMTPPort),
					Credentials = new NetworkCredential(this.UserID, this.Password)
				};
				try
				{
					smtpClient.Send(mailMessage);
				}
				catch (SmtpFailedRecipientsException smtpFailedRecipientsException1)
				{
					SmtpFailedRecipientsException smtpFailedRecipientsException = smtpFailedRecipientsException1;
					for (int j = 0; j <= (int)smtpFailedRecipientsException.InnerExceptions.Length; j++)
					{
						SmtpStatusCode statusCode = smtpFailedRecipientsException.InnerExceptions[j].StatusCode;
						if (statusCode == SmtpStatusCode.MailboxBusy | statusCode == SmtpStatusCode.MailboxUnavailable)
						{
							Thread.Sleep(2000);
							smtpClient.Send(mailMessage);
						}
					}
				}
			}
		}
	}
}