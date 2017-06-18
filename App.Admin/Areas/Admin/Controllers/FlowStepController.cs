using App.Admin.Helpers;
using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Step;
using App.Framework.Ultis;
using App.ImagePlugin;
using App.Service.Step;
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
	public class FlowStepController : BaseAdminController
	{
		private readonly IFlowStepService _flowStepService;

		private IImagePlugin _imagePlugin;

		public FlowStepController(IFlowStepService flowStepService, IImagePlugin imagePlugin)
		{
			this._flowStepService = flowStepService;
			this._imagePlugin = imagePlugin;
		}

		[RequiredPermisson(Roles="CreateEditFlowStep")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditFlowStep")]
		public ActionResult Create(FlowStepViewModel post, string ReturnUrl)
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
                    string str = post.Title.NonAccent();
                    if (post.Image != null && post.Image.ContentLength > 0)
					{
						string str1 = string.Concat(str, ".jpg");
						int? nullable = null;
						int? nullable1 = nullable;
						nullable = null;
                        _imagePlugin.CropAndResizeImage(post.Image, string.Format("{0}", Contains.PostFolder), str1, nullable1, nullable, false);
						post.ImageUrl = string.Concat(Contains.PostFolder, str1);
					}
					FlowStep flowStep = Mapper.Map<FlowStepViewModel, FlowStep>(post);
					this._flowStepService.Create(flowStep);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.FlowStep)));
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

		[RequiredPermisson(Roles="CreateEditFlowStep")]
		public ActionResult Delete(int[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<FlowStep> flowSteps = 
						from id in ids
						select this._flowStepService.Get((FlowStep x) => x.Id == id, false);
					this._flowStepService.BatchDelete(flowSteps);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("Post.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditFlowStep")]
		public ActionResult Edit(int Id)
		{
			FlowStepViewModel flowStepViewModel = Mapper.Map<FlowStep, FlowStepViewModel>(this._flowStepService.Get((FlowStep x) => x.Id == Id, false));
			return base.View(flowStepViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditFlowStep")]
		public ActionResult Edit(FlowStepViewModel postView, string ReturnUrl)
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
					FlowStep flowStep = this._flowStepService.Get((FlowStep x) => x.Id == postView.Id, false);
					string str = postView.Title.NonAccent();
					if (postView.Image != null && postView.Image.ContentLength > 0)
					{
						string str1 = string.Concat(str, ".jpg");
						int? nullable = null;
						int? nullable1 = nullable;
						nullable = null;
						this._imagePlugin.CropAndResizeImage(postView.Image, string.Format("{0}", Contains.PostFolder), str1, nullable1, nullable, false);
						postView.ImageUrl = string.Concat(Contains.PostFolder, str1);
					}
					FlowStep flowStep1 = Mapper.Map<FlowStepViewModel, FlowStep>(postView, flowStep);
					this._flowStepService.Update(flowStep1);
					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.FlowStep)));
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

		[RequiredPermisson(Roles="ViewFlowStep")]
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
			IEnumerable<FlowStep> flowSteps = this._flowStepService.PagedList(sortingPagingBuilder, paging);
			if (flowSteps != null && flowSteps.Any<FlowStep>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(flowSteps);
		}
	}
}