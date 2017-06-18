using App.Admin.Helpers;
using App.ImagePlugin;
using App.Utils;
using System;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class UtilityController : Controller
	{
		private readonly IImagePlugin _imagePlugin;

		public UtilityController(IImagePlugin imagePlugin)
		{
			this._imagePlugin = imagePlugin;
		}

		public ActionResult GoogleMap()
		{
			return base.View();
		}

		public ActionResult Upload()
		{
			HttpPostedFileBase item = base.HttpContext.Request.Files["upload"];
			string str = base.HttpContext.Request["CKEditorFuncNum"];
			Guid guid = Guid.NewGuid();
			string str1 = string.Concat(guid.ToString(), ".jpg");
			this._imagePlugin.CropAndResizeImage(item, string.Format("{0}", Contains.PostFolder), str1, new int?(ImageSize.WithOrignalSize), new int?(ImageSize.HeighthOrignalSize), false);
			string str2 = string.Concat(new string[] { "http://", base.HttpContext.Request.Url.Authority, "/", Contains.PostFolder, str1 });
			base.HttpContext.Response.Write(string.Concat(new string[] { "<script>window.parent.CKEDITOR.tools.callFunction(", str, ", \"", str2, "\");</script>" }));
			return new EmptyResult();
		}
	}
}