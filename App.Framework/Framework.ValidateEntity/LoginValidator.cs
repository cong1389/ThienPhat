using App.FakeEntity.User;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class LoginValidator : AbstractValidator<LoginViewModel>
	{
		public LoginValidator()
		{
			base.RuleFor<string>((LoginViewModel x) => x.UserName).NotEmpty<LoginViewModel, string>().WithMessage<LoginViewModel, string>("Vui lòng nhập tên đăng nhập.");
			base.RuleFor<string>((LoginViewModel x) => x.Password).NotEmpty<LoginViewModel, string>().WithMessage<LoginViewModel, string>("Vui lòng nhập mật khẩu.");
		}
	}
}