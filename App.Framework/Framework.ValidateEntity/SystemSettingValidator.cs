using App.FakeEntity.System;
using FluentValidation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class SystemSettingValidator : AbstractValidator<SystemSettingViewModel>
	{
		public SystemSettingValidator()
		{
			base.RuleFor<string>((SystemSettingViewModel x) => x.Title).NotEmpty<SystemSettingViewModel, string>().WithMessage<SystemSettingViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<HttpPostedFileBase>((SystemSettingViewModel x) => x.Logo).Must<SystemSettingViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(SystemSettingValidator.IsValidFileType)).WithMessage<SystemSettingViewModel, HttpPostedFileBase>("Hình ảnh Logo không đúng định dạng");
			base.RuleFor<HttpPostedFileBase>((SystemSettingViewModel x) => x.Icon).Must<SystemSettingViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(SystemSettingValidator.IsValidFileType)).WithMessage<SystemSettingViewModel, HttpPostedFileBase>("Hình ảnh Favicon không đúng định dạng");
		}

		public static bool IsValidFileType(HttpPostedFileBase file)
		{
			bool flag;
			if ((file == null ? false : file.ContentLength > 0))
			{
				ImageFormat[] jpeg = new ImageFormat[] { ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif, ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Tiff, ImageFormat.Icon };
				using (Image image = Image.FromStream(file.InputStream))
				{
					if (!jpeg.Contains<ImageFormat>(image.RawFormat))
					{
						flag = false;
						return flag;
					}
				}
			}
			flag = true;
			return flag;
		}
	}
}