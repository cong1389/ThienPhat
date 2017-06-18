using App.Core.Common;
using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Context;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace App.Infra.Data.Common
{
	public abstract class RepositoryBaseAsync<T> : IRepositoryBaseAsync<T>
	where T : BaseEntity
	{
		private readonly IDbSet<T> _dbSet;

		private App.Infra.Data.Context.AppContext _dataContext;

		protected App.Infra.Data.Context.AppContext DbContext
		{
			get
			{
				App.Infra.Data.Context.AppContext appContext = this._dataContext;
				if (appContext == null)
				{
					App.Infra.Data.Context.AppContext appContext1 = this.DbFactory.Init();
					App.Infra.Data.Context.AppContext appContext2 = appContext1;
					this._dataContext = appContext1;
					appContext = appContext2;
				}
				return appContext;
			}
		}

		protected IDbFactory DbFactory
		{
			get;
		}

		protected RepositoryBaseAsync(IDbFactory dbFactory)
		{
			this.DbFactory = dbFactory;
			this._dbSet = this.DbContext.Set<T>();
		}

		public T Add(T entity)
		{
			return this._dbSet.Add(entity);
		}

		public void Delete(Expression<Func<T, bool>> where)
		{
			foreach (T t in this._dbSet.Where<T>(where).AsEnumerable<T>())
			{
				this._dbSet.Remove(t);
			}
		}

		public void Delete(T entity)
		{
			this._dbSet.Remove(entity);
		}

		public IEnumerable<T> Find<TKey>(Expression<Func<T, bool>> whereClause, Expression<Func<T, TKey>> orderByClause, Paging page)
		{
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).Count<T>();
			IEnumerable<T> list = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).OrderBy<T, TKey>(orderByClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToList<T>();
			return list;
		}

		public IEnumerable<T> Find(Expression<Func<T, bool>> whereClause, Paging page)
		{
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).Count<T>();
			IEnumerable<T> list = this.GetDefaultOrder(this._dbSet.AsNoTracking<T>()).Where<T>(whereClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToList<T>();
			return list;
		}

		public virtual async Task<IEnumerable<T>> FindAndSort(Expression<Func<T, bool>> whereClause, SortBuilder sortBuilder, Paging page)
		{
			IEnumerable<T> list;
			List<T> listAsync;
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).Count<T>();
			if (string.IsNullOrEmpty(sortBuilder.ColumnName))
			{
				list = this.GetDefaultOrder(this._dbSet.AsNoTracking<T>()).Where<T>(whereClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToList<T>();
			}
			else if (sortBuilder.ColumnOrder != SortBuilder.SortOrder.Ascending)
			{
				listAsync = await this._dbSet.OrderByDescending<T>(sortBuilder.ColumnName).AsNoTracking<T>().Where<T>(whereClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToListAsync<T>();
				list = listAsync;
			}
			else
			{
				listAsync = await this._dbSet.OrderByDescending<T>(sortBuilder.ColumnName).AsNoTracking<T>().Where<T>(whereClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToListAsync<T>();
				list = listAsync;
			}
			return list;
		}

		public virtual async Task<IEnumerable<T>> FindAsync<TKey>(CancellationToken cancellationToken, Expression<Func<T, bool>> whereClause, Expression<Func<T, TKey>> orderByClause, Paging page)
		{
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).Count<T>();
			List<T> listAsync = await QueryableExtensions.ToListAsync<T>(this._dbSet.AsNoTracking<T>().Where<T>(whereClause).OrderBy<T, TKey>(orderByClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize), cancellationToken);
			return listAsync;
		}

		public virtual async Task<IEnumerable<T>> FindAsync<TKey>(Expression<Func<T, bool>> whereClause, Expression<Func<T, TKey>> orderByClause, Paging page)
		{
			page.TotalRecord = this._dbSet.Where<T>(whereClause).Count<T>();
			List<T> listAsync = await this._dbSet.AsNoTracking<T>().Where<T>(whereClause).OrderBy<T, TKey>(orderByClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToListAsync<T>();
			return listAsync;
		}

		public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> whereClause, Paging page)
		{
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).Count<T>();
			List<T> listAsync = await this.GetDefaultOrder(this._dbSet.AsNoTracking<T>()).Where<T>(whereClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToListAsync<T>();
			return listAsync;
		}

		public virtual async Task<IEnumerable<T>> FindAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> whereClause, Paging page)
		{
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).Count<T>();
			List<T> listAsync = await QueryableExtensions.ToListAsync<T>(this.GetDefaultOrder(this._dbSet.AsNoTracking<T>()).Where<T>(whereClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize), cancellationToken);
			return listAsync;
		}

		public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, bool @readonly = false)
		{
			return (@readonly ? this._dbSet.AsNoTracking<T>().Where<T>(predicate).ToList<T>() : this._dbSet.Where<T>(predicate).ToList<T>());
		}

		public virtual async Task<IEnumerable<T>> FindByAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> predicate, bool @readonly = false)
		{
			Task<List<T>> task;
			task = (@readonly ? QueryableExtensions.ToListAsync<T>(this._dbSet.AsNoTracking<T>().Where<T>(predicate), cancellationToken) : QueryableExtensions.ToListAsync<T>(this._dbSet.Where<T>(predicate), cancellationToken));
			return await task;
		}

		public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, bool @readonly = false)
		{
			Task<List<T>> task;
			task = (@readonly ? this._dbSet.AsNoTracking<T>().Where<T>(predicate).ToListAsync<T>() : this._dbSet.Where<T>(predicate).ToListAsync<T>());
			return await task;
		}

		public T FindById(object id, bool @readonly = false)
		{
			return this._dbSet.Find(new object[] { id });
		}

		public T Get(Expression<Func<T, bool>> where, bool @readonly = false)
		{
			return (@readonly ? this._dbSet.AsNoTracking<T>().Where<T>(where).FirstOrDefault<T>() : this._dbSet.Where<T>(where).FirstOrDefault<T>());
		}

		public IEnumerable<T> GetAll()
		{
			return this._dbSet.AsNoTracking<T>().ToList<T>();
		}

		public virtual Task<List<T>> GetAllAsync()
		{
			return this._dbSet.AsNoTracking<T>().ToListAsync<T>();
		}

		public virtual async Task<IEnumerable<T>> GetAllPagedListAsync(Paging page)
		{
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Count<T>();
			List<T> listAsync = await this.GetDefaultOrder(this._dbSet.AsNoTracking<T>()).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToListAsync<T>();
			return listAsync;
		}

		public virtual async Task<IEnumerable<T>> GetAllPagedListAsync(CancellationToken cancellationToken, Paging page)
		{
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Count<T>();
			List<T> listAsync = await QueryableExtensions.ToListAsync<T>(this.GetDefaultOrder(this._dbSet.AsNoTracking<T>()).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize), cancellationToken);
			return listAsync;
		}

		public Task<T> GetAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> where, bool @readonly = false)
		{
			return (@readonly ? this._dbSet.AsNoTracking<T>().Where<T>(where).FirstOrDefaultAsync<T>(cancellationToken) : this._dbSet.Where<T>(where).FirstOrDefaultAsync<T>(cancellationToken));
		}

		public Task<T> GetAsync(Expression<Func<T, bool>> where, bool @readonly = false)
		{
			return (@readonly ? this._dbSet.AsNoTracking<T>().Where<T>(where).FirstOrDefaultAsync<T>() : this._dbSet.Where<T>(where).FirstOrDefaultAsync<T>());
		}

		protected abstract IOrderedQueryable<T> GetDefaultOrder(IQueryable<T> query);

		public async Task<IEnumerable<T>> GetTop(int take)
		{
			List<T> listAsync = await this._dbSet.AsNoTracking<T>().Take<T>(take).ToListAsync<T>();
			return listAsync;
		}

		public async Task<IEnumerable<T>> GettopBy<TKey>(Expression<Func<T, bool>> whereClause, Expression<Func<T, TKey>> orderByClause, int take)
		{
			List<T> listAsync = await this._dbSet.AsNoTracking<T>().OrderBy<T, TKey>(orderByClause).Where<T>(whereClause).Take<T>(take).ToListAsync<T>();
			return listAsync;
		}

		public Task<IEnumerable<T>> GetTopBy<TKey>(int take, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderByClause)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<T>> GetTopBy(int take, Expression<Func<T, bool>> where)
		{
			List<T> listAsync = await this._dbSet.AsNoTracking<T>().Where<T>(where).Take<T>(take).ToListAsync<T>();
			return listAsync;
		}

		public virtual async Task<int> SaveAsync()
		{
			return await this._dataContext.SaveChangesAsync();
		}

		public virtual async Task<int> SaveAsync(CancellationToken cancellationToken)
		{
			return await this._dataContext.SaveChangesAsync(cancellationToken);
		}

		public void Update(T entity)
		{
			this._dbSet.Attach(entity);
			this._dataContext.Entry<T>(entity).State = EntityState.Modified;
		}
	}
}