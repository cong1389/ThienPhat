using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.Service.LocalizedProperty
{
    public interface ILocalizedPropertyService : IBaseService<App.Domain.Entities.Language.LocalizedProperty>, IService
    {
        void CreateLocalizedProperty(App.Domain.Entities.Language.LocalizedProperty LocalizedProperty);

        App.Domain.Entities.Language.LocalizedProperty GetLocalizedPropertyById(int Id);

        IEnumerable<App.Domain.Entities.Language.LocalizedProperty> GetLocalizedPropertyByEntityId(int entityId);

        App.Domain.Entities.Language.LocalizedProperty GetLocalizedPropertByKey(int languageId,int entityId, string localeKeyGroup, string localeKey);

        IEnumerable<App.Domain.Entities.Language.LocalizedProperty> PagedList(SortingPagingBuilder sortBuider, Paging page);

        int SaveLocalizedProperty();

        void SaveLocalizedValue<T>(
            T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            int languageId) where T : BaseEntity;

        void SaveLocalizedValueItem<T, TPropType>(
           T entity,
           Expression<Func<T, TPropType>> keySelector,
           TPropType localeValue,
           int languageId) where T : BaseEntity;

        int SaveLocalized();

    }
}