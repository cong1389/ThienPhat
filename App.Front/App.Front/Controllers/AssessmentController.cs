using App.Domain.Entities.Data;
using App.Domain.Entities.Brandes;
using App.FakeEntity.Assessments;
using App.Framework.Ultis;
using App.ImagePlugin;
using App.Service.Assessments;
using App.Service.Brandes;
using App.Utils;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace App.Front.Controllers
{
    public class AssessmentController : FrontBaseController
    {
        private readonly IAssessmentService _assessmentService;

        private readonly IBrandService _BrandService;

        private IImagePlugin _imagePlugin;

        public AssessmentController(IAssessmentService fssessmentService, IImagePlugin imagePlugin, IBrandService brandService)
        {
            this._assessmentService = fssessmentService;
            this._imagePlugin = imagePlugin;
            this._BrandService = brandService;
        }


        public ActionResult Create()
        {
            return base.View();
        }

        [HttpPost]
        //[RequiredPermisson(Roles = "CreateEditAssessment")]
        public ActionResult Create(AssessmentViewModel post, string ReturnUrl)
        {
            ActionResult action = null;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(post);
                }
                else
                {
                    string str = post.FullName.NonAccent();
                    if (post.Image != null && post.Image.ContentLength > 0)
                    {
                        string str1 = string.Concat(str, ".jpg");
                        int? nullable = null;
                        int? nullable1 = nullable;
                        nullable = null;
                        _imagePlugin.CropAndResizeImage(post.Image, string.Format("{0}", Contains.AssessmentFolder), str1, nullable1, nullable, false);
                        post.ImageUrl = string.Concat(Contains.AssessmentFolder, str1);
                    }

                    post.BillNumber = string.Format("Hd{0}", post.PhoneNumber);
                    post.CusomterNumber = string.Format("Kh{0}", post.PhoneNumber);

                    Assessment assessment = Mapper.Map<AssessmentViewModel, Assessment>(post);
                    this._assessmentService.Create(assessment);
                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Assessment)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        action = this.Redirect(ReturnUrl);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("Post.Create: ", exception.Message));
                base.ModelState.AddModelError("", exception.Message);
                return base.View(post);
            }
            return action;
        }

        public ActionResult Index()
        {
            IEnumerable<Assessment> assessment = this._assessmentService.FindBy((Assessment x) => x.Status == 1, false);
            return base.PartialView(assessment);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].ToString().ToLower().Equals("edit") || filterContext.RouteData.Values["action"].ToString().ToLower().Equals("create"))
            {
                IEnumerable<Brand> Brand = this._BrandService.FindBy((Brand x) => x.Status == 1);
                ((dynamic)base.ViewBag).Brand = Brand;
            }
        }
    }
}