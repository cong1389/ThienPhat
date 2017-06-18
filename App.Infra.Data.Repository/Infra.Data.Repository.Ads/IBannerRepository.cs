using App.Core.Utils;
using App.Domain.Entities.Ads;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Ads
{
	public interface IBannerRepository : IRepositoryBase<Banner>
	{
		Banner GetById(int Id);

		IEnumerable<Banner> PagedList(Paging page);

		IEnumerable<Banner> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}