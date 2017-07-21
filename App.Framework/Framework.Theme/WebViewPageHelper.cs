using App.Core.Extensions;
using App.Core.Localization;
using System.Web.Mvc;

namespace App.Framework.Theme
{
    public class WebViewPageHelper
    {
        private bool _initialized;
        private ControllerContext _controllerContext;

        public WebViewPageHelper()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Initialize(ViewContext viewContext)
        {
            if (!_initialized)
            {
                _controllerContext = viewContext.GetMasterControllerContext();
                _initialized = true;
            }
        }
    }
}
