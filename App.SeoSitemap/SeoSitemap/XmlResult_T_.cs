using App.SeoSitemap.Common;
using App.SeoSitemap.Serialization;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace App.SeoSitemap
{
	internal class XmlResult<T> : ActionResult
	{
		private readonly IBaseUrlProvider _baseUrlProvider;

		private readonly T data;

		private readonly IUrlValidator _urlValidator;

		internal XmlResult(T data, IUrlValidator urlValidator)
		{
			this.data = data;
			this._urlValidator = urlValidator;
		}

		internal XmlResult(T data, IBaseUrlProvider baseUrlProvider) : this(data, new UrlValidator(new ReflectionHelper()))
		{
			this._baseUrlProvider = baseUrlProvider;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			IUrlValidator urlValidator = this._urlValidator;
			object obj = this.data;
			object mvcBaseUrlProvider = this._baseUrlProvider;
			if (mvcBaseUrlProvider == null)
			{
				mvcBaseUrlProvider = new MvcBaseUrlProvider(context.HttpContext);
			}
			urlValidator.ValidateUrls(obj, (IBaseUrlProvider)mvcBaseUrlProvider);
			HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = "text/xml";
			response.ContentEncoding = Encoding.UTF8;
			response.BufferOutput = false;
			(new XmlSerializer()).SerializeToStream<T>(this.data, response.OutputStream);
		}
	}
}