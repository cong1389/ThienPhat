using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace App.Service.Order
{
	public interface IOrderService : IBaseService<App.Domain.Entities.Data.Order>, IService
	{
		IEnumerable<App.Domain.Entities.Data.Order> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}