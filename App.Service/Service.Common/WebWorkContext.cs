using App.Domain.Entities.Data;
using App.Domain.Entities.Language;
using App.Service.Common;
using App.Service.GenericAttribute;
using App.Service.Language;
using System.Collections.Generic;

namespace App.Service.Common
{
    public partial class WebWorkContext : IWorkContext
    {
        private App.Domain.Entities.Language.Language _cachedLanguage;

        private readonly ILanguageService _languageService;

        private readonly IGenericAttributeService _genericAttributeService;

        public WebWorkContext(IGenericAttributeService genericAttributeService, ILanguageService languageService)
        {
            _genericAttributeService = genericAttributeService;
            _languageService = languageService;
        }

        public App.Domain.Entities.Language.Language WorkingLanguage
        {
            get
            {
                if (_cachedLanguage != null)
                    return _cachedLanguage;

                int customerLangId = 0;
                App.Domain.Entities.Data.GenericAttribute attribute = _genericAttributeService.GetGenericAttributeByKey(1, "Customer", "LanguageId");
                _cachedLanguage = _languageService.GetLanguageById(int.Parse(attribute.Value));

                return _cachedLanguage;
            }
            set
            {
                // _cachedLanguage = value;

                var languageId = value != null ? value.Id : 1;
                SetCustomerLanguage(languageId, 1);
                _cachedLanguage = null;
            }
        }

        private void SetCustomerLanguage(int languageId, int storeId)
        {
            App.Domain.Entities.Data.GenericAttribute objAttribute = new App.Domain.Entities.Data.GenericAttribute
            {
                EntityId = 1,
                KeyGroup = "Customer",
                Key = "LanguageId",
                Value = languageId.ToString(),
                StoreId = storeId
            };

            var attribute = _genericAttributeService.GetGenericAttributeByKey(objAttribute.EntityId
                , objAttribute.KeyGroup, objAttribute.Key);

            if (attribute == null)
                _genericAttributeService.Create(objAttribute);
            else
            {
                attribute.Value = languageId.ToString();
                _genericAttributeService.Update(attribute);
            }
        }

    }
}
