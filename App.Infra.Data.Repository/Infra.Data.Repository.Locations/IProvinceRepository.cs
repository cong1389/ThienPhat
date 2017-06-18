using App.Core.Utils;
using App.Domain.Entities.Location;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Locations
{
	public interface IProvinceRepository : IRepositoryBase<Province>
	{
		Province GetById(int id);

		IEnumerable<Province> PagedList(Paging page);

		IEnumerable<Province> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}