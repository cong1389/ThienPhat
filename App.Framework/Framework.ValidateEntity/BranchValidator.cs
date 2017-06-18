using App.FakeEntity.Brandes;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class BrandValidator : AbstractValidator<BrandViewModel>
	{
		public BrandValidator()
		{
			base.RuleFor<string>((BrandViewModel x) => x.Name).NotEmpty<BrandViewModel, string>().WithMessage<BrandViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<int>((BrandViewModel x) => x.OrderDisplay).NotEmpty<BrandViewModel, int>().WithMessage<BrandViewModel, int>("Vui lòng nhập vị trí hiển thị.");
		}
	}
}