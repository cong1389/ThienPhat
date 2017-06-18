using App.Core.Utils;
using App.Domain.Entities.Location;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Locations
{
	public interface IDistrictService : IBaseService<District>, IService
	{
		District GetById(int Id);

		IEnumerable<District> GetByProvinceId(int provinceId);

		IEnumerable<District> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}