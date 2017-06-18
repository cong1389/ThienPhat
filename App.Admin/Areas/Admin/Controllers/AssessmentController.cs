using App.Admin.Helpers;
using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
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
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class AssessmentController : BaseAdminController
    {
        private readonly IAssessmentService _assessmentService;

        private readonly IBrandService _BrandService;

        private IImagePlugin _imagePlugin;

        public AssessmentController(IAssessmentService assessmentService, IImagePlugin imagePlugin, IBrandService _brandService)
        {
            this._assessmentService = assessmentService;
            this._imagePlugin = imagePlugin;
            this._BrandService = _brandService;
        }

        //[RequiredPermisson(Roles = "CreateEditAssessment")]
        public ActionResult Create()
        {
            return base.View();
        }

        [HttpPost]
        //[RequiredPermisson(Roles = "CreateEditAssessment")]
        public ActionResult Create(AssessmentViewModel post, string ReturnUrl)
        {
            ActionResult action;
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
                        _imagePlugin.CropAndResizeImage(post.Image, string.Format("{0}", Contains.PostFolder), str1, nullable1, nullable, false);
                        post.ImageUrl = string.Concat(Contains.PostFolder, str1);
                    }

                    post.BillNumber = string.Format("Hd{0}",post.PhoneNumber);
                    post.CusomterNumber = string.Format("Kh{0}", post.PhoneNumber);

                    Assessment flowStep = Mapper.Map<AssessmentViewModel, Assessment>(post);
                    this._assessmentService.Create(flowStep);
                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Assessment)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
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

        [RequiredPermisson(Roles = "CreateEditAssessment")]
        public ActionResult Delete(int[] ids)
        {
            try
            {
                if (ids.Length != 0)
                {
                    IEnumerable<Assessment> flowSteps =
                        from id in ids
                        select this._assessmentService.Get((Assessment x) => x.Id == id, false);
                    this._assessmentService.BatchDelete(flowSteps);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("Post.Delete: ", exception.Message));
            }
            return base.RedirectToAction("Index");
        }

        [RequiredPermisson(Roles = "CreateEditAssessment")]
        public ActionResult Edit(int Id)
        {
            AssessmentViewModel flowStepViewModel = Mapper.Map<Assessment, AssessmentViewModel>(this._assessmentService.Get((Assessment x) => x.Id == Id, false));
            return base.View(flowStepViewModel);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditAssessment")]
        public ActionResult Edit(AssessmentViewModel postView, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(postView);
                }
                else
                {
                    Assessment flowStep = this._assessmentService.Get((Assessment x) => x.Id == postView.Id, false);
                    string str = postView.FullName.NonAccent();
                    if (postView.Image != null && postView.Image.ContentLength > 0)
                    {
                        string str1 = string.Concat(str, ".jpg");
                        int? nullable = null;
                        int? nullable1 = nullable;
                        nullable = null;
                        this._imagePlugin.CropAndResizeImage(postView.Image, string.Format("{0}", Contains.PostFolder), str1, nullable1, nullable, false);
                        postView.ImageUrl = string.Concat(Contains.PostFolder, str1);
                    }
                    postView.BillNumber = string.Format("Hd{0}", postView.IdentityCard);
                    postView.CusomterNumber = string.Format("Kh{0}", postView.IdentityCard);

                    Assessment flowStep1 = Mapper.Map<AssessmentViewModel, Assessment>(postView, flowStep);
                    this._assessmentService.Update(flowStep1);
                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Assessment)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
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
                base.ModelState.AddModelError("", exception.Message);
                ExtentionUtils.Log(string.Concat("Post.Edit: ", exception.Message));
                return base.View(postView);
            }
            return action;
        }

        [RequiredPermisson(Roles = "ViewAssessment")]
        public ActionResult Index(int page = 1, string keywords = "")
        {
            ((dynamic)base.ViewBag).Keywords = keywords;
            SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
            {
                Keywords = keywords,
                Sorts = new SortBuilder()
                {
                    ColumnName = "CreatedDate",
                    ColumnOrder = SortBuilder.SortOrder.Descending
                }
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = base._pageSize,
                TotalRecord = 0
            };
            IEnumerable<Assessment> flowSteps = this._assessmentService.PagedList(sortingPagingBuilder, paging);
            if (flowSteps != null && flowSteps.Any<Assessment>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(flowSteps);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].ToString().ToLower().Equals("edit") || filterContext.RouteData.Values["action"].ToString().ToLower().Equals("create"))
            {
                IEnumerable<Brand> brand = this._BrandService.FindBy((Brand x) => x.Status == 1);
                ((dynamic)base.ViewBag).Brand = brand;
            }
        }
    }
}