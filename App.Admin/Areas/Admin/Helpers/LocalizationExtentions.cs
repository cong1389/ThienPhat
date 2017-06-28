using App.Core.Common;
using App.Domain.Entities.Language;
using App.FakeEntity.Language;
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
   //      /// <summary>
   //     /// Get localized property of an entity
   //     /// </summary>
   //     /// <typeparam name="T">Entity type</typeparam>
   //     /// <param name="entity">Entity</param>
   //     /// <param name="keySelector">Key selector</param>
   //     /// <param name="languageId">Language identifier</param>
   //     /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
   //     /// <param name="ensureTwoPublishedLanguages">A value indicating whether to ensure that we have at least two published languages; otherwise, load only default value</param>
   //     /// <returns>Localized property</returns>
   //     public static string GetLocalized<T>(this T entity, 
   //         Expression<Func<T, string>> keySelector, int languageId, 
   //         bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true) 
   //         where T : BaseEntity
   //     {
			//return GetLocalized<T, string>(entity, keySelector, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
   //     }

   //     public static TPropType GetLocalized<T, TPropType>(this T entity,
   //        Expression<Func<T, TPropType>> keySelector,
   //        int languageId,
   //        bool returnDefaultValue = true,
   //        bool ensureTwoPublishedLanguages = true)
   //        where T : BaseEntity
   //     {
   //         Guard.NotNull(entity, nameof(entity));

   //         var member = keySelector.Body as MemberExpression;
   //         if (member == null)
   //         {
   //             throw new ArgumentException(string.Format(
   //                 "Expression '{0}' refers to a method, not a property.",
   //                 keySelector));
   //         }

   //         var propInfo = member.Member as PropertyInfo;
   //         if (propInfo == null)
   //         {
   //             throw new ArgumentException(string.Format(
   //                    "Expression '{0}' refers to a field, not a property.",
   //                    keySelector));
   //         }

   //         TPropType result = default(TPropType);
   //         string resultStr = string.Empty;

   //         // load localized value
   //         string localeKeyGroup = typeof(T).Name;
   //         string localeKey = propInfo.Name;

   //         if (languageId > 0)
   //         {
   //             // ensure that we have at least two published languages
   //             bool loadLocalizedValue = true;
   //             if (ensureTwoPublishedLanguages)
   //             {
   //                 var lService = EngineContext.Current.Resolve<ILanguageService>();
   //                 var totalPublishedLanguages = lService.GetLanguagesCount(false);
   //                 loadLocalizedValue = totalPublishedLanguages >= 2;
   //             }

   //             // localized value
   //             if (loadLocalizedValue)
   //             {
   //                 var leService = EngineContext.Current.Resolve<ILocalizedEntityService>();
   //                 resultStr = leService.GetLocalizedValue(languageId, entity.Id, localeKeyGroup, localeKey);
   //                 if (!String.IsNullOrEmpty(resultStr))
   //                     result = resultStr.Convert<TPropType>();
   //             }
   //         }

   //         //set default value if required
   //         if (String.IsNullOrEmpty(resultStr) && returnDefaultValue)
   //         {
   //             //var localizer = (Func<T, TPropType>)_compiledExpressions.GetOrAdd(keySelector, exp => exp.Compile()); // --> MEM LEAK
   //             var localizer = keySelector.Compile();
   //             result = localizer(entity);
   //         }

   //         return result;
   //     }

    }
}