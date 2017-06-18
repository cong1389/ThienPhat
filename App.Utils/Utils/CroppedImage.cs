using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace App.Utils
{
	public static class CroppedImage
	{
		public static string SaveCroppedImage(HttpPostedFileBase imageFile, string filePath, string fileName, int? width = null, int? height = null)
		{
			string str;
			Image image = Image.FromStream(imageFile.InputStream);
			Guid guid = ImageFormat.Jpeg.Guid;
			string lower = Path.GetExtension(imageFile.FileName).ToLower();
			if (lower.Equals(".jpeg") || lower.Equals(".jpg"))
			{
				guid = ImageFormat.Jpeg.Guid;
			}
			if (lower.Equals(".png"))
			{
				guid = ImageFormat.Png.Guid;
			}
			if (lower.Equals(".gif"))
			{
				guid = ImageFormat.Gif.Guid;
			}
			if (lower.Equals(".bmp"))
			{
				guid = ImageFormat.Bmp.Guid;
			}
			if (lower.Equals(".tiff"))
			{
				guid = ImageFormat.Tiff.Guid;
			}
			ImageCodecInfo imageCodecInfo = ImageCodecInfo.GetImageEncoders().First<ImageCodecInfo>((ImageCodecInfo codecInfo) => codecInfo.FormatID == guid);
			Image image1 = image;
			Bitmap bitmap = null;
			try
			{
				int num = 0;
				int num1 = 0;
				int value = 1;
				int value1 = 1;
				value1 = height.Value;
				value = width.Value;
				bitmap = new Bitmap(value, value1);
				double num2 = (double)value1 / (double)value;
				double num3 = (double)value / (double)value1;
				if (image.Width <= image.Height)
				{
					value1 = (int)Math.Round((double)image.Width * num2);
					if (value1 >= image.Height)
					{
						value = (int)Math.Round((double)image.Width * ((double)image.Height / (double)value1));
						value1 = image.Height;
						num = (image.Width - value) / 2;
					}
					else
					{
						value = image.Width;
						num1 = (image.Height - value1) / 2;
					}
				}
				else
				{
					value = (int)Math.Round((double)image.Height * num3);
					if (value >= image.Width)
					{
						value1 = (int)Math.Round((double)image.Height * ((double)image.Width / (double)value));
						value = image.Width;
						num1 = (image.Height - value1) / 2;
					}
					else
					{
						value1 = image.Height;
						num = (image.Width - value) / 2;
					}
				}
				using (Graphics graphic = Graphics.FromImage(bitmap))
				{
					graphic.SmoothingMode = SmoothingMode.HighQuality;
					graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
					graphic.CompositingQuality = CompositingQuality.HighQuality;
					graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
					graphic.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(num, num1, value, value1), GraphicsUnit.Pixel);
				}
				image1 = bitmap;
				using (EncoderParameters encoderParameter = new EncoderParameters(1))
				{
					encoderParameter.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
					string empty = string.Empty;
					empty = (!string.IsNullOrEmpty(fileName) ? string.Format("{0}{1}", filePath, string.Concat(fileName, lower)) : string.Format("{0}{1}", filePath, string.Concat(App.Utils.Utils.GetTime(), lower)));
					if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", filePath))))
					{
						Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", filePath)));
					}
					image1.Save(HttpContext.Current.Server.MapPath(string.Concat("~/", empty)), imageCodecInfo, encoderParameter);
					if (bitmap != null)
					{
						bitmap.Dispose();
					}
					str = empty;
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
				throw new Exception(string.Concat("Error First: ", exception.Message));
			}
			return str;
		}
	}
}