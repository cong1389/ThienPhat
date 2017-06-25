using App.AsyncService.Post;
using App.Domain.Interfaces.Services;
using App.ImagePlugin;
using App.Infra.Data.Common;
using App.SeoSitemap;
using App.Service.Account;
using App.Service.Ads;
using App.Service.Assessments;
using App.Service.Attribute;
using App.Service.ContactInformation;
using App.Service.Gallery;
using App.Service.Language;
using App.Service.Locations;
using App.Service.MailSetting;
using App.Service.Menu;
using App.Service.News;
using App.Service.Other;
using App.Service.Post;
using App.Service.SeoSetting;
using App.Service.Slide;
using App.Service.Static;
using App.Service.Step;
using App.Service.SystemApp;
using App.Service.Brandes;
using Autofac;
using Autofac.Builder;
using System;
using App.Service.Order;
using App.Service.LocalizedProperty;

namespace App.Framework.Ioc
{
	public class ServiceModule : Module
	{
		public ServiceModule()
		{
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterGeneric(typeof(BaseService<>)).As(new Type[] { typeof(IBaseService<>) });
			builder.RegisterGeneric(typeof(BaseAsyncService<>)).As(new Type[] { typeof(IBaseAsyncService<>) });
			builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerRequest<LanguageService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<MailSettingService>().As<IMailSettingService>().InstancePerRequest<MailSettingService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<ContactInfoService>().As<IContactInfoService>().InstancePerRequest<ContactInfoService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<SystemSettingService>().As<ISystemSettingService>().InstancePerRequest<SystemSettingService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<MenuLinkService>().As<IMenuLinkService>().InstancePerRequest<MenuLinkService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<ProvinceService>().As<IProvinceService>().InstancePerRequest<ProvinceService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<DistrictService>().As<IDistrictService>().InstancePerRequest<DistrictService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<PostService>().As<IPostService>().InstancePerRequest<PostService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<NewsService>().As<INewsService>().InstancePerRequest<NewsService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<StaticContentService>().As<IStaticContentService>().InstancePerRequest<StaticContentService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<SettingSeoGlobalService>().As<ISettingSeoGlobalService>().InstancePerRequest<SettingSeoGlobalService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<PageBannerService>().As<IPageBannerService>().InstancePerRequest<PageBannerService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<BannerService>().As<IBannerService>().InstancePerRequest<BannerService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<GalleryService>().As<IGalleryService>().InstancePerRequest<GalleryService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<AttributeService>().As<IAttributeService>().InstancePerRequest<AttributeService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<AttributeValueService>().As<IAttributeValueService>().InstancePerRequest<AttributeValueService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<SlideShowService>().As<ISlideShowService>().InstancePerRequest<SlideShowService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<LandingPageService>().As<ILandingPageService>().InstancePerRequest<LandingPageService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<FlowStepService>().As<IFlowStepService>().InstancePerRequest<FlowStepService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<SitemapProvider>().As<ISitemapProvider>().InstancePerRequest<SitemapProvider, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<PostAsynService>().As<IPostAsynService>().InstancePerRequest<PostAsynService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest<UserService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<RoleService>().As<IRoleService>().InstancePerRequest<RoleService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<App.ImagePlugin.ImagePlugin>().As<IImagePlugin>().InstancePerRequest<App.ImagePlugin.ImagePlugin, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);

            builder.RegisterType<AssessmentService>().As<IAssessmentService>().InstancePerRequest<AssessmentService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<BrandService>().As<IBrandService>().InstancePerRequest<BrandService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerRequest<OrderService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<OrderGalleryService>().As<IOrderGalleryService>().InstancePerRequest<OrderGalleryService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<OrderItemService>().As<IOrderItemService>().InstancePerRequest<OrderItemService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<LocalizedPropertyService>().As<ILocalizedPropertyService>().InstancePerRequest<LocalizedPropertyService, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
        }
	}
}