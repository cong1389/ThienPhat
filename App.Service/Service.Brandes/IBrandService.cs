using App.Core.Utils;
using App.Domain.Entities.Brandes;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Brandes
{
	public interface IBrandService : IBaseService<Brand>, IService
	{
		Brand GetById(int Id);

		IEnumerable<Brand> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}