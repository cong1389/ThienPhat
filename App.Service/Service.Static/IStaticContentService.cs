using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Static
{
	public interface IStaticContentService : IBaseService<StaticContent>, IService
	{
		StaticContent GetById(int Id);

		IEnumerable<StaticContent> GetBySeoUrl(string seoUrl);

		IEnumerable<StaticContent> PagedList(SortingPagingBuilder sortBuider, Paging page);

		IEnumerable<StaticContent> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page);
	}
}