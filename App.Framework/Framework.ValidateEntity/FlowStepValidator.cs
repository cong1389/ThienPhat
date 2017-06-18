using App.FakeEntity.Step;
using FluentValidation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class FlowStepValidator : AbstractValidator<FlowStepViewModel>
	{
		public FlowStepValidator()
		{
			base.RuleFor<string>((FlowStepViewModel x) => x.Title).NotEmpty<FlowStepViewModel, string>().WithMessage<FlowStepViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<HttpPostedFileBase>((FlowStepViewModel x) => x.Image).Must<FlowStepViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(FlowStepValidator.IsValidFileType)).WithMessage<FlowStepViewModel, HttpPostedFileBase>("Hình ảnh không đúng định dạng");
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