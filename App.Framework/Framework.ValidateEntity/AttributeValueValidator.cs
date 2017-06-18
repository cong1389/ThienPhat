using App.FakeEntity.Attribute;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class AttributeValueValidator : AbstractValidator<AttributeValueViewModel>
	{
		public AttributeValueValidator()
		{
			base.RuleFor<string>((AttributeValueViewModel x) => x.ValueName).NotEmpty<AttributeValueViewModel, string>().WithMessage<AttributeValueViewModel, string>("Vui lòng nhập giá trị thuộc tính.");
			base.RuleFor<int>((AttributeValueViewModel x) => x.AttributeId).NotEmpty<AttributeValueViewModel, int>().WithMessage<AttributeValueViewModel, int>("Vui lòng chọn thuộc tính.");
		}
	}
}