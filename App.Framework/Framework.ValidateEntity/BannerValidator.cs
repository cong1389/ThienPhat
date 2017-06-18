using App.FakeEntity.Ads;
using FluentValidation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class BannerValidator : AbstractValidator<BannerViewModel>
	{
		public BannerValidator()
		{
			base.RuleFor<string>((BannerViewModel x) => x.Title).NotEmpty<BannerViewModel, string>().WithMessage<BannerViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<int>((BannerViewModel x) => x.OrderDisplay).NotEmpty<BannerViewModel, int>().WithMessage<BannerViewModel, int>("Vui lòng nhập xắp xếp vị trí.");
			base.RuleFor<int>((BannerViewModel x) => x.PageId).NotEmpty<BannerViewModel, int>().WithMessage<BannerViewModel, int>("Vui lòng chọn vị trí banner hiển thị");
			base.RuleFor<HttpPostedFileBase>((BannerViewModel x) => x.Image).Must<BannerViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(BannerValidator.IsValidFileType)).WithMessage<BannerViewModel, HttpPostedFileBase>("Ảnh không đúng định dạng.");
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