using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Account;
using App.Infra.Data.Repository.Ads;
using App.Infra.Data.Repository.Assessments;
using App.Infra.Data.Repository.Attribute;
using App.Infra.Data.Repository.ContactInformation;
using App.Infra.Data.Repository.Gallery;
using App.Infra.Data.Repository.Language;
using App.Infra.Data.Repository.Locations;
using App.Infra.Data.Repository.MailSetting;
using App.Infra.Data.Repository.Menu;
using App.Infra.Data.Repository.News;
using App.Infra.Data.Repository.Other;
using App.Infra.Data.Repository.Post;
using App.Infra.Data.Repository.SeoSetting;
using App.Infra.Data.Repository.Slide;
using App.Infra.Data.Repository.Static;
using App.Infra.Data.Repository.Step;
using App.Infra.Data.Repository.System;
using App.Infra.Data.RepositoryAsync.Post;
using App.Infra.Data.Repository.Brandes;
using Autofac;
using Autofac.Builder;
using System;
using App.Infra.Data.Repository.Order;

namespace App.Framework.Ioc
{
	public class RepositoryModule : Module
	{
		public RepositoryModule()
		{
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterGeneric(typeof(RepositoryBase<>)).As(new Type[] { typeof(IRepositoryBase<>) });
			builder.RegisterGeneric(typeof(RepositoryBaseAsync<>)).As(new Type[] { typeof(IRepositoryBaseAsync<>) });
			builder.RegisterType<LanguageRepository>().As<ILanguageRepository>().InstancePerRequest<LanguageRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<MailSettingRepository>().As<IMailSettingRepository>().InstancePerRequest<MailSettingRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<ContactInfoRepository>().As<IContactInfoRepository>().InstancePerRequest<ContactInfoRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<SystemSettingRepository>().As<ISystemSettingRepository>().InstancePerRequest<SystemSettingRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<MenuLinkRepository>().As<IMenuLinkRepository>().InstancePerRequest<MenuLinkRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<ProvinceRepository>().As<IProvinceRepository>().InstancePerRequest<ProvinceRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<DistrictRepository>().As<IDistrictRepository>().InstancePerRequest<DistrictRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<PostRepository>().As<IPostRepository>().InstancePerRequest<PostRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<NewsRepository>().As<INewsRepository>().InstancePerRequest<NewsRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<StaticContentRepository>().As<IStaticContentRepository>().InstancePerRequest<StaticContentRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<SettingSeoGlobalRepository>().As<ISettingSeoGlobalRepository>().InstancePerRequest<SettingSeoGlobalRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<PageBannerRepository>().As<IPageBannerRepository>().InstancePerRequest<PageBannerRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<LandingPageRepository>().As<ILandingPageRepository>().InstancePerRequest<LandingPageRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<BannerRepository>().As<IBannerRepository>().InstancePerRequest<BannerRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<GalleryRepository>().As<IGalleryRepository>().InstancePerRequest<GalleryRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<AttributeRepository>().As<IAttributeRepository>().InstancePerRequest<AttributeRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<AttributeValueRepository>().As<IAttributeValueRepository>().InstancePerRequest<AttributeValueRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<SlideShowRepository>().As<ISlideShowRepository>().InstancePerRequest<SlideShowRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<FlowStepRepository>().As<IFlowStepRepository>().InstancePerRequest<FlowStepRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<PostRepositoryAsync>().As<IPostRepositoryAsync>().InstancePerRequest<PostRepositoryAsync, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest<UserRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerRequest<RoleRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
			builder.RegisterType<ExternalLoginRepository>().As<IExternalLoginRepository>().InstancePerRequest<ExternalLoginRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);

            builder.RegisterType<AssessmentRepository>().As<IAssessmentRepository>().InstancePerRequest<AssessmentRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<BrandRepository>().As<IBrandRepository>().InstancePerRequest<BrandRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerRequest<OrderRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<OrderGalleryRepository>().As<IOrderGalleryRepository>().InstancePerRequest<OrderGalleryRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<OrderItemRepository>().As<IOrderItemRepository>().InstancePerRequest<OrderItemRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
        }
	}
}