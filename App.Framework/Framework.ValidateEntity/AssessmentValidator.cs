using App.FakeEntity.Assessments;
using FluentValidation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class AssessmentValidator : AbstractValidator<AssessmentViewModel>
	{
		public AssessmentValidator()
		{
			base.RuleFor<string>((AssessmentViewModel x) => x.FullName).NotEmpty<AssessmentViewModel, string>().WithMessage<AssessmentViewModel, string>("Vui lòng nhập họ tên.");            
            base.RuleFor<string>((AssessmentViewModel x) => x.PhoneNumber).NotEmpty<AssessmentViewModel, string>().WithMessage<AssessmentViewModel, string>("Vui lòng nhập số điện thoại.");
            base.RuleFor<HttpPostedFileBase>((AssessmentViewModel x) => x.Image).Must<AssessmentViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(AssessmentValidator.IsValidFileType)).WithMessage<AssessmentViewModel, HttpPostedFileBase>("Hình ảnh không đúng định dạng");
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
				ImageFormat[] jpeg = new ImageFormat[] { ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif, ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Tiff };
				using (Image image = Image.FromStream(file.InputStream))
				{
					if (!jpeg.Contains<ImageFormat>(image.RawFormat))
					{
						flag = false;
						return flag;
					}
				}
				flag = true;
			}
			return flag;
		}
	}
}