using App.FakeEntity.User;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class RegisterValidation : AbstractValidator<RegisterFormViewModel>
	{
		public RegisterValidation()
		{
			base.RuleFor<string>((RegisterFormViewModel x) => x.FirstName).NotEmpty<RegisterFormViewModel, string>().WithMessage<RegisterFormViewModel, string>("Vui lòng nhập tên.").Length<RegisterFormViewModel>(1, 50).WithMessage<RegisterFormViewModel, string>("Tên phải từ 1 đến 50 ký tự.");
			base.RuleFor<string>((RegisterFormViewModel x) => x.UserName).NotEmpty<RegisterFormViewModel, string>().WithMessage<RegisterFormViewModel, string>("Vui lòng nhập tên tài khoản").Length<RegisterFormViewModel>(4, 50).WithMessage<RegisterFormViewModel, string>("Tên tài khoản phải từ 4 đến 50 ký tự.");
			base.RuleFor<string>((RegisterFormViewModel x) => x.Password).NotEmpty<RegisterFormViewModel, string>().WithMessage<RegisterFormViewModel, string>("Vui lòng nhập mật khẩu tài khoản.");
			base.RuleFor<string>((RegisterFormViewModel x) => x.Password).Length<RegisterFormViewModel>(6, 32).WithMessage<RegisterFormViewModel, string>("Mật khẩu phải có ít nhất từ 6 ký tự trở lên.");
			base.RuleFor<string>((RegisterFormViewModel x) => x.ConfirmPassword).NotEmpty<RegisterFormViewModel, string>().WithMessage<RegisterFormViewModel, string>("Vui lòng xác nhận mật khẩu.").Equal<RegisterFormViewModel, string>((RegisterFormViewModel x) => x.Password, null).WithMessage<RegisterFormViewModel, string>("Mật khẩu xác nhận không chính xác");
			base.RuleFor<string>((RegisterFormViewModel x) => x.Email).NotEmpty<RegisterFormViewModel, string>().WithMessage<RegisterFormViewModel, string>("Vui lòng nhập email tài khoản.");
			base.RuleFor<string>((RegisterFormViewModel x) => x.Email).EmailAddress<RegisterFormViewModel>().WithMessage<RegisterFormViewModel, string>("EMail không đúng định dạng");
			base.RuleFor<string>((RegisterFormViewModel x) => x.Phone).Length<RegisterFormViewModel>(10, 12).WithMessage<RegisterFormViewModel, string>("Số điện thoại phải từ 10-12 ký tự");
		}
	}
}