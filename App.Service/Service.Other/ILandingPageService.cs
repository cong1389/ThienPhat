using App.Core.Utils;
using App.Domain.Entities.Other;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace App.Service.Other
{
	public interface ILandingPageService : IBaseService<LandingPage>, IService
	{
		IEnumerable<LandingPage> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}