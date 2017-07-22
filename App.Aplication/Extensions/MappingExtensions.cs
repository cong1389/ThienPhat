using App.Domain.Entities.Data;
using App.Service.Language;

namespace App.Extensions
{
    public static class MappingExtensions
    {
        public static StaticContent ToModel(this StaticContent entity)
        {
            if (entity == null)
                return null;
            var model = new StaticContent
            {
                Id = entity.Id,
                MenuId = entity.MenuId,
                VirtualCategoryId = entity.VirtualCategoryId,
                Language = entity.Language,
                Status = entity.Status,
                SeoUrl = entity.SeoUrl,
                ImagePath = entity.ImagePath,

                Title = entity.GetLocalized(x =>x.Title),
                //ShortDesc = entity.GetLocalizedByLocaleKey(entity.ShortDesc, entity.Id, languageId, "StaticContent", "ShortDesc"),
                //Description = entity.GetLocalizedByLocaleKey(entity.Description, entity.Id, languageId, "StaticContent", "Description"),
                //MetaTitle = entity.GetLocalizedByLocaleKey(entity.MetaTitle, entity.Id, languageId, "StaticContent", "MetaTitle"),
                //MetaKeywords = entity.GetLocalizedByLocaleKey(entity.MetaKeywords, entity.Id, languageId, "StaticContent", "MetaKeywords"),
                //MetaDescription = entity.GetLocalizedByLocaleKey(entity.MetaDescription, entity.Id, languageId, "StaticContent", "MetaDescription")
            };
            return model;
        }
    }
}
