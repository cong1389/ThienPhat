using App.Core.Localization;
using App.Service.Language;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Framework.Theme
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        private ITextService _textService;

        protected virtual ITextService TextService
        {
            get
            {
                if (_textService == null)
                {
                    _textService = DependencyResolver.Current.GetService<ITextService>();                    
                }

                return _textService;
            }
        }

        //private WebViewPageHelper _helper;

        //private Localizer t;
        /// <summary>
        /// Get a localized resource
        /// </summary>
        public MvcHtmlString T(string text, params object[] formatterArguments)
        {
            var translated = TextService.Get(text, formatterArguments);
            return
                MvcHtmlString.Create(translated);
        }

        public override void InitHelpers()
        {
            base.InitHelpers();

            //t = NullLocalizer.Instance;
            //_helper = DependencyResolver.Current.GetService<WebViewPageHelper>();
            ////_helper.Initialize(this.ViewContext);
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}
