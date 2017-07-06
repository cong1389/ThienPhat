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
        private static readonly ILocalizedPropertyService _localizedPropertyService;

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

    }
}
