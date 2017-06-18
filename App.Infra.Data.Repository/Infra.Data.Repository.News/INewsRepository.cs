using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.News
{
	public interface INewsRepository : IRepositoryBase<App.Domain.Entities.Data.News>
	{
		App.Domain.Entities.Data.News GetById(int Id);

		IEnumerable<App.Domain.Entities.Data.News> PagedList(Paging page);

		IEnumerable<App.Domain.Entities.Data.News> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);

		IEnumerable<App.Domain.Entities.Data.News> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page);
	}
}