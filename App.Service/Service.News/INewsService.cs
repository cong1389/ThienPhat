using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.News
{
	public interface INewsService : IBaseService<App.Domain.Entities.Data.News>, IService
	{
		App.Domain.Entities.Data.News GetById(int Id);

		IEnumerable<App.Domain.Entities.Data.News> GetBySeoUrl(string seoUrl);

		IEnumerable<App.Domain.Entities.Data.News> PagedList(SortingPagingBuilder sortBuider, Paging page);

		IEnumerable<App.Domain.Entities.Data.News> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page);
	}
}