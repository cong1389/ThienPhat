using App.Core.Utils;
using App.Domain.Entities.Brandes;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Brandes
{
	public interface IBrandRepository : IRepositoryBase<Brand>
	{
		Brand GetById(int id);

		IEnumerable<Brand> PagedList(Paging page);

		IEnumerable<Brand> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}