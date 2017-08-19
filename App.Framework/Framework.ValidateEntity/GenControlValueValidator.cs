using App.FakeEntity.GenericControl;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class GenericControlValueValidator : AbstractValidator<GenericControlValueViewModel>
	{
		public GenericControlValueValidator()
		{
			base.RuleFor<string>((GenericControlValueViewModel x) => x.ValueName).NotEmpty<GenericControlValueViewModel, string>().WithMessage<GenericControlValueViewModel, string>("Vui lòng nhập giá trị thuộc tính.");
			base.RuleFor<int>((GenericControlValueViewModel x) => x.GenericControlId).NotEmpty<GenericControlValueViewModel, int>().WithMessage<GenericControlValueViewModel, int>("Vui lòng chọn thuộc tính.");
		}
	}
}