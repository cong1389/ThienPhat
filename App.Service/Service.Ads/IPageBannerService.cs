using App.Core.Utils;
using App.Domain.Entities.Ads;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Ads
{
	public interface IPageBannerService : IBaseService<PageBanner>, IService
	{
		PageBanner GetById(int Id);

		IEnumerable<PageBanner> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}