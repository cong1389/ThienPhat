using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Language;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.Service.LocalizedProperty
{
    public class LocalizedPropertyService : BaseService<App.Domain.Entities.Language.LocalizedProperty>, ILocalizedPropertyService, IBaseService<App.Domain.Entities.Language.LocalizedProperty>, IService
    {
        private readonly ILocalizedPropertyRepository _localizedPropertyRepository;

        private readonly IUnitOfWork _unitOfWork;

        public LocalizedPropertyService(IUnitOfWork unitOfWork, ILocalizedPropertyRepository LocalizedPropertyRepository) : base(unitOfWork, LocalizedPropertyRepository)
        {
            this._unitOfWork = unitOfWork;
            this._localizedPropertyRepository = LocalizedPropertyRepository;
        }

        public void CreateLocalizedProperty(App.Domain.Entities.Language.LocalizedProperty LocalizedProperty)
        {
            this._localizedPropertyRepository.Add(LocalizedProperty);
        }

        public App.Domain.Entities.Language.LocalizedProperty GetLocalizedPropertyById(int Id)
        {
            return this._localizedPropertyRepository.GetId(Id);
        }

        public IEnumerable<App.Domain.Entities.Language.LocalizedProperty> GetLocalizedPropertyByEntityId(int entityId)
        {
            IEnumerable<App.Domain.Entities.Language.LocalizedProperty> localizedProperty = this._localizedPropertyRepository.FindBy((App.Domain.Entities.Language.LocalizedProperty x) => x.EntityId == entityId, false);
            return localizedProperty;
        }

        public App.Domain.Entities.Language.LocalizedProperty GetLocalizedPropertByKey(int languageId,int entityId, string localeKeyGroup, string localeKey)
        {
            App.Domain.Entities.Language.LocalizedProperty attr = this.Get((App.Domain.Entities.Language.LocalizedProperty x) =>
                x.LanguageId.Equals(languageId)
                && x.EntityId.Equals(entityId)
                 && x.LocaleKeyGroup.Equals(localeKeyGroup)
                 && x.LocaleKey.Equals(localeKey)
                , false);
            return attr;
        }

        public IEnumerable<App.Domain.Entities.Language.LocalizedProperty> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._localizedPropertyRepository.PagedSearchList(sortbuBuilder, page);
        }

        public int SaveLocalizedProperty()
        {
            return this._unitOfWork.Commit();
        }

        public virtual void SaveLocalizedValue<T>(
            T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            int languageId) where T : BaseEntity
        {
            SaveLocalizedValueItem<T, string>(entity, keySelector, localeValue, languageId);
        }

        public virtual void SaveLocalizedValueItem<T, TPropType>(
           T entity,
           Expression<Func<T, TPropType>> keySelector,
           TPropType localeValue,
           int languageId) where T : BaseEntity
        {
            var attribute = this.GetLocalizedPropertByKey(languageId, entity.i
           , objAttribute.KeyGroup, objAttribute.Key);

            if (attribute == null)
                _genericAttributeService.Create(objAttribute);
            else
            {
                attribute.Value = languageId.ToString();
                _genericAttributeService.Update(attribute);
            }
        }

        public void SaveLocalized()
        {
          
        }
    }
}