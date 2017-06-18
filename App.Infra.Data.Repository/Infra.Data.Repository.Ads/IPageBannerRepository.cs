using App.Core.Utils;
using App.Domain.Entities.Ads;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Ads
{
	public interface IPageBannerRepository : IRepositoryBase<PageBanner>
	{
		PageBanner GetById(int Id);

		IEnumerable<PageBanner> PagedList(Paging page);

		IEnumerable<PageBanner> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}