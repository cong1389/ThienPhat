using App.FakeEntity.Other;
using FluentValidation;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace App.Framework.ValidateEntity
{
	public class LandingPageValidator : AbstractValidator<LandingPageViewModel>
	{
		public LandingPageValidator()
		{
			base.RuleFor<string>((LandingPageViewModel x) => x.FullName).NotEmpty<LandingPageViewModel, string>().WithMessage<LandingPageViewModel, string>("Vui lòng nhập họ tên.");
			base.RuleFor<string>((LandingPageViewModel x) => x.PhoneNumber).NotEmpty<LandingPageViewModel, string>().WithMessage<LandingPageViewModel, string>("Vui lòng nhập số điện thoại.");
			base.RuleFor<string>((LandingPageViewModel x) => x.Email).Must<LandingPageViewModel, string>(new Func<string, bool>(LandingPageValidator.IsValidEmail)).WithMessage<LandingPageViewModel, string>("Email không đúng định dạng");
			base.RuleFor<string>((LandingPageViewModel x) => x.DateOfBith).NotEmpty<LandingPageViewModel, string>().WithMessage<LandingPageViewModel, string>("Vui lòng nhập ngày sinh.");
			base.RuleFor<int>((LandingPageViewModel x) => x.ShopId).NotEmpty<LandingPageViewModel, int>().WithMessage<LandingPageViewModel, int>("Vui lòng chọn cửa hàng.");
		}

		public static bool IsValidEmail(string email)
		{
			bool flag;
			flag = ((new Regex("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$")).IsMatch(email) ? true : false);
			return flag;
		}
	}
}