using App.FakeEntity.ServerMail;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
	public class MailSettingValidator : AbstractValidator<ServerMailSettingViewModel>
	{
		public MailSettingValidator()
		{
			base.RuleFor<string>((ServerMailSettingViewModel x) => x.FromAddress).NotEmpty<ServerMailSettingViewModel, string>().WithMessage<ServerMailSettingViewModel, string>("Vui lòng nhập địa chỉ email.");
			base.RuleFor<string>((ServerMailSettingViewModel x) => x.UserId).NotEmpty<ServerMailSettingViewModel, string>().WithMessage<ServerMailSettingViewModel, string>("Vui lòng nhập email id.");
			base.RuleFor<string>((ServerMailSettingViewModel x) => x.Password).NotEmpty<ServerMailSettingViewModel, string>().WithMessage<ServerMailSettingViewModel, string>("Vui lòng nhập mật khẩu email id.");
			base.RuleFor<string>((ServerMailSettingViewModel x) => x.SMTPPort).NotEmpty<ServerMailSettingViewModel, string>().WithMessage<ServerMailSettingViewModel, string>("Vui lòng nhập port server.");
			base.RuleFor<string>((ServerMailSettingViewModel x) => x.SmtpClient).NotEmpty<ServerMailSettingViewModel, string>().WithMessage<ServerMailSettingViewModel, string>("Vui lòng nhập địa chỉ stmtp server.");
		}
	}
}