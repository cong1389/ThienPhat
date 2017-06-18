using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Post
{
	public interface IPostRepository : IRepositoryBase<App.Domain.Entities.Data.Post>
	{
		App.Domain.Entities.Data.Post GetById(int Id);

		IEnumerable<App.Domain.Entities.Data.Post> PagedList(Paging page);

		IEnumerable<App.Domain.Entities.Data.Post> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);

		IEnumerable<App.Domain.Entities.Data.Post> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page);
	}
}