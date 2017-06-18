using App.Core.Utils;
using App.Domain.Entities.Other;
using App.Domain.Interfaces.Repository;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Other
{
	public interface ILandingPageRepository : IRepositoryBase<LandingPage>
	{
		IEnumerable<LandingPage> PagedList(Paging page);

		IEnumerable<LandingPage> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}