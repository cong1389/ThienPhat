using App.Core.Utils;
using App.Domain.Entities.Location;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Locations
{
	public interface IProvinceService : IBaseService<Province>, IService
	{
		Province GetById(int Id);

		IEnumerable<Province> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}