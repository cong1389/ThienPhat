using App.FakeEntity.Post;
using FluentValidation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class PostValidator : AbstractValidator<PostViewModel>
	{
		public PostValidator()
		{
			base.RuleFor<string>((PostViewModel x) => x.Title).NotEmpty<PostViewModel, string>().WithMessage<PostViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<int?>((PostViewModel x) => x.MenuId).NotEmpty<PostViewModel, int?>().WithMessage<PostViewModel, int?>("Vui lòng chọn danh mục.");
			base.RuleFor<string>((PostViewModel x) => x.ProductCode).NotEmpty<PostViewModel, string>().WithMessage<PostViewModel, string>("Vui lòng nhập mã sản phẩm.");
			base.RuleFor<HttpPostedFileBase>((PostViewModel x) => x.Image).Must<PostViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(PostValidator.IsValidFileType)).WithMessage<PostViewModel, HttpPostedFileBase>("Ảnh không đúng định dạng.");
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