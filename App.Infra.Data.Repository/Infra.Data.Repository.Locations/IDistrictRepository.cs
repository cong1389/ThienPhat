using App.Core.Utils;
using App.Domain.Entities.Location;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Locations
{
	public interface IDistrictRepository : IRepositoryBase<District>
	{
		District GetById(int id);

		IEnumerable<District> PagedList(Paging page);

		IEnumerable<District> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}