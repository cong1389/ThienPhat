using App.FakeEntity.Slide;
using FluentValidation;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class SlideShowValidator : AbstractValidator<SlideShowViewModel>
	{
		public SlideShowValidator()
		{
			base.RuleFor<string>((SlideShowViewModel x) => x.Title).NotEmpty<SlideShowViewModel, string>().WithMessage<SlideShowViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<int>((SlideShowViewModel x) => x.OrderDisplay).NotEmpty<SlideShowViewModel, int>().WithMessage<SlideShowViewModel, int>("Vui lòng nhập xắp xếp vị trí.");
			base.RuleFor<HttpPostedFileBase>((SlideShowViewModel x) => x.Image).Must<SlideShowViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(SlideShowValidator.IsValidFileType)).WithMessage<SlideShowViewModel, HttpPostedFileBase>("Tệp tin không đúng định dạng.");
		}

		public static bool IsValidFileType(HttpPostedFileBase file)
		{
			bool flag;
			if ((file == null ? true : file.ContentLength <= 0))
			{
				flag = true;
			}
			else
			{
				flag = ((new string[] { ".jpg", ".png", ".gif", ".jpeg", ".mp4" }).Contains<string>(Path.GetExtension(file.FileName)) ? true : false);
			}
			return flag;
		}
	}
}