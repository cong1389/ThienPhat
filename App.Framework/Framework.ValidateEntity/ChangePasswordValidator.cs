using App.FakeEntity.User;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class ChangePasswordValidator : AbstractValidator<ChangePasswordViewModel>
	{
		public ChangePasswordValidator()
		{
			base.RuleFor<string>((ChangePasswordViewModel x) => x.OldPassword).NotEmpty<ChangePasswordViewModel, string>().WithMessage<ChangePasswordViewModel, string>("Vui lòng nhập mật khẩu cũ.");
			base.RuleFor<string>((ChangePasswordViewModel x) => x.NewPassword).NotEmpty<ChangePasswordViewModel, string>().WithMessage<ChangePasswordViewModel, string>("Vui lòng nhập mật khẩu mới.");
			base.RuleFor<string>((ChangePasswordViewModel x) => x.ConfirmPassword).NotEmpty<ChangePasswordViewModel, string>().WithMessage<ChangePasswordViewModel, string>("Vui lòng nhập xác nhận mật khẩu.");
		}
	}
}