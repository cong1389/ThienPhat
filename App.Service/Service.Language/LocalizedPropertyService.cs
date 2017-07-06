using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Language;
using App.Infra.Data.UOW.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

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

        public App.Domain.Entities.Language.LocalizedProperty GetLocalizedPropertByKey(int languageId, int entityId, string localeKeyGroup, string localeKey)
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
            int languageId) where T : AuditableEntity<int>
        {
            SaveLocalizedValueItem<T, string>(entity, keySelector, localeValue, languageId);
        }

        public virtual void SaveLocalizedValueItem<T, TPropType>(
           T entity,
           Expression<Func<T, TPropType>> keySelector,
           string localeValue,
           int languageId) where T : AuditableEntity<int>
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");
            }

            var keyGroup = typeof(T).Name;
            var key = propInfo.Name;

            App.Domain.Entities.Language.LocalizedProperty obj = this.GetLocalizedPropertByKey(languageId, entity.Id
           , keyGroup, key);

            if (obj == null)
            {
                if (!string.IsNullOrEmpty(localeValue))
                {
                    obj = new App.Domain.Entities.Language.LocalizedProperty
                    {
                        EntityId = entity.Id,
                        LanguageId = languageId,
                        LocaleKey = key,
                        LocaleKeyGroup = keyGroup,
                        LocaleValue = localeValue
                    };
                    this.Create(obj);
                }
            }
            else
            {
                obj.Id = obj.Id;
                obj.EntityId = entity.Id;
                obj.LanguageId = languageId;
                obj.LocaleKey = key;
                obj.LocaleValue = localeValue;
                this.Update(obj);
            }
        }

    }
}