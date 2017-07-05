using App.Core.Common;
using App.Domain.Entities.Language;
using App.FakeEntity.Language;
using App.Service.GenericAttribute;
using App.Service.LocalizedProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace App.Admin.Areas.Admin.Helpers
{
    public static class LocalizationExtentions
    {
        private static readonly IGenericAttributeService _genericAttributeService;

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
            Expression<Func<T, string>> keySelector, int languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where T : BaseEntity
        {
            return GetLocalized<T, string>(entity, keySelector, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        public static string GetLocalized<T, TPropType>(this T entity,
           Expression<Func<T, TPropType>> keySelector,
           int languageId,
           bool returnDefaultValue = true,
           bool ensureTwoPublishedLanguages = true)
           where T : BaseEntity
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
            string result=null;
            

            // load localized value
            string localeKeyGroup = typeof(T).Name;
            string localeKey = propInfo.Name;

            if (languageId > 0)
            {
                App.Domain.Entities.Data.GenericAttribute attribute = _genericAttributeService.GetGenericAttributeByKey(1, "Customer", "LanguageId");

                result = attribute.Value;
            }
                        
            return result;
        }

    }
}