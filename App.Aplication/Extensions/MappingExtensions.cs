using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using App.Service.Language;
using System.Collections.Generic;
using System.Linq;

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

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                //ShortDesc = entity.GetLocalized(entity.ShortDesc, entity.Id, languageId, "StaticContent", "ShortDesc"),
                //Description = entity.GetLocalized(entity.Description, entity.Id, languageId, "StaticContent", "Description"),
                //MetaTitle = entity.GetLocalized(entity.MetaTitle, entity.Id, languageId, "StaticContent", "MetaTitle"),
                //MetaKeywords = entity.GetLocalized(entity.MetaKeywords, entity.Id, languageId, "StaticContent", "MetaKeywords"),
                //MetaDescription = entity.GetLocalized(entity.MetaDescription, entity.Id, languageId, "StaticContent", "MetaDescription")
            };
            return model;
        }

        public static News ToModel(this News entity)
        {
            if (entity == null)
                return null;

            var model = new News
            {
                Id = entity.Id,
                MenuId = entity.MenuId,
                VirtualCategoryId = entity.VirtualCategoryId,
                Language = entity.Language,
                Status = entity.Status,
                SeoUrl = entity.SeoUrl,             
                ImageBigSize = entity.ImageBigSize,
                ImageMediumSize = entity.ImageMediumSize,
                ImageSmallSize = entity.ImageSmallSize,
                CreatedDate = entity.CreatedDate,            

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                ShortDesc = entity.GetLocalized(x => x.ShortDesc, entity.Id),
                Description = entity.GetLocalized(x => x.Description, entity.Id),
                MetaTitle = entity.GetLocalized(x => x.MetaTitle, entity.Id),
                MetaKeywords = entity.GetLocalized(x => x.MetaKeywords, entity.Id),
                MetaDescription = entity.GetLocalized(x => x.MetaDescription, entity.Id)
            };

            return model;
        }

        public static MenuLink ToModel(this MenuLink entity)
        {
            if (entity == null)
                return null;

            var model = new MenuLink
            {
                Id = entity.Id,
                ParentId = entity.ParentId,
                Status = entity.Status,
                TypeMenu = entity.TypeMenu,
                Position = entity.Position,
                MenuName = entity.GetLocalized(x => x.MenuName, entity.Id),
                SeoUrl = entity.SeoUrl,
                OrderDisplay = entity.OrderDisplay,
                ImageUrl = entity.ImageUrl,
                Icon1 = entity.Icon1,
                Icon2 = entity.Icon2,
                CurrentVirtualId = entity.CurrentVirtualId,
                VirtualId = entity.VirtualId,
                TemplateType = entity.TemplateType,
                MetaTitle = entity.GetLocalized(x => x.MetaTitle, entity.Id),
                MetaKeywords = entity.GetLocalized(x => x.MetaKeywords, entity.Id),
                MetaDescription = entity.GetLocalized(x => x.MetaDescription, entity.Id),

                Language = entity.Language,
                SourceLink = entity.SourceLink,
                VirtualSeoUrl = entity.VirtualSeoUrl,
                DisplayOnHomePage = entity.DisplayOnHomePage,
                DisplayOnMenu = entity.DisplayOnMenu,
                DisplayOnSearch = entity.DisplayOnSearch,
            };

            return model;
        }
    }
}
