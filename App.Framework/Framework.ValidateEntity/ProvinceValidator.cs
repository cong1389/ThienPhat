using App.FakeEntity.Location;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class ProvinceValidator : AbstractValidator<ProvinceViewModel>
	{
		public ProvinceValidator()
		{
			base.RuleFor<string>((ProvinceViewModel x) => x.Name).NotEmpty<ProvinceViewModel, string>().WithMessage<ProvinceViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<int>((ProvinceViewModel x) => x.OrderDisplay).NotEmpty<ProvinceViewModel, int>().WithMessage<ProvinceViewModel, int>("Vui lòng nhập vị trí hiển thị.");
		}
	}
}