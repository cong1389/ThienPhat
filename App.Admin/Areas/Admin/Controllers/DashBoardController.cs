using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Service.News;
using App.Service.Post;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class DashBoardController : BaseAdminController
	{
		private readonly IPostService _postService;

		private readonly INewsService _newsService;

		public DashBoardController(IPostService postService, INewsService newsService)
		{
			this._postService = postService;
			this._newsService = newsService;
		}

		public async Task<JsonResult> GetNewRealtime()
		{
			INewsService newsService = this._newsService;
			Expression<Func<News, bool>> status = (News x) => x.Status >= 0;
			IEnumerable<News> top = newsService.GetTop<DateTime>(50, status, (News x) => x.CreatedDate);
			IOrderedEnumerable<News> news = await Task.FromResult<IOrderedEnumerable<News>>(
				from x in top
				orderby x.CreatedDate descending
				select x);
			IOrderedEnumerable<News> news1 = news;
			JsonResult jsonResult = this.Json(new { success = true, list = this.RenderRazorViewToString("_DashBoardNews", news1) }, JsonRequestBehavior.AllowGet);
			return jsonResult;
		}

		public async Task<JsonResult> GetPostRealtime()
		{
			IPostService postService = this._postService;
			Expression<Func<Post, bool>> status = (Post x) => x.Status >= 0;
			IEnumerable<Post> top = postService.GetTop<DateTime>(50, status, (Post x) => x.CreatedDate);
			IOrderedEnumerable<Post> posts = await Task.FromResult<IOrderedEnumerable<Post>>(
				from x in top
				orderby x.CreatedDate descending
				select x);
			IOrderedEnumerable<Post> posts1 = posts;
			JsonResult jsonResult = this.Json(new { success = true, list = this.RenderRazorViewToString("_DashBoardPost", posts1) }, JsonRequestBehavior.AllowGet);
			return jsonResult;
		}
	}
}