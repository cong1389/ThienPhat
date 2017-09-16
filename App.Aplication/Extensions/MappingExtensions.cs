using App.Domain.Entities.Data;
using App.Domain.Entities.GlobalSetting;
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
                ShortDesc = entity.GetLocalized(x => x.ShortDesc, entity.Id),
                Description = entity.GetLocalized(x => x.Description, entity.Id),
                MetaTitle = entity.GetLocalized(x => x.MetaTitle, entity.Id),
                MetaKeywords = entity.GetLocalized(x => x.MetaKeywords, entity.Id),
                MetaDescription = entity.GetLocalized(x => x.MetaDescription, entity.Id)

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

        public static ContactInformation ToModel(this ContactInformation entity)
        {
            if (entity == null)
                return null;

            var model = new ContactInformation
            {
                Id = entity.Id,
                Email = entity.Email,
                Fax = entity.Fax,
                Hotline = entity.Hotline,
                Lag = entity.Lag,
                Language = entity.Language,
                Lat = entity.Lat,
                MobilePhone = entity.MobilePhone,
                NumberOfStore = entity.NumberOfStore,
                OrderDisplay = entity.OrderDisplay,
                ProvinceId = entity.ProvinceId,
                Status = entity.Status,

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                Address = entity.GetLocalized(x => x.Address, entity.Id)
            };
            return model;
        }

        public static SystemSetting ToModel(this SystemSetting entity)
        {
            if (entity == null)
                return null;

            var model = new SystemSetting
            {
                Id = entity.Id,
                Language = entity.Language,
                Status = entity.Status,
                Favicon = entity.Favicon,
                LogoImage = entity.LogoImage,
                MaintanceSite = entity.MaintanceSite,
                Hotline = entity.Hotline,
                Email = entity.Email,
                TimeWork = entity.TimeWork,

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                FooterContent = entity.GetLocalized(x => x.FooterContent, entity.Id),
                MetaTitle = entity.GetLocalized(x => x.MetaTitle, entity.Id),
                MetaDescription = entity.GetLocalized(x => x.MetaDescription, entity.Id),
                MetaKeywords = entity.GetLocalized(x => x.MetaKeywords, entity.Id),
                Description = entity.GetLocalized(x => x.Description, entity.Id)
            };

            return model;
        }
    }

}
