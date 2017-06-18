using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Order
{
	public interface IOrderRepository : IRepositoryBase<App.Domain.Entities.Data.Order>
	{
		IEnumerable<App.Domain.Entities.Data.Order> PagedList(Paging page);

		IEnumerable<App.Domain.Entities.Data.Order> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}