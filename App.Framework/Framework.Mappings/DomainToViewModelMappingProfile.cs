using App.Domain.Entities.Ads;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Identity;
using App.Domain.Entities.Language;
using App.Domain.Entities.Location;
using App.Domain.Entities.Menu;
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
using App.FakeEntity.Order;
using App.FakeEntity.Post;
using App.FakeEntity.SeoGlobal;
using App.FakeEntity.ServerMail;
using App.FakeEntity.Slide;
using App.FakeEntity.Static;
using App.FakeEntity.Step;
using App.FakeEntity.System;
using App.FakeEntity.User;
using AutoMapper;
using System;

namespace App.Framework.Mappings
{
	public class DomainToViewModelMappingProfile : Profile
	{
		public override string ProfileName
		{
			get
			{
				return "DomainToViewModelMappings";
			}
		}

		public DomainToViewModelMappingProfile()
		{
		}

		protected override void Configure()
		{
			Mapper.CreateMap<Language, LanguageFormViewModel>();
			Mapper.CreateMap<ServerMailSetting, ServerMailSettingViewModel>();
			Mapper.CreateMap<ContactInformation, ContactInformationViewModel>();
			Mapper.CreateMap<SystemSetting, SystemSettingViewModel>();
			Mapper.CreateMap<MenuLink, MenuLinkViewModel>();
			Mapper.CreateMap<Province, ProvinceViewModel>();
			Mapper.CreateMap<District, DistrictViewModel>();
			Mapper.CreateMap<Post, PostViewModel>();
			Mapper.CreateMap<News, NewsViewModel>();
			Mapper.CreateMap<SettingSeoGlobal, SettingSeoGlobalViewModel>();
			Mapper.CreateMap<PageBanner, PageBannerViewModel>();
			Mapper.CreateMap<Banner, BannerViewModel>();
			Mapper.CreateMap<StaticContent, StaticContentViewModel>();
			Mapper.CreateMap<GalleryImage, GalleryImageViewModel>();
			Mapper.CreateMap<FlowStep, FlowStepViewModel>();
			Mapper.CreateMap<IdentityUser, RegisterFormViewModel>();
			Mapper.CreateMap<AttributeValue, AttributeValueViewModel>();
			Mapper.CreateMap<Domain.Entities.Attribute.Attribute, AttributeViewModel>();
			Mapper.CreateMap<SlideShow, SlideShowViewModel>();
            Mapper.CreateMap<Assessment, AssessmentViewModel>();
            Mapper.CreateMap<Brand, BrandViewModel>();
            Mapper.CreateMap<Order, OrderViewModel>();
            Mapper.CreateMap<OrderGallery, OrderGalleryViewModel>();
            Mapper.CreateMap<OrderItem, OrderItemViewModel>();

            Mapper.CreateMap<LocalizedProperty, LocalizedPropertyViewModel>();
            Mapper.CreateMap<LocaleStringResource, LocaleStringResourceViewModel>();
        }
	}
}