using App.FakeEntity.Location;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class DistrictValidator : AbstractValidator<DistrictViewModel>
	{
		public DistrictValidator()
		{
			base.RuleFor<string>((DistrictViewModel x) => x.Name).NotEmpty<DistrictViewModel, string>().WithMessage<DistrictViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<int>((DistrictViewModel x) => x.OrderDisplay).NotEmpty<DistrictViewModel, int>().WithMessage<DistrictViewModel, int>("Vui lòng nhập vị trí hiển thị.");
		}
	}
}