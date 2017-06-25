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
	public class LocalizedPropertyValidator : AbstractValidator<LocalizedPropertyViewModel>
	{
		public LocalizedPropertyValidator()
		{
			base.RuleFor<string>((LocalizedPropertyViewModel x) => x.LocaleValue).NotEmpty<LocalizedPropertyViewModel, string>().WithMessage<LocalizedPropertyViewModel, string>("Vui lòng nhập gía trị Localized.");						
		}
		
	}
}