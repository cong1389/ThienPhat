using App.FakeEntity.ContactInformation;
using FluentValidation;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace App.Framework.ValidateEntity
{
	public class ContactInformationValidator : AbstractValidator<ContactInformationViewModel>
	{
		public ContactInformationValidator()
		{
			base.RuleFor<string>((ContactInformationViewModel x) => x.Title).NotEmpty<ContactInformationViewModel, string>().WithMessage<ContactInformationViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<string>((ContactInformationViewModel x) => x.Address).NotEmpty<ContactInformationViewModel, string>().WithMessage<ContactInformationViewModel, string>("Vui lòng nhập địa chỉ.");
			base.RuleFor<string>((ContactInformationViewModel x) => x.Email).Must<ContactInformationViewModel, string>(new Func<string, bool>(ContactInformationValidator.IsValidEmail)).WithMessage<ContactInformationViewModel, string>("Email không đúng định dạng");
			base.RuleFor<int>((ContactInformationViewModel x) => x.OrderDisplay).NotEmpty<ContactInformationViewModel, int>().WithMessage<ContactInformationViewModel, int>("Vui lòng nhập vị trí hiển thị.");
			base.RuleFor<int>((ContactInformationViewModel x) => x.OrderDisplay).GreaterThanOrEqualTo<ContactInformationViewModel, int>(0).WithMessage<ContactInformationViewModel, int>("Vị trí hiển thị phải là số và lớn hơn 0.");
		}

		public static bool IsValidEmail(string email)
		{
			bool flag;
			flag = ((new Regex("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$")).IsMatch(email) ? true : false);
			return flag;
		}
	}
}