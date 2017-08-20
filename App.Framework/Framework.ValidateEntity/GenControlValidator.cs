using App.FakeEntity.GenericControl;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class GenericControlValidator : AbstractValidator<GenericControlViewModel>
	{
		public GenericControlValidator()
		{
			base.RuleFor<string>((GenericControlViewModel x) => x.Name).NotEmpty<GenericControlViewModel, string>().WithMessage<GenericControlViewModel, string>("Vui lòng nhập tên thuộc tính.");
		}
	}
}