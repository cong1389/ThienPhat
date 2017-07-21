using App.FakeEntity.Language;
using FluentValidation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class LocaleStringResourceValidator : AbstractValidator<LocaleStringResourceViewModel>
	{
		public LocaleStringResourceValidator()
		{
			base.RuleFor<string>((LocaleStringResourceViewModel x) => x.ResourceName)
                .NotEmpty<LocaleStringResourceViewModel, string>()
                .WithMessage<LocaleStringResourceViewModel, string>("Vui lòng nhập tên resource.");						
		}
		
	}
}