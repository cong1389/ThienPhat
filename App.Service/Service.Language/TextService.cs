using App.Core.Localization;
using App.Service.LocaleStringResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Language
{
    public class TextService: ITextService
    {
        private readonly ILocaleStringResourceService _localizationService;

        public TextService(ILocaleStringResourceService localizationService)
        {
            _localizationService = localizationService;
        }

        public LocalizedString Get(string key, params object[] args)
        {
            try
            {
                var value = _localizationService.GetResource(key);
               
                if (string.IsNullOrEmpty(value))
                {
                    return new LocalizedString(key);
                }

                if (args == null || args.Length == 0)
                {
                    return new LocalizedString(value);
                }

                return new LocalizedString(string.Format(value, args), key, args);
            }
            catch { }

            return new LocalizedString(key);
        }
    }
}
