using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.Account;
using App.Framework.Ultis;
using App.Service.Account;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class RoleController : BaseAdminController
	{
		private readonly IRoleService _roleService;

		public RoleController(IRoleService roleService)
		{
			this._roleService = roleService;
		}

		[RequiredPermisson(Roles="ViewRole")]
		public async Task<ActionResult> Index(int page = 1, string keywords = "")
		{
			((dynamic)this.ViewBag).Keywords = keywords;
			SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
			{
				Keywords = keywords
			};
			SortBuilder sortBuilder = new SortBuilder()
			{
				ColumnName = "Name",
				ColumnOrder = SortBuilder.SortOrder.Descending
			};
			sortingPagingBuilder.Sorts = sortBuilder;
			SortingPagingBuilder sortingPagingBuilder1 = sortingPagingBuilder;
			Paging paging = new Paging()
			{
				PageNumber = page,
				PageSize = this._pageSize,
				TotalRecord = 0
			};
			Paging paging1 = paging;
			IEnumerable<Role> roles = await this._roleService.PagedList(sortingPagingBuilder1, paging1);
			if (roles != null && roles.Any<Role>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging1.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)this.ViewBag).PageInfo = pageInfo;
			}
			return this.View(roles);
		}
	}
}