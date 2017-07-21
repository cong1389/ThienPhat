using App.Service.LocaleStringResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Common
{
    public interface ICommonServices
    {
        IWorkContext WorkContext
        {
            get;
        }

        ILocaleStringResourceService Localization
        {
            get;
        }
    }
}
