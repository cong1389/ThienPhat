using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Static
{
	public interface IStaticContentRepository : IRepositoryBase<StaticContent>
	{
		StaticContent GetById(int Id);

		IEnumerable<StaticContent> PagedList(Paging page);

		IEnumerable<StaticContent> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);

		IEnumerable<StaticContent> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page);
	}
}