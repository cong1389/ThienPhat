using App.Core.Utils;
using App.Domain.Entities.Ads;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Ads
{
	public interface IBannerService : IBaseService<Banner>, IService
	{
		Banner GetById(int Id);

		IEnumerable<Banner> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}