using System;
using System.Drawing;
using System.Web;

namespace App.ImagePlugin
{
	public interface IImagePlugin
	{
		void CropAndResizeImage(HttpPostedFileBase imageFile, string outPutFilePath, string outPuthFileName, int? width = null, int? height = null, bool pngFormat = false);

		void CropAndResizeImage(string inPutFilePath, string outPutFilePath, string outPuthFileName, int? width = null, int? height = null, bool pngFormat = false);

		void CropAndResizeImage(Image image, string outPutFilePath, string outPuthFileName, int? width = null, int? height = null, bool pngFormat = false);
	}
}