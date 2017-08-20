using App.Core.Common;
using App.Domain.Entities.Ads;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Identity;
using App.Domain.Entities.Language;
using App.Domain.Entities.Location;
using App.Domain.Entities.Menu;
using App.Domain.Entities.Other;
using App.Domain.Entities.Slide;
using App.FakeEntity.Ads;
using App.FakeEntity.Assessments;
using App.FakeEntity.Attribute;
using App.FakeEntity.Brandes;
using App.FakeEntity.ContactInformation;
using App.FakeEntity.Gallery;
using App.FakeEntity.Language;
using App.FakeEntity.Location;
using App.FakeEntity.Meu;
using App.FakeEntity.News;
using App.FakeEntity.Other;
using App.FakeEntity.Post;
using App.FakeEntity.SeoGlobal;
using App.FakeEntity.ServerMail;
using App.FakeEntity.Slide;
using App.FakeEntity.Static;
using App.FakeEntity.Step;
using App.FakeEntity.System;
using App.FakeEntity.User;
using App.FakeEntity.Order;
using App.Utils;
using AutoMapper;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using App.FakeEntity.GenericControl;
using App.Domain.Entities.GenericControl;

namespace App.Framework.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "ViewModelToDomainMappings";
            }
        }

        public ViewModelToDomainMappingProfile()
        {
        }

        protected override void Configure()
        {
            Mapper.CreateMap<RegisterFormViewModel, IdentityUser>().ForAllMembers((IMemberConfigurationExpression<RegisterFormViewModel> opt) => opt.Condition((ResolutionContext srs) => !srs.IsSourceValueNull));

            Mapper.CreateMap<LanguageFormViewModel, Language>()
                .ForMember((Language x) => x.LanguageName, (IMemberConfigurationExpression<LanguageFormViewModel> map)
                => map.MapFrom<string>((LanguageFormViewModel vm) => vm.LanguageName))
            .ForMember((Language x) => (object)x.Id, (IMemberConfigurationExpression<LanguageFormViewModel> map)
            => map.MapFrom<int>((LanguageFormViewModel vm) => vm.Id))
            .ForMember((Language x) => x.LanguageCode, (IMemberConfigurationExpression<LanguageFormViewModel> map)
            => map.MapFrom<string>((LanguageFormViewModel vm) => vm.LanguageCode))
            .ForMember((Language x) => (object)x.Status, (IMemberConfigurationExpression<LanguageFormViewModel> map)
            => map.MapFrom<int>((LanguageFormViewModel vm) => vm.Status))
            .ForMember((Language x) => x.Flag, (IMemberConfigurationExpression<LanguageFormViewModel> map)
                => map.Condition((LanguageFormViewModel source) => !string.IsNullOrEmpty(source.Flag)));

            Mapper.CreateMap<LocalizedPropertyViewModel, LocalizedProperty>()
                 .ForMember((LocalizedProperty x) => x.Id, (IMemberConfigurationExpression<LocalizedPropertyViewModel> map)
               => map.MapFrom<int>((LocalizedPropertyViewModel vm) => vm.Id))
               .ForMember((LocalizedProperty x) => x.EntityId, (IMemberConfigurationExpression<LocalizedPropertyViewModel> map)
               => map.MapFrom<int>((LocalizedPropertyViewModel vm) => vm.EntityId))
           .ForMember((LocalizedProperty x) => (object)x.LanguageId, (IMemberConfigurationExpression<LocalizedPropertyViewModel> map)
           => map.MapFrom<int>((LocalizedPropertyViewModel vm) => vm.LanguageId))
           .ForMember((LocalizedProperty x) => x.LocaleKeyGroup, (IMemberConfigurationExpression<LocalizedPropertyViewModel> map)
           => map.MapFrom<string>((LocalizedPropertyViewModel vm) => vm.LocaleKeyGroup))
           .ForMember((LocalizedProperty x) => (object)x.LocaleKey, (IMemberConfigurationExpression<LocalizedPropertyViewModel> map)
           => map.MapFrom<string>((LocalizedPropertyViewModel vm) => vm.LocaleKey))
           .ForMember((LocalizedProperty x) => (object)x.LocaleValue, (IMemberConfigurationExpression<LocalizedPropertyViewModel> map)
           => map.MapFrom<string>((LocalizedPropertyViewModel vm) => vm.LocaleValue));

            Mapper.CreateMap<ServerMailSettingViewModel, ServerMailSetting>().ForMember((ServerMailSetting x) => x.FromAddress, (IMemberConfigurationExpression<ServerMailSettingViewModel> map) => map.MapFrom<string>((ServerMailSettingViewModel vm) => vm.FromAddress)).ForMember((ServerMailSetting x) => (object)x.Id, (IMemberConfigurationExpression<ServerMailSettingViewModel> map) => map.MapFrom<int>((ServerMailSettingViewModel vm) => vm.Id)).ForMember((ServerMailSetting x) => x.SmtpClient, (IMemberConfigurationExpression<ServerMailSettingViewModel> map) => map.MapFrom<string>((ServerMailSettingViewModel vm) => vm.SmtpClient)).ForMember((ServerMailSetting x) => (object)x.Status, (IMemberConfigurationExpression<ServerMailSettingViewModel> map) => map.MapFrom<int>((ServerMailSettingViewModel vm) => vm.Status)).ForMember((ServerMailSetting x) => x.UserID, (IMemberConfigurationExpression<ServerMailSettingViewModel> map) => map.MapFrom<string>((ServerMailSettingViewModel vm) => vm.UserId)).ForMember((ServerMailSetting x) => x.Password, (IMemberConfigurationExpression<ServerMailSettingViewModel> map) => map.MapFrom<string>((ServerMailSettingViewModel vm) => vm.Password)).ForMember((ServerMailSetting x) => x.SMTPPort, (IMemberConfigurationExpression<ServerMailSettingViewModel> map) => map.MapFrom<string>((ServerMailSettingViewModel vm) => vm.SMTPPort)).ForMember((ServerMailSetting x) => (object)x.EnableSSL, (IMemberConfigurationExpression<ServerMailSettingViewModel> map) => map.MapFrom<bool>((ServerMailSettingViewModel vm) => vm.EnableSSL)).ForMember((ServerMailSetting x) => (object)x.Status, (IMemberConfigurationExpression<ServerMailSettingViewModel> map) => map.MapFrom<int>((ServerMailSettingViewModel vm) => vm.Status));
            Mapper.CreateMap<LandingPageViewModel, LandingPage>().ForMember((LandingPage x) => x.FullName, (IMemberConfigurationExpression<LandingPageViewModel> map) => map.MapFrom<string>((LandingPageViewModel vm) => vm.FullName)).ForMember((LandingPage x) => (object)x.ShopId, (IMemberConfigurationExpression<LandingPageViewModel> map) => map.MapFrom<int>((LandingPageViewModel vm) => vm.ShopId)).ForMember((LandingPage x) => x.DateOfBith, (IMemberConfigurationExpression<LandingPageViewModel> map) => map.MapFrom<string>((LandingPageViewModel vm) => vm.DateOfBith)).ForMember((LandingPage x) => (object)x.Status, (IMemberConfigurationExpression<LandingPageViewModel> map) => map.MapFrom<int>((LandingPageViewModel vm) => vm.Status)).ForMember((LandingPage x) => x.PhoneNumber, (IMemberConfigurationExpression<LandingPageViewModel> map) => map.MapFrom<string>((LandingPageViewModel vm) => vm.PhoneNumber)).ForMember((LandingPage x) => x.Email, (IMemberConfigurationExpression<LandingPageViewModel> map) => map.MapFrom<string>((LandingPageViewModel vm) => vm.Email));
            Mapper.CreateMap<ContactInformationViewModel, ContactInformation>().ForMember((ContactInformation x) => (object)x.Id, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<int>((ContactInformationViewModel vm) => vm.Id)).ForMember((ContactInformation x) => x.Language, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Language)).ForMember((ContactInformation x) => (object)x.Status, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<int>((ContactInformationViewModel vm) => vm.Status)).ForMember((ContactInformation x) => x.Lag, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Lag)).ForMember((ContactInformation x) => x.Lat, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Lat)).ForMember((ContactInformation x) => x.NumberOfStore, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<string>((ContactInformationViewModel vm) => vm.NumberOfStore)).ForMember((ContactInformation x) => (object)x.Type, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<int>((ContactInformationViewModel vm) => vm.Type)).ForMember((ContactInformation x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<int>((ContactInformationViewModel vm) => vm.OrderDisplay)).ForMember((ContactInformation x) => (object)x.Status, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<int>((ContactInformationViewModel vm) => vm.Status)).ForMember((ContactInformation x) => x.Email, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Email)).ForMember((ContactInformation x) => x.Hotline, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Hotline)).ForMember((ContactInformation x) => x.Address, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Address)).ForMember((ContactInformation x) => x.MobilePhone, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<string>((ContactInformationViewModel vm) => vm.MobilePhone)).ForMember((ContactInformation x) => x.Fax, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Fax)).ForMember((ContactInformation x) => x.Province, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.Ignore()).ForMember((ContactInformation x) => (object)x.ProvinceId, (IMemberConfigurationExpression<ContactInformationViewModel> map) => map.MapFrom<int>((ContactInformationViewModel vm) => vm.ProvinceId));
            Mapper.CreateMap<SystemSettingViewModel, SystemSetting>().ForMember((SystemSetting x) => x.Title, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Title)).ForMember((SystemSetting x) => (object)x.Id, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<int>((SystemSettingViewModel vm) => vm.Id)).ForMember((SystemSetting x) => (object)x.Status, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<int>((SystemSettingViewModel vm) => vm.Status)).ForMember((SystemSetting x) => x.MetaTitle, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.MetaTitle)).ForMember((SystemSetting x) => x.MetaDescription, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.MetaDescription)).ForMember((SystemSetting x) => x.MetaKeywords, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.MetaKeywords)).ForMember((SystemSetting x) => x.Description, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Description)).ForMember((SystemSetting x) => x.TimeWork, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.TimeWork)).ForMember((SystemSetting x) => x.Hotline, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Hotline)).ForMember((SystemSetting x) => x.Email, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Email)).ForMember((SystemSetting x) => x.LogoImage, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.LogoImage)).ForMember((SystemSetting x) => x.Favicon, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Favicon)).ForMember((SystemSetting x) => x.Slogan, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Slogan)).ForMember((SystemSetting x) => x.FooterContent, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<string>((SystemSettingViewModel vm) => vm.FooterContent)).ForMember((SystemSetting x) => (object)x.MaintanceSite, (IMemberConfigurationExpression<SystemSettingViewModel> map) => map.MapFrom<bool>((SystemSettingViewModel vm) => vm.MaintanceSite));
            Mapper.CreateMap<MenuLinkViewModel, MenuLink>().ForMember((MenuLink x) => x.MenuName, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<string>((MenuLinkViewModel vm) => vm.MenuName)).ForMember((MenuLink x) => (object)x.Id, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<int>((MenuLinkViewModel vm) => vm.Id)).ForMember((MenuLink x) => (object)x.Status, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<int>((MenuLinkViewModel vm) => vm.Status)).ForMember((MenuLink x) => x.MetaTitle, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<string>((MenuLinkViewModel vm) => vm.MetaTitle)).ForMember((MenuLink x) => x.MetaDescription, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<string>((MenuLinkViewModel vm) => vm.MetaDescription)).ForMember((MenuLink x) => x.MetaKeywords, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<string>((MenuLinkViewModel vm) => vm.MetaKeywords))
                .ForMember((MenuLink x) => (object)x.ParentId, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<int?>((MenuLinkViewModel vm) => vm.ParentId))
                .ForMember((MenuLink x) => x.CurrentVirtualId, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.CurrentVirtualId))).ForMember((MenuLink x) => x.VirtualId, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.VirtualId))).ForMember((MenuLink x) => (object)x.TypeMenu, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<int>((MenuLinkViewModel vm) => vm.TypeMenu)).ForMember((MenuLink x) => (object)x.Position, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<int>((MenuLinkViewModel vm) => vm.Position)).ForMember((MenuLink x) => (object)x.TemplateType, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<int>((MenuLinkViewModel vm) => vm.TemplateType)).ForMember((MenuLink x) => (object)x.DisplayOnMenu, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<bool>((MenuLinkViewModel vm) => vm.DisplayOnMenu)).ForMember((MenuLink x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<int>((MenuLinkViewModel vm) => vm.OrderDisplay)).ForMember((MenuLink x) => x.SourceLink, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<string>((MenuLinkViewModel vm) => vm.SourceLink)).ForMember((MenuLink x) => x.SeoUrl, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<string>((MenuLinkViewModel vm) => vm.SeoUrl)).ForMember((MenuLink x) => x.VirtualSeoUrl, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.VirtualSeoUrl))).ForMember((MenuLink x) => x.Language, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<string>((MenuLinkViewModel vm) => vm.Language)).ForMember((MenuLink x) => x.ImageUrl, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.ImageUrl))).ForMember((MenuLink x) => x.Icon1, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.Icon1))).ForMember((MenuLink x) => x.Icon2, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.Icon2))).ForMember((MenuLink x) => (object)x.DisplayOnHomePage, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.MapFrom<bool>((MenuLinkViewModel vm) => vm.DisplayOnHomePage)).ForMember((MenuLink x) => x.ParentMenu, (IMemberConfigurationExpression<MenuLinkViewModel> map) => map.Ignore());

            Mapper.CreateMap<ProvinceViewModel, Province>().ForMember((Province x) => x.Name, (IMemberConfigurationExpression<ProvinceViewModel> map) => map.MapFrom<string>((ProvinceViewModel vm) => vm.Name)).ForMember((Province x) => (object)x.Id, (IMemberConfigurationExpression<ProvinceViewModel> map) => map.MapFrom<int>((ProvinceViewModel vm) => vm.Id)).ForMember((Province x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<ProvinceViewModel> map) => map.MapFrom<int>((ProvinceViewModel vm) => vm.OrderDisplay)).ForMember((Province x) => (object)x.Status, (IMemberConfigurationExpression<ProvinceViewModel> map) => map.MapFrom<int>((ProvinceViewModel vm) => vm.Status));

            Mapper.CreateMap<AttributeViewModel, App.Domain.Entities.Attribute.Attribute>().ForMember((App.Domain.Entities.Attribute.Attribute x) => x.AttributeName, (IMemberConfigurationExpression<AttributeViewModel> map) => map.MapFrom<string>((AttributeViewModel vm) => vm.AttributeName)).ForMember((App.Domain.Entities.Attribute.Attribute x) => (object)x.Id, (IMemberConfigurationExpression<AttributeViewModel> map) => map.MapFrom<int>((AttributeViewModel vm) => vm.Id)).ForMember((App.Domain.Entities.Attribute.Attribute x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<AttributeViewModel> map) => map.MapFrom<int?>((AttributeViewModel vm) => vm.OrderDisplay)).ForMember((App.Domain.Entities.Attribute.Attribute x) => x.Description, (IMemberConfigurationExpression<AttributeViewModel> map) => map.MapFrom<string>((AttributeViewModel vm) => vm.Description)).ForMember((App.Domain.Entities.Attribute.Attribute x) => (object)x.Status, (IMemberConfigurationExpression<AttributeViewModel> map) => map.MapFrom<int>((AttributeViewModel vm) => vm.Status)).ForMember((App.Domain.Entities.Attribute.Attribute x) => x.AttributeValues, (IMemberConfigurationExpression<AttributeViewModel> map) => map.Ignore());

            Mapper.CreateMap<AttributeValueViewModel, AttributeValue>().ForMember((AttributeValue x) => x.ValueName, (IMemberConfigurationExpression<AttributeValueViewModel> map) => map.MapFrom<string>((AttributeValueViewModel vm) => vm.ValueName)).ForMember((AttributeValue x) => (object)x.Id, (IMemberConfigurationExpression<AttributeValueViewModel> map) => map.MapFrom<int>((AttributeValueViewModel vm) => vm.Id)).ForMember((AttributeValue x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<AttributeValueViewModel> map) => map.MapFrom<int?>((AttributeValueViewModel vm) => vm.OrderDisplay)).ForMember((AttributeValue x) => x.ColorHex, (IMemberConfigurationExpression<AttributeValueViewModel> map) => map.MapFrom<string>((AttributeValueViewModel vm) => vm.ColorHex)).ForMember((AttributeValue x) => x.Description, (IMemberConfigurationExpression<AttributeValueViewModel> map) => map.MapFrom<string>((AttributeValueViewModel vm) => vm.Description)).ForMember((AttributeValue x) => (object)x.AttributeId, (IMemberConfigurationExpression<AttributeValueViewModel> map) => map.MapFrom<int>((AttributeValueViewModel vm) => vm.AttributeId)).ForMember((AttributeValue x) => x.Attribute, (IMemberConfigurationExpression<AttributeValueViewModel> map) => map.Ignore()).ForMember((AttributeValue x) => (object)x.Status, (IMemberConfigurationExpression<AttributeValueViewModel> map) => map.MapFrom<int>((AttributeValueViewModel vm) => vm.Status));

            Mapper.CreateMap<GalleryImageViewModel, GalleryImage>().ForAllMembers((IMemberConfigurationExpression<GalleryImageViewModel> opt) => opt.Condition((ResolutionContext srs) => !srs.IsSourceValueNull));

            Mapper.CreateMap<PostViewModel, Post>()
                .ForMember((Post x) => x.Title, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<string>((PostViewModel vm) => vm.Title))
                .ForMember((Post x) => (object)x.Id, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<int>((PostViewModel vm) => vm.Id))
                .ForMember((Post x) => (object)x.Status, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<int>((PostViewModel vm) => vm.Status))
                .ForMember((Post x) => x.MetaTitle, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<string>((PostViewModel vm) => vm.MetaTitle))
                .ForMember((Post x) => x.MetaDescription, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<string>((PostViewModel vm) => vm.MetaDescription))
                .ForMember((Post x) => x.MetaKeywords, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<string>((PostViewModel vm) => vm.MetaKeywords))
                .ForMember((Post x) => (object)x.MenuId, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<int>((PostViewModel vm) => (int)vm.MenuId))
                .ForMember((Post x) => x.TechInfo, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<string>((PostViewModel vm) => vm.TechInfo))
                .ForMember((Post x) => (object)x.PostType, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<int>((PostViewModel vm) => vm.PostType))
                .ForMember((Post x) => (object)x.OldOrNew, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<bool>((PostViewModel vm) => vm.OldOrNew))
                .ForMember((Post x) => (object)x.Price, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<double?>((PostViewModel vm) => vm.Price))
                .ForMember((Post x) => (object)x.Discount, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<double?>((PostViewModel vm) => vm.Discount))
                .ForMember((Post x) => (object)x.ProductHot, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<bool>((PostViewModel vm) => vm.ProductHot))
                .ForMember((Post x) => (object)x.OutOfStock, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<bool>((PostViewModel vm) => vm.OutOfStock))
                .ForMember((Post x) => x.Description, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<string>((PostViewModel vm) => vm.Description))
                .ForMember((Post x) => x.ProductCode, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<string>((PostViewModel vm) => vm.ProductCode))
                .ForMember((Post x) => (object)x.ProductNew, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<bool>((PostViewModel vm) => vm.ProductNew))
                .ForMember((Post x) => x.ShortDesc, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<string>((PostViewModel vm) => vm.ShortDesc))
                .ForMember((Post x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<int>((PostViewModel vm) => vm.OrderDisplay))
                .ForMember((Post x) => x.SeoUrl, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<string>((PostViewModel vm) => vm.SeoUrl))
                .ForMember((Post x) => x.VirtualCatUrl, (IMemberConfigurationExpression<PostViewModel> map)
                => map.Condition((PostViewModel source) => !string.IsNullOrEmpty(source.VirtualCatUrl)))
                .ForMember((Post x) => x.Language, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<string>((PostViewModel vm) => vm.Language))
                .ForMember((Post x) => x.ImageBigSize, (IMemberConfigurationExpression<PostViewModel> map)
                => map.Condition((PostViewModel source) => !string.IsNullOrEmpty(source.ImageBigSize)))
                .ForMember((Post x) => x.ImageMediumSize, (IMemberConfigurationExpression<PostViewModel> map)
                => map.Condition((PostViewModel source) => !string.IsNullOrEmpty(source.ImageMediumSize)))
                .ForMember((Post x) => x.ImageSmallSize, (IMemberConfigurationExpression<PostViewModel> map)
                => map.Condition((PostViewModel source) => !string.IsNullOrEmpty(source.ImageSmallSize)))
                .ForMember((Post x) => (object)x.StartDate, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<DateTime?>((PostViewModel vm) => vm.StartDate))
                .ForMember((Post x) => (object)x.EndDate, (IMemberConfigurationExpression<PostViewModel> map)
                => map.MapFrom<DateTime?>((PostViewModel vm) => vm.EndDate))
                .ForMember((Post x) => x.AttributeValues, (IMemberConfigurationExpression<PostViewModel> map)
                => map.Condition((PostViewModel source) => source.AttributeValues.IsAny<AttributeValueViewModel>()))
                .ForMember((Post x) => x.GalleryImages, (IMemberConfigurationExpression<PostViewModel> map) => map.Ignore())
                .ForMember((Post x) => x.VirtualCategoryId, (IMemberConfigurationExpression<PostViewModel> map)
                => map.Condition((PostViewModel source) => !string.IsNullOrEmpty(source.VirtualCategoryId)))
                .ForMember((Post x) => x.MenuLink, (IMemberConfigurationExpression<PostViewModel> map) => map.Ignore())
                .ForMember((Post x) => x.MenuLink, (IMemberConfigurationExpression<PostViewModel> map) => map.Ignore());

            Mapper.CreateMap<NewsViewModel, News>().ForMember((News x) => x.Title, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.Title)).ForMember((News x) => (object)x.Id, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<int>((NewsViewModel vm) => vm.Id)).ForMember((News x) => (object)x.Status, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<int>((NewsViewModel vm) => vm.Status)).ForMember((News x) => x.MetaTitle, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.MetaTitle)).ForMember((News x) => x.MetaDescription, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.MetaDescription)).ForMember((News x) => x.MetaKeywords, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.MetaKeywords)).ForMember((News x) => (object)x.MenuId, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<int>((NewsViewModel vm) => vm.MenuId)).ForMember((News x) => x.Description, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.Description)).ForMember((News x) => x.ShortDesc, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.ShortDesc)).ForMember((News x) => (object)x.Video, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<bool>((NewsViewModel vm) => vm.Video)).ForMember((News x) => x.VideoLink, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.VideoLink)).ForMember((News x) => x.OtherLink, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.OtherLink)).ForMember((News x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.OrderDisplay)).ForMember((News x) => (object)x.SpecialDisplay, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<bool>((NewsViewModel vm) => vm.SpecialDisplay)).ForMember((News x) => (object)x.HomeDisplay, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<bool>((NewsViewModel vm) => vm.HomeDisplay)).ForMember((News x) => x.SeoUrl, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.SeoUrl)).ForMember((News x) => x.VirtualCatUrl, (IMemberConfigurationExpression<NewsViewModel> map) => map.Condition((NewsViewModel source) => !string.IsNullOrEmpty(source.VirtualCatUrl))).ForMember((News x) => x.Language, (IMemberConfigurationExpression<NewsViewModel> map) => map.MapFrom<string>((NewsViewModel vm) => vm.Language)).ForMember((News x) => x.ImageBigSize, (IMemberConfigurationExpression<NewsViewModel> map) => map.Condition((NewsViewModel source) => !string.IsNullOrEmpty(source.ImageBigSize))).ForMember((News x) => x.ImageMediumSize, (IMemberConfigurationExpression<NewsViewModel> map) => map.Condition((NewsViewModel source) => !string.IsNullOrEmpty(source.ImageMediumSize))).ForMember((News x) => x.ImageSmallSize, (IMemberConfigurationExpression<NewsViewModel> map) => map.Condition((NewsViewModel source) => !string.IsNullOrEmpty(source.ImageSmallSize))).ForMember((News x) => x.VirtualCategoryId, (IMemberConfigurationExpression<NewsViewModel> map) => map.Condition((NewsViewModel source) => !string.IsNullOrEmpty(source.VirtualCategoryId))).ForMember((News x) => x.MenuLink, (IMemberConfigurationExpression<NewsViewModel> map) => map.Ignore());

            Mapper.CreateMap<FlowStepViewModel, FlowStep>().ForMember((FlowStep x) => x.Title, (IMemberConfigurationExpression<FlowStepViewModel> map) => map.MapFrom<string>((FlowStepViewModel vm) => vm.Title)).ForMember((FlowStep x) => (object)x.Id, (IMemberConfigurationExpression<FlowStepViewModel> map) => map.MapFrom<int>((FlowStepViewModel vm) => vm.Id)).ForMember((FlowStep x) => (object)x.Status, (IMemberConfigurationExpression<FlowStepViewModel> map) => map.MapFrom<int>((FlowStepViewModel vm) => vm.Status)).ForMember((FlowStep x) => x.Description, (IMemberConfigurationExpression<FlowStepViewModel> map) => map.MapFrom<string>((FlowStepViewModel vm) => vm.Description)).ForMember((FlowStep x) => x.OtherLink, (IMemberConfigurationExpression<FlowStepViewModel> map) => map.MapFrom<string>((FlowStepViewModel vm) => vm.OtherLink)).ForMember((FlowStep x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<FlowStepViewModel> map) => map.MapFrom<int>((FlowStepViewModel vm) => vm.OrderDisplay)).ForMember((FlowStep x) => x.ImageUrl, (IMemberConfigurationExpression<FlowStepViewModel> map) => map.Condition((FlowStepViewModel source) => !string.IsNullOrEmpty(source.ImageUrl)));

            Mapper.CreateMap<StaticContentViewModel, StaticContent>().
                ForMember((StaticContent x) => x.Title,
                (IMemberConfigurationExpression<StaticContentViewModel> map)
                => map.MapFrom<string>((StaticContentViewModel vm)
                => vm.Title)).ForMember((StaticContent x)
                => (object)x.Id, (IMemberConfigurationExpression<StaticContentViewModel> map)
                => map.MapFrom<int>((StaticContentViewModel vm)
                => vm.Id)).ForMember((StaticContent x) => (object)x.Status, (IMemberConfigurationExpression<StaticContentViewModel> map)
                => map.MapFrom<int>((StaticContentViewModel vm) => vm.Status)).ForMember((StaticContent x)
                => x.MetaTitle, (IMemberConfigurationExpression<StaticContentViewModel> map) => map.MapFrom<string>((StaticContentViewModel vm)
                => vm.MetaTitle)).ForMember((StaticContent x) => x.MetaDescription, (IMemberConfigurationExpression<StaticContentViewModel> map)
                => map.MapFrom<string>((StaticContentViewModel vm) => vm.MetaDescription)).ForMember((StaticContent x)
                => x.MetaKeywords, (IMemberConfigurationExpression<StaticContentViewModel> map)
                => map.MapFrom<string>((StaticContentViewModel vm) => vm.MetaKeywords)).ForMember((StaticContent x)
                => (object)x.MenuId, (IMemberConfigurationExpression<StaticContentViewModel> map) => map.MapFrom<int>((StaticContentViewModel vm) => vm.MenuId)).ForMember((StaticContent x) => x.Description, (IMemberConfigurationExpression<StaticContentViewModel> map) => map.MapFrom<string>((StaticContentViewModel vm) => vm.Description)).ForMember((StaticContent x) => x.ShortDesc, (IMemberConfigurationExpression<StaticContentViewModel> map) => map.MapFrom<string>((StaticContentViewModel vm) => vm.ShortDesc)).ForMember((StaticContent x) => x.SeoUrl, (IMemberConfigurationExpression<StaticContentViewModel> map) => map.MapFrom<string>((StaticContentViewModel vm) => vm.SeoUrl)).ForMember((StaticContent x) => x.Language, (IMemberConfigurationExpression<StaticContentViewModel> map) => map.MapFrom<string>((StaticContentViewModel vm) => vm.Language)).ForMember((StaticContent x) => x.ImagePath, (IMemberConfigurationExpression<StaticContentViewModel> map) => map.Condition((StaticContentViewModel source) => !string.IsNullOrEmpty(source.ImagePath))).ForMember((StaticContent x) => (object)x.ViewCount, (IMemberConfigurationExpression<StaticContentViewModel> map) => map.MapFrom<int>((StaticContentViewModel vm) => vm.ViewCount)).ForMember((StaticContent x) => x.MenuLink, (IMemberConfigurationExpression<StaticContentViewModel> map) => map.Ignore());


            Mapper.CreateMap<SettingSeoGlobalViewModel, SettingSeoGlobal>().ForMember((SettingSeoGlobal x) => x.FbAppId, (IMemberConfigurationExpression<SettingSeoGlobalViewModel> map) => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.FbAppId)).ForMember((SettingSeoGlobal x) => (object)x.Id, (IMemberConfigurationExpression<SettingSeoGlobalViewModel> map) => map.MapFrom<int>((SettingSeoGlobalViewModel vm) => vm.Id)).ForMember((SettingSeoGlobal x) => x.FbAdminsId, (IMemberConfigurationExpression<SettingSeoGlobalViewModel> map) => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.FbAdminsId)).ForMember((SettingSeoGlobal x) => x.SnippetGoogleAnalytics, (IMemberConfigurationExpression<SettingSeoGlobalViewModel> map) => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.SnippetGoogleAnalytics)).ForMember((SettingSeoGlobal x) => x.MetaTagMasterTool, (IMemberConfigurationExpression<SettingSeoGlobalViewModel> map) => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.MetaTagMasterTool)).ForMember((SettingSeoGlobal x) => x.PublisherGooglePlus, (IMemberConfigurationExpression<SettingSeoGlobalViewModel> map) => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.PublisherGooglePlus)).ForMember((SettingSeoGlobal x) => x.FacebookRetargetSnippet, (IMemberConfigurationExpression<SettingSeoGlobalViewModel> map) => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.FacebookRetargetSnippet)).ForMember((SettingSeoGlobal x) => x.GoogleRetargetSnippet, (IMemberConfigurationExpression<SettingSeoGlobalViewModel> map) => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.GoogleRetargetSnippet)).ForMember((SettingSeoGlobal x) => (object)x.Status, (IMemberConfigurationExpression<SettingSeoGlobalViewModel> map) => map.MapFrom<int>((SettingSeoGlobalViewModel vm) => vm.Status));
            Mapper.CreateMap<PageBannerViewModel, PageBanner>().ForMember((PageBanner x) => x.PageName, (IMemberConfigurationExpression<PageBannerViewModel> map) => map.MapFrom<string>((PageBannerViewModel vm) => vm.PageName)).ForMember((PageBanner x) => (object)x.Id, (IMemberConfigurationExpression<PageBannerViewModel> map) => map.MapFrom<int>((PageBannerViewModel vm) => vm.Id)).ForMember((PageBanner x) => x.Language, (IMemberConfigurationExpression<PageBannerViewModel> map) => map.MapFrom<string>((PageBannerViewModel vm) => vm.Language)).ForMember((PageBanner x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<PageBannerViewModel> map) => map.MapFrom<int>((PageBannerViewModel vm) => vm.OrderDisplay)).ForMember((PageBanner x) => (object)x.Position, (IMemberConfigurationExpression<PageBannerViewModel> map) => map.MapFrom<int>((PageBannerViewModel vm) => vm.Position)).ForMember((PageBanner x) => (object)x.Status, (IMemberConfigurationExpression<PageBannerViewModel> map) => map.MapFrom<int>((PageBannerViewModel vm) => vm.Status));

            Mapper.CreateMap<BannerViewModel, Banner>()
                .ForMember((Banner x) => x.Title, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<string>((BannerViewModel vm) => vm.Title))
                .ForMember((Banner x) => (object)x.Id, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<int>((BannerViewModel vm) => vm.Id))
                .ForMember((Banner x) => (object)x.Status, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<int>((BannerViewModel vm) => vm.Status))
                .ForMember((Banner x) => x.WebsiteLink, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<string>((BannerViewModel vm) => vm.WebsiteLink))
                .ForMember((Banner x) => x.ImgPath, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.Condition((BannerViewModel source) => !string.IsNullOrEmpty(source.ImgPath)))
                .ForMember((Banner x) => x.Width, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<string>((BannerViewModel vm) => vm.Width))
                .ForMember((Banner x) => x.Language, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<string>((BannerViewModel vm) => vm.Language))
                .ForMember((Banner x) => x.Height, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<string>((BannerViewModel vm) => vm.Height))
                .ForMember((Banner x) => x.Target, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<string>((BannerViewModel vm) => vm.Target))
                .ForMember((Banner x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<int>((BannerViewModel vm) => vm.OrderDisplay))
                .ForMember((Banner x) => (object)x.FromDate, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<TimeSpan?>((BannerViewModel vm) => vm.FromDate))
                .ForMember((Banner x) => (object)x.ToDate, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<TimeSpan?>((BannerViewModel vm) => vm.ToDate))
                .ForMember((Banner x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<int>((BannerViewModel vm) => vm.OrderDisplay))
                .ForMember((Banner x) => (object)x.PageId, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<int>((BannerViewModel vm) => vm.PageId))
                .ForMember((Banner x) => (object)x.MenuId, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<int?>((BannerViewModel vm) => vm.MenuId))
                .ForMember((Banner x) => x.Language, (IMemberConfigurationExpression<BannerViewModel> map)
                => map.MapFrom<string>((BannerViewModel vm) => vm.Language))
                .ForMember((Banner x) => x.PageBanner, (IMemberConfigurationExpression<BannerViewModel> map) => map.Ignore())
                .ForMember((Banner x) => x.MenuLink, (IMemberConfigurationExpression<BannerViewModel> map) => map.Ignore());

            Mapper.CreateMap<SlideShowViewModel, SlideShow>().ForMember((SlideShow x) => x.Title, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<string>((SlideShowViewModel vm) => vm.Title)).ForMember((SlideShow x) => (object)x.Id, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<int>((SlideShowViewModel vm) => vm.Id)).ForMember((SlideShow x) => (object)x.Status, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<int>((SlideShowViewModel vm) => vm.Status)).ForMember((SlideShow x) => x.WebsiteLink, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<string>((SlideShowViewModel vm) => vm.WebsiteLink)).ForMember((SlideShow x) => x.ImgPath, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.Condition((SlideShowViewModel source) => !string.IsNullOrEmpty(source.ImgPath))).ForMember((SlideShow x) => x.Width, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<string>((SlideShowViewModel vm) => vm.Width)).ForMember((SlideShow x) => x.Description, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<string>((SlideShowViewModel vm) => vm.Description)).ForMember((SlideShow x) => (object)x.Video, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<bool>((SlideShowViewModel vm) => vm.Video)).ForMember((SlideShow x) => x.Height, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<string>((SlideShowViewModel vm) => vm.Height)).ForMember((SlideShow x) => x.Target, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<string>((SlideShowViewModel vm) => vm.Target)).ForMember((SlideShow x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<int>((SlideShowViewModel vm) => vm.OrderDisplay)).ForMember((SlideShow x) => (object)x.FromDate, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<TimeSpan?>((SlideShowViewModel vm) => vm.FromDate)).ForMember((SlideShow x) => (object)x.ToDate, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<TimeSpan?>((SlideShowViewModel vm) => vm.ToDate)).ForMember((SlideShow x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<SlideShowViewModel> map) => map.MapFrom<int>((SlideShowViewModel vm) => vm.OrderDisplay));

            Mapper.CreateMap<AssessmentViewModel, Assessment>()
                .ForMember((Assessment x) => x.BillNumber, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.BillNumber))
                .ForMember((Assessment x) => x.CusomterNumber, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.CusomterNumber))
                .ForMember((Assessment x) => x.Address, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.Address))
                .ForMember((Assessment x) => x.FullName, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.FullName))
                .ForMember((Assessment x) => x.Password, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.Password))
                .ForMember((Assessment x) => x.IdentityCard, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.IdentityCard))
                .ForMember((Assessment x) => x.Warranty, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<int>((AssessmentViewModel vm) => vm.Warranty))
                .ForMember((Assessment x) => (DateTime?)x.FromWarranty, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<DateTime?>((AssessmentViewModel vm) => vm.FromWarranty))
                .ForMember((Assessment x) => (DateTime?)x.ToWarranty, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<DateTime?>((AssessmentViewModel vm) => vm.ToWarranty))
                .ForMember((Assessment x) => x.BrandId, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<int>((AssessmentViewModel vm) => vm.BrandId))
                .ForMember((Assessment x) => x.Branch, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.Branch))
                .ForMember((Assessment x) => x.Model, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.Model))
                .ForMember((Assessment x) => x.Imei, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.Imei))
                .ForMember((Assessment x) => x.PhoneNumber, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.PhoneNumber))
                .ForMember((Assessment x) => x.AppleId, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.AppleId))
                .ForMember((Assessment x) => x.ICloudPassword, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.ICloudPassword))
                .ForMember((Assessment x) => x.StatusCurrent, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.StatusCurrent))
                .ForMember((Assessment x) => x.RepairTypeId, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<int>((AssessmentViewModel vm) => vm.RepairTypeId))
                .ForMember((Assessment x) => x.RepairStatus, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.RepairStatus))
                .ForMember((Assessment x) => (object)x.Id, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<int>((AssessmentViewModel vm) => vm.Id))
                .ForMember((Assessment x) => x.Status, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<int>((AssessmentViewModel vm) => vm.Status))
                .ForMember((Assessment x) => x.StoreId, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<int>((AssessmentViewModel vm) => vm.StoreId))
                .ForMember((Assessment x) => x.Accessories, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.Accessories))
                .ForMember((Assessment x) => x.Description, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.Description))
                .ForMember((Assessment x) => x.OtherLink, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<string>((AssessmentViewModel vm) => vm.OtherLink))
                .ForMember((Assessment x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.MapFrom<int>((AssessmentViewModel vm) => vm.OrderDisplay))
                .ForMember((Assessment x) => x.ImageUrl, (IMemberConfigurationExpression<AssessmentViewModel> map) => map.Condition((AssessmentViewModel source) => !string.IsNullOrEmpty(source.ImageUrl))
                );

            Mapper.CreateMap<BrandViewModel, Brand>()
                .ForMember((Brand x) => x.Name, (IMemberConfigurationExpression<BrandViewModel> map) => map.MapFrom<string>((BrandViewModel vm) => vm.Name))
                .ForMember((Brand x) => (object)x.Id, (IMemberConfigurationExpression<BrandViewModel> map) => map.MapFrom<int>((BrandViewModel vm) => vm.Id))
                .ForMember((Brand x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<BrandViewModel> map) => map.MapFrom<int>((BrandViewModel vm) => vm.OrderDisplay))
                .ForMember((Brand x) => (object)x.Status, (IMemberConfigurationExpression<BrandViewModel> map) => map.MapFrom<int>((BrandViewModel vm) => vm.Status));

            Mapper.CreateMap<OrderViewModel, Order>().ForMember((Order x) => x.Model, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.Model)).ForMember((Order x) => (object)x.Id, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<int>((OrderViewModel vm) => vm.Id)).ForMember((Order x) => (object)x.Status, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<int>((OrderViewModel vm) => vm.Status)).ForMember((Order x) => x.ModelBrand, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.ModelBrand)).ForMember((Order x) => x.SerialNumber, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.SerialNumber)).ForMember((Order x) => (object)x.BrandId, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<int>((OrderViewModel vm) => vm.BrandId)).ForMember((Order x) => x.OrderCode, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.OrderCode)).ForMember((Order x) => x.CustomerCode, (IMemberConfigurationExpression<OrderViewModel> map) => map.Condition((OrderViewModel source) => !string.IsNullOrEmpty(source.CustomerCode))).ForMember((Order x) => x.StoreName, (IMemberConfigurationExpression<OrderViewModel> map) => map.Condition((OrderViewModel source) => !string.IsNullOrEmpty(source.StoreName))).ForMember((Order x) => x.CustomerName, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.CustomerName)).ForMember((Order x) => x.PhoneNumber, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.PhoneNumber)).ForMember((Order x) => x.CustomerIdNumber, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.CustomerIdNumber)).ForMember((Order x) => x.Address, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.Address)).ForMember((Order x) => x.Accessories, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.Accessories)).ForMember((Order x) => x.PasswordPhone, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.PasswordPhone)).ForMember((Order x) => x.AppleId, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.AppleId)).ForMember((Order x) => x.IcloudPassword, (IMemberConfigurationExpression<OrderViewModel> map) => map.Condition((OrderViewModel source) => !string.IsNullOrEmpty(source.IcloudPassword))).ForMember((Order x) => (object)x.OldWarranty, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel source) => source.OldWarranty)).ForMember((Order x) => x.PhoneStatus, (IMemberConfigurationExpression<OrderViewModel> map) => map.Condition((OrderViewModel source) => !string.IsNullOrEmpty(source.PhoneStatus))).ForMember((Order x) => x.Note, (IMemberConfigurationExpression<OrderViewModel> map) => map.MapFrom<string>((OrderViewModel vm) => vm.Note)).ForMember((Order x) => x.OrderGalleries, (IMemberConfigurationExpression<OrderViewModel> map) => map.Ignore()).ForMember((Order x) => x.OrderItems, (IMemberConfigurationExpression<OrderViewModel> map) => map.Ignore()).ForMember((Order x) => x.Brand, (IMemberConfigurationExpression<OrderViewModel> map) => map.Ignore());

            Mapper.CreateMap<OrderGalleryViewModel, OrderGallery>().ForAllMembers((IMemberConfigurationExpression<OrderGalleryViewModel> opt) => opt.Condition((ResolutionContext srs) => !srs.IsSourceValueNull));

            Mapper.CreateMap<OrderItemViewModel, OrderItem>().ForMember((OrderItem x) => (object)x.Id, (IMemberConfigurationExpression<OrderItemViewModel> map) => map.MapFrom<int>((OrderItemViewModel vm) => vm.Id)).ForMember((OrderItem x) => (object)x.OrderId, (IMemberConfigurationExpression<OrderItemViewModel> map) => map.MapFrom<int>((OrderItemViewModel vm) => vm.OrderId)).ForMember((OrderItem x) => (object)x.FixedFee, (IMemberConfigurationExpression<OrderItemViewModel> map) => map.MapFrom<decimal?>((OrderItemViewModel vm) => vm.FixedFee)).ForMember((OrderItem x) => (object)x.WarrantyFrom, (IMemberConfigurationExpression<OrderItemViewModel> map) => map.MapFrom<DateTime?>((OrderItemViewModel vm) => vm.WarrantyFrom)).ForMember((OrderItem x) => x.Order, (IMemberConfigurationExpression<OrderItemViewModel> map) => map.Ignore()).ForMember((OrderItem x) => (object)x.WarrantyTo, (IMemberConfigurationExpression<OrderItemViewModel> map) => map.MapFrom<DateTime?>((OrderItemViewModel vm) => vm.WarrantyTo));

            Mapper.CreateMap<LocaleStringResourceViewModel, LocaleStringResource>()
              .ForMember((LocaleStringResource x) => x.LanguageId, (IMemberConfigurationExpression<LocaleStringResourceViewModel> map) => map.MapFrom<int>((LocaleStringResourceViewModel vm) => vm.LanguageId))
              .ForMember((LocaleStringResource x) => x.ResourceName, (IMemberConfigurationExpression<LocaleStringResourceViewModel> map) => map.MapFrom<string>((LocaleStringResourceViewModel vm) => vm.ResourceName))
              .ForMember((LocaleStringResource x) => x.ResourceValue, (IMemberConfigurationExpression<LocaleStringResourceViewModel> map) => map.MapFrom<string>((LocaleStringResourceViewModel vm) => vm.ResourceValue))
              .ForMember((LocaleStringResource x) => x.IsFromPlugin, (IMemberConfigurationExpression<LocaleStringResourceViewModel> map) => map.MapFrom<bool>((LocaleStringResourceViewModel vm) => vm.IsFromPlugin))
              .ForMember((LocaleStringResource x) => x.IsTouched, (IMemberConfigurationExpression<LocaleStringResourceViewModel> map) => map.MapFrom<bool>((LocaleStringResourceViewModel vm) => vm.IsTouched));

            Mapper.CreateMap<GenericControlViewModel, GenericControl>()
                .ForMember((GenericControl x)
                => x.Name, (IMemberConfigurationExpression<GenericControlViewModel> map)
                => map.MapFrom<string>((GenericControlViewModel vm) => vm.Name))
                .ForMember((App.Domain.Entities.GenericControl.GenericControl x)
                => (object)x.Id, (IMemberConfigurationExpression<GenericControlViewModel> map)
                => map.MapFrom<int>((GenericControlViewModel vm) => vm.Id))
                .ForMember((App.Domain.Entities.GenericControl.GenericControl x) => (object)x.OrderDisplay
                , (IMemberConfigurationExpression<GenericControlViewModel> map) => map.MapFrom<int?>((GenericControlViewModel vm) => vm.OrderDisplay))
                .ForMember((App.Domain.Entities.GenericControl.GenericControl x) => x.Description
                , (IMemberConfigurationExpression<GenericControlViewModel> map) => map.MapFrom<string>((GenericControlViewModel vm) => vm.Description))
                .ForMember((App.Domain.Entities.GenericControl.GenericControl x) => (object)x.Status, (IMemberConfigurationExpression<GenericControlViewModel> map)
                => map.MapFrom<int>((GenericControlViewModel vm) => vm.Status))
                .ForMember((App.Domain.Entities.GenericControl.GenericControl x) => (object)x.EntityId, (IMemberConfigurationExpression<GenericControlViewModel> map)
                => map.MapFrom<int>((GenericControlViewModel vm) => vm.EntityId))
                .ForMember((App.Domain.Entities.GenericControl.GenericControl x) => (object)x.ControlTypeId, (IMemberConfigurationExpression<GenericControlViewModel> map)
                => map.MapFrom<int?>((GenericControlViewModel vm) => vm.ControlTypeId))
                .ForMember((App.Domain.Entities.GenericControl.GenericControl x)
                => x.GenericControlValues, (IMemberConfigurationExpression<GenericControlViewModel> map) => map.Ignore());

            Mapper.CreateMap<GenericControlValueViewModel, GenericControlValue>()
                .ForMember((GenericControlValue x) => x.ValueName, (IMemberConfigurationExpression<GenericControlValueViewModel> map)
                => map.MapFrom<string>((GenericControlValueViewModel vm) => vm.ValueName))
                .ForMember((GenericControlValue x) => (object)x.Id, (IMemberConfigurationExpression<GenericControlValueViewModel> map)
                => map.MapFrom<int>((GenericControlValueViewModel vm) => vm.Id))
                .ForMember((GenericControlValue x) => (object)x.OrderDisplay, (IMemberConfigurationExpression<GenericControlValueViewModel> map)
                => map.MapFrom<int?>((GenericControlValueViewModel vm) => vm.OrderDisplay))
                .ForMember((GenericControlValue x) => x.ColorHex, (IMemberConfigurationExpression<GenericControlValueViewModel> map)
                => map.MapFrom<string>((GenericControlValueViewModel vm) => vm.ColorHex))
                .ForMember((GenericControlValue x) => x.Description, (IMemberConfigurationExpression<GenericControlValueViewModel> map)
                => map.MapFrom<string>((GenericControlValueViewModel vm) => vm.Description))
                .ForMember((GenericControlValue x) => (object)x.GenericControlId, (IMemberConfigurationExpression<GenericControlValueViewModel> map)
                => map.MapFrom<int>((GenericControlValueViewModel vm) => vm.GenericControlId))
                .ForMember((GenericControlValue x) => x.GenericControl, (IMemberConfigurationExpression<GenericControlValueViewModel> map)
                => map.Ignore()).ForMember((GenericControlValue x) => (object)x.Status, (IMemberConfigurationExpression<GenericControlValueViewModel> map)
                => map.MapFrom<int>((GenericControlValueViewModel vm) => vm.Status));


        }
    }
}