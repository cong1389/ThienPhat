using App.FakeEntity.Static;
using FluentValidation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class StaticContentValidator : AbstractValidator<StaticContentViewModel>
	{
		public StaticContentValidator()
		{
			base.RuleFor<string>((StaticContentViewModel x) => x.Title).NotEmpty<StaticContentViewModel, string>().WithMessage<StaticContentViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<int>((StaticContentViewModel x) => x.MenuId).NotEmpty<StaticContentViewModel, int>().WithMessage<StaticContentViewModel, int>("Vui lòng chọn danh mục.");
			base.RuleFor<HttpPostedFileBase>((StaticContentViewModel x) => x.Image).Must<StaticContentViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(StaticContentValidator.IsValidFileType)).WithMessage<StaticContentViewModel, HttpPostedFileBase>("Hình ảnh không đúng định dạng");
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