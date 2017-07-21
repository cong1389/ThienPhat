using App.Service.GenericAttribute;
using App.Service.LocalizedProperty;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace App.Service.Language
{
    public static class LocalizationExtentions
    {
        static LocalizationExtentions()
        {

        }

        public static string GetLocalized<T>(this T entity, Expression<Func<T, string>> keySelector)
        {
            return GetLocalized(entity, keySelector, 1, 1);
        }

        /// <summary>
        /// Get localized property of an entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="ensureTwoPublishedLanguages">A value indicating whether to ensure that we have at least two published languages; otherwise, load only default value</param>
        /// <returns>Localized property</returns>
        public static string GetLocalized<T>(this T entity,
            Expression<Func<T, string>> keySelector, int entityId, int languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
        {
            return GetLocalized<T, string>(entity, keySelector, entityId, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        public static string GetLocalized<T, TPropType>(this T entity,
           Expression<Func<T, TPropType>> keySelector,
            int entityId,
           int languageId,
           bool returnDefaultValue = true,
           bool ensureTwoPublishedLanguages = true)

        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }
            string result = null;


            // load localized value
            string localeKeyGroup = typeof(T).Name.Replace("ViewModel", "");
            string localeKey = propInfo.Name;

            if (languageId > 0)
            {
                var _localizedPropertyService = DependencyResolver.Current.GetService<ILocalizedPropertyService>();
                App.Domain.Entities.Language.LocalizedProperty localizedProperty = _localizedPropertyService.GetLocalizedPropertByKey(languageId
                    , entityId, localeKeyGroup, localeKey);

                result = localizedProperty != null ? localizedProperty.LocaleValue : null;
            }

            return result;
        }

        /// <summary>
        /// GetLocalizedByLocaleKey
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">Class name</param>
        /// <param name="fallBackValue">Value mặc định nếu không có localized</param>
        /// <param name="entityId">Id của thực thể cần set localized</param>
        /// <param name="languageId">Id ngôn ngữ</param>
        /// <param name="localeKeyGroup">Tên nhóm</param>
        /// <param name="localeKey">Tên key</param>
        /// <returns></returns>
        public static string GetLocalizedByLocaleKey<T>(this T entity,string fallBackValue, int entityId,int languageId, string localeKeyGroup, string localeKey)
        {
            string result = null;
           // string localeKeyGroup = typeof(T).Name.Replace("ViewModel", "");
            if (languageId > 0)
            {
                var _localizedPropertyService = DependencyResolver.Current.GetService<ILocalizedPropertyService>();
                App.Domain.Entities.Language.LocalizedProperty localizedProperty = _localizedPropertyService.GetLocalizedPropertByKey(languageId
                    , entityId, localeKeyGroup, localeKey);

                result = localizedProperty != null ? localizedProperty.LocaleValue : fallBackValue;
            }

            return result;
        }

    }
}
