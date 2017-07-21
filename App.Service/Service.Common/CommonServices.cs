using App.Service.LocaleStringResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Common
{
    public class CommonServices : ICommonServices
    {
        private readonly Lazy<IWorkContext> _workContext;

        private readonly Lazy<ILocaleStringResourceService> _localization;

        public CommonServices(Lazy<IWorkContext> workContext, Lazy<ILocaleStringResourceService> localization)
        {
            this._workContext = workContext;
            this._localization = localization;
        }

        public IWorkContext WorkContext
        {
            get
            {
                return _workContext.Value;
            }

        }

        public ILocaleStringResourceService Localization
        {
            get
            {
                return _localization.Value;
            }
        }
    }
}
