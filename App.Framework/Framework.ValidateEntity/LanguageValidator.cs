using App.FakeEntity.Language;
using FluentValidation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace App.Framework.ValidateEntity
{
	public class LanguageValidator : AbstractValidator<LanguageFormViewModel>
	{
		public LanguageValidator()
		{
			base.RuleFor<string>((LanguageFormViewModel x) => x.LanguageName).NotEmpty<LanguageFormViewModel, string>().WithMessage<LanguageFormViewModel, string>("Vui lòng nhập tên ngôn ngữ.");
			base.RuleFor<string>((LanguageFormViewModel x) => x.LanguageCode).NotEmpty<LanguageFormViewModel, string>().WithMessage<LanguageFormViewModel, string>("Vui lòng nhập mã ngôn ngữ.");
			base.RuleFor<HttpPostedFileBase>((LanguageFormViewModel x) => x.File).Must<LanguageFormViewModel, HttpPostedFileBase>(new Func<HttpPostedFileBase, bool>(LanguageValidator.IsValidFileType)).WithMessage<LanguageFormViewModel, HttpPostedFileBase>("Hình ảnh không đúng định dạng");
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