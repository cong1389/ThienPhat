using App.FakeEntity.Ads;
using App.FakeEntity.Attribute;
using App.FakeEntity.ContactInformation;
using App.FakeEntity.Language;
using App.FakeEntity.Location;
using App.FakeEntity.Meu;
using App.FakeEntity.News;
using App.FakeEntity.Other;
using App.FakeEntity.Post;
using App.FakeEntity.ServerMail;
using App.FakeEntity.Slide;
using App.FakeEntity.Static;
using App.FakeEntity.Step;
using App.FakeEntity.System;
using App.FakeEntity.User;
using App.FakeEntity.Assessments;
using App.Framework.ValidateEntity;
using FluentValidation;
using System;
using System.Collections.Generic;
using App.FakeEntity.Brandes;
using App.FakeEntity.Order;

namespace App.Framework.FluentValidation
{
	public class FluentValidationConfig : ValidatorFactoryBase
	{
		private readonly Dictionary<Type, IValidator> _validators;

		public FluentValidationConfig()
		{
			this._validators = new Dictionary<Type, IValidator>();
			this.AddBinding();
		}

		public void AddBinding()
		{
			this._validators.Add(typeof(IValidator<LanguageFormViewModel>), new LanguageValidator());
			this._validators.Add(typeof(IValidator<ServerMailSettingViewModel>), new MailSettingValidator());
			this._validators.Add(typeof(IValidator<ContactInformationViewModel>), new ContactInformationValidator());
			this._validators.Add(typeof(IValidator<SystemSettingViewModel>), new SystemSettingValidator());
			this._validators.Add(typeof(IValidator<MenuLinkViewModel>), new MenuLinkValidator());
			this._validators.Add(typeof(IValidator<ProvinceViewModel>), new ProvinceValidator());
			this._validators.Add(typeof(IValidator<DistrictViewModel>), new DistrictValidator());
			this._validators.Add(typeof(IValidator<PostViewModel>), new PostValidator());
			this._validators.Add(typeof(IValidator<NewsViewModel>), new NewsValidator());
			this._validators.Add(typeof(IValidator<StaticContentViewModel>), new StaticContentValidator());
			this._validators.Add(typeof(IValidator<BannerViewModel>), new BannerValidator());
			this._validators.Add(typeof(IValidator<LoginViewModel>), new LoginValidator());
			this._validators.Add(typeof(IValidator<ChangePasswordViewModel>), new ChangePasswordValidator());
			this._validators.Add(typeof(IValidator<SlideShowViewModel>), new SlideShowValidator());
			this._validators.Add(typeof(IValidator<AttributeViewModel>), new AttributeValidator());
			this._validators.Add(typeof(IValidator<AttributeValueViewModel>), new AttributeValueValidator());
			this._validators.Add(typeof(IValidator<LandingPageViewModel>), new LandingPageValidator());
			this._validators.Add(typeof(IValidator<FlowStepViewModel>), new FlowStepValidator());

            this._validators.Add(typeof(IValidator<AssessmentViewModel>), new AssessmentValidator());
            this._validators.Add(typeof(IValidator<BrandViewModel>), new BrandValidator());
            this._validators.Add(typeof(IValidator<OrderViewModel>), new OrderValidator());
        }

		public override IValidator CreateInstance(Type validatorType)
		{
			IValidator validator;
			IValidator validator1;
			validator1 = (!this._validators.TryGetValue(validatorType, out validator) ? validator : validator);
			return validator1;
		}
	}
}