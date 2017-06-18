using App.Core.Utils;
using App.Domain.Entities.Account;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Service.Account
{
	public interface IUserService : IBaseAsyncService<User>, IService
	{
		Task<IEnumerable<User>> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}