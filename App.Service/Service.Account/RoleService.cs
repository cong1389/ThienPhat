using App.Core.Utils;
using App.Domain.Entities.Account;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Account;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Service.Account
{
	public class RoleService : BaseAsyncService<Role>, IRoleService, IBaseAsyncService<Role>, IService
	{
		private readonly IUnitOfWorkAsync _unitOfWork;

		private readonly IRoleRepository _roleRepository;

		public RoleService(IUnitOfWorkAsync unitOfWork, IRoleRepository roleRepository) : base(roleRepository, unitOfWork)
		{
			this._unitOfWork = unitOfWork;
			this._roleRepository = roleRepository;
		}

		public Task<IEnumerable<Role>> PagedList(SortingPagingBuilder sortBuider, Paging page)
		{
			return this._roleRepository.PagedSearchList(sortBuider, page);
		}
	}
}