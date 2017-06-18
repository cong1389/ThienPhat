using App.FakeEntity.Attribute;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class AttributeValidator : AbstractValidator<AttributeViewModel>
	{
		public AttributeValidator()
		{
			base.RuleFor<string>((AttributeViewModel x) => x.AttributeName).NotEmpty<AttributeViewModel, string>().WithMessage<AttributeViewModel, string>("Vui lòng nhập tên thuộc tính.");
		}
	}
}