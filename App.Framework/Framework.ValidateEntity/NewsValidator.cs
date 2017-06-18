using App.FakeEntity.News;
using FluentValidation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class NewsValidator : AbstractValidator<NewsViewModel>
	{
		public NewsValidator()
		{
			base.RuleFor<string>((NewsViewModel x) => x.Title).NotEmpty<NewsViewModel, string>().WithMessage<NewsViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<int>((NewsViewModel x) => x.MenuId).NotEmpty<NewsViewModel, int>().WithMessage<NewsViewModel, int>("Vui lòng chọn danh mục.");
			base.RuleFor<HttpPostedFileBase>((NewsViewModel x) => x.Image).Must<NewsViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(NewsValidator.IsValidFileType)).WithMessage<NewsViewModel, HttpPostedFileBase>("Hình ảnh không đúng định dạng");
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