using App.Core.Utils;
using App.Domain.Entities.Account;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Account;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace App.Service.Account
{
	public class UserService : BaseAsyncService<User>, IUserService, IBaseAsyncService<User>, IService
	{
		private readonly IUnitOfWorkAsync _unitOfWork;

		private readonly IUserRepository _userRepository;

		public UserService(IUnitOfWorkAsync unitOfWork, IUserRepository userRepository) : base(userRepository, unitOfWork)
		{
			this._unitOfWork = unitOfWork;
			this._userRepository = userRepository;
		}

		public void BatchCreate(IEnumerable<User> entity)
		{
			throw new NotImplementedException();
		}

		public void BatchDelete(IEnumerable<User> entity)
		{
			throw new NotImplementedException();
		}

		public void Create(User entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(User entity)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<User>> FindAndSort(Expression<Func<User, bool>> whereClause, SortBuilder sortBuilder, Paging page)
		{
			return await this._userRepository.FindAndSort(whereClause, sortBuilder, page);
		}

		public IEnumerable<User> FindBy(Expression<Func<User, bool>> predicate, bool @readonly = false)
		{
			throw new NotImplementedException();
		}

		public User Get(Expression<Func<User, bool>> whereClause, bool @readonly = false)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<User> GetAll()
		{
			throw new NotImplementedException();
		}

		public new IEnumerable<User> GetTop(int take)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<User> GetTop(int take, Expression<Func<User, bool>> whereClause)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<User> GetTop<TKey>(int take, Expression<Func<User, bool>> whereClause, Expression<Func<User, TKey>> orderByClause)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<User>> PagedList(SortingPagingBuilder sortBuider, Paging page)
		{
			return await this._userRepository.PagedSearchList(sortBuider, page);
		}

		public void Update(User entity)
		{
			throw new NotImplementedException();
		}
	}
}