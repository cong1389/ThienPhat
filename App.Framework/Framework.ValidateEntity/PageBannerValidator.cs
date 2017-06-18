using App.FakeEntity.Ads;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	internal class PageBannerValidator : AbstractValidator<PageBannerViewModel>
	{
		public PageBannerValidator()
		{
			base.RuleFor<string>((PageBannerViewModel x) => x.PageName).NotEmpty<PageBannerViewModel, string>().WithMessage<PageBannerViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<int>((PageBannerViewModel x) => x.OrderDisplay).NotEmpty<PageBannerViewModel, int>().WithMessage<PageBannerViewModel, int>("Vui lòng nhập vị trí hiển thị.");
		}
	}
}