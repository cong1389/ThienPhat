using App.Core.Common;
using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace App.Infra.Data.Common
{
	public class BaseAsyncService<T> : IBaseAsyncService<T>, IService
	where T : BaseEntity
	{
		private readonly IRepositoryBaseAsync<T> _repository;

		private readonly IUnitOfWorkAsync _unitOfWork;

		public BaseAsyncService(IRepositoryBaseAsync<T> repository, IUnitOfWorkAsync unitOfWork)
		{
			this._repository = repository;
			this._unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> whereClause, Paging page)
		{
			return await this._repository.FindAsync(whereClause, page);
		}

		public async Task<IEnumerable<T>> FindAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> whereClause, Paging page)
		{
			return await this._repository.FindAsync(cancellationToken, whereClause, page);
		}

		public async Task<IEnumerable<T>> FindAsync<TKey>(CancellationToken cancellationToken, Expression<Func<T, bool>> whereClause, Expression<Func<T, TKey>> orderByClause, Paging page)
		{
			IEnumerable<T> ts = await this._repository.FindAsync<TKey>(cancellationToken, whereClause, orderByClause, page);
			return ts;
		}

		public async Task<IEnumerable<T>> FindAsync<TKey>(Expression<Func<T, bool>> whereClause, Expression<Func<T, TKey>> orderByClause, Paging page)
		{
			return await this._repository.FindAsync(whereClause, page);
		}

		public async Task<IEnumerable<T>> FindByAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> predicate, bool @readonly = false)
		{
			return await this._repository.FindByAsync(cancellationToken, predicate, @readonly);
		}

		public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, bool @readonly = false)
		{
			return await this._repository.FindByAsync(predicate, @readonly);
		}

		public async Task<List<T>> GetAllAsync()
		{
			return await this._repository.GetAllAsync();
		}

		public async Task<IEnumerable<T>> GetAllPagedListAsync(Paging page)
		{
			return await this._repository.GetAllPagedListAsync(page);
		}

		public async Task<IEnumerable<T>> GetAllPagedListAsync(CancellationToken cancellationToken, Paging page)
		{
			return await this._repository.GetAllPagedListAsync(cancellationToken, page);
		}

		public async Task<T> GetAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> where, bool @readonly = false)
		{
			return await this._repository.GetAsync(cancellationToken, where, @readonly);
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>> where, bool @readonly = false)
		{
			return await this._repository.GetAsync(where, @readonly);
		}

		public async Task<IEnumerable<T>> GetTop(int take)
		{
			return await this._repository.GetTop(take);
		}

		public async Task<IEnumerable<T>> GetTop(int take, Expression<Func<T, bool>> whereClause)
		{
			return await this._repository.GetTopBy(take, whereClause);
		}

		public async Task<IEnumerable<T>> GetTop<TKey>(int take, Expression<Func<T, bool>> whereClause, Expression<Func<T, TKey>> orderByClause)
		{
			return await this._repository.GetTopBy<TKey>(take, whereClause, orderByClause);
		}
	}
}