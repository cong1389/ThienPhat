using App.FakeEntity.Meu;
using FluentValidation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class MenuLinkValidator : AbstractValidator<MenuLinkViewModel>
	{
		public MenuLinkValidator()
		{
			base.RuleFor<string>((MenuLinkViewModel x) => x.MenuName).NotEmpty<MenuLinkViewModel, string>().WithMessage<MenuLinkViewModel, string>("Vui lòng nhập tiêu đề.");
			base.RuleFor<string>((MenuLinkViewModel x) => x.MetaKeywords).NotEmpty<MenuLinkViewModel, string>().WithMessage<MenuLinkViewModel, string>("Vui lòng nhập từ khoá.");
			base.RuleFor<HttpPostedFileBase>((MenuLinkViewModel x) => x.Image).Must<MenuLinkViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(MenuLinkValidator.IsValidFileType)).WithMessage<MenuLinkViewModel, HttpPostedFileBase>("Ảnh không đúng định dạng.");
			base.RuleFor<HttpPostedFileBase>((MenuLinkViewModel x) => x.ImageIcon1).Must<MenuLinkViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(MenuLinkValidator.IsValidFileType)).WithMessage<MenuLinkViewModel, HttpPostedFileBase>("Icon1 không đúng định dạng.");
			base.RuleFor<HttpPostedFileBase>((MenuLinkViewModel x) => x.ImageIcon2).Must<MenuLinkViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(MenuLinkValidator.IsValidFileType)).WithMessage<MenuLinkViewModel, HttpPostedFileBase>("Icon2 không đúng định dạng.");
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