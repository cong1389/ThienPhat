using System;
using System.Web;

namespace App.SeoSitemap.Common
{
	internal class MvcBaseUrlProvider : IBaseUrlProvider
	{
		private readonly HttpContextBase httpContext;

		public Uri BaseUrl
		{
			get
			{
				return new Uri(string.Format("{0}://{1}{2}", this.httpContext.Request.Url.Scheme, this.httpContext.Request.Url.Authority, this.httpContext.Request.ApplicationPath));
			}
		}

		public MvcBaseUrlProvider(HttpContextBase httpContext)
		{
			this.httpContext = httpContext;
		}
	}
}