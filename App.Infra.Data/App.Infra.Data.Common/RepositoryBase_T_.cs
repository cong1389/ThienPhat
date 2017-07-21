using App.Core.Common;
using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Context;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Common
{
	public abstract class RepositoryBase<T> : IRepositoryBase<T>
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

		protected RepositoryBase(IDbFactory dbFactory)
		{
			this.DbFactory = dbFactory;
			this._dbSet = this.DbContext.Set<T>();
		}

		public virtual T Add(T entity)
		{
			return this._dbSet.Add(entity);
		}

		public void BactchAdd(IEnumerable<T> entity)
		{
			foreach (T t in this._dbSet.AsEnumerable<T>())
			{
				this._dbSet.Add(t);
			}
		}

		public virtual void Delete(Expression<Func<T, bool>> where)
		{
			foreach (T t in this._dbSet.Where<T>(where).AsEnumerable<T>())
			{
				this._dataContext.Entry<T>(t).State = EntityState.Deleted;
				this._dbSet.Remove(t);
			}
		}

		public virtual void Delete(T entity)
		{
			this._dataContext.Entry<T>(entity).State = EntityState.Deleted;
			this._dbSet.Remove(entity);
		}

		public virtual IEnumerable<T> Find(Expression<Func<T, bool>> whereClause, Paging page)
		{
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).Count<T>();
			IEnumerable<T> list = this.GetDefaultOrder(this._dbSet.AsNoTracking<T>()).Where<T>(whereClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToList<T>();
			return list;
		}

		public virtual IEnumerable<T> Find<TKey>(Expression<Func<T, bool>> whereClause, Expression<Func<T, TKey>> orderByClause, Paging page)
		{
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).Count<T>();
			IEnumerable<T> list = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).OrderBy<T, TKey>(orderByClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToList<T>();
			return list;
		}

		public virtual IEnumerable<T> FindAndSort(Expression<Func<T, bool>> whereClause, SortBuilder sortBuilder, Paging page)
		{
			IEnumerable<T> list;
			page.TotalRecord = this._dbSet.AsNoTracking<T>().Where<T>(whereClause).Count<T>();
			if (!string.IsNullOrEmpty(sortBuilder.ColumnName))
			{
				list = (sortBuilder.ColumnOrder != SortBuilder.SortOrder.Descending ? this._dbSet.OrderBy<T>(sortBuilder.ColumnName).AsNoTracking<T>().Where<T>(whereClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToList<T>() : this._dbSet.OrderByDescending<T>(sortBuilder.ColumnName).AsNoTracking<T>().Where<T>(whereClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToList<T>());
			}
			else
			{
				list = this.GetDefaultOrder(this._dbSet.AsNoTracking<T>()).Where<T>(whereClause).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToList<T>();
			}
			return list;
		}

		public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, bool @readonly = false)
		{
			return (@readonly ? this._dbSet.AsNoTracking<T>().Where<T>(predicate).ToList<T>() : this._dbSet.Where<T>(predicate).ToList<T>());
		}

		public virtual T Get(Expression<Func<T, bool>> where, bool @readonly = false)
		{
			return (@readonly ? this._dbSet.AsNoTracking<T>().Where<T>(where).FirstOrDefault<T>() : this._dbSet.Where<T>(where).FirstOrDefault<T>());
		}

		public virtual IEnumerable<T> GetAll()
		{
			return this._dbSet.AsNoTracking<T>().ToList<T>();
		}

		public virtual IEnumerable<T> GetAllPagedList(Paging page)
		{
			page.TotalRecord = this._dbSet.Count<T>();
			IEnumerable<T> list = this.GetDefaultOrder(this._dbSet.AsNoTracking<T>()).Skip<T>((page.PageNumber - 1) * page.PageSize).Take<T>(page.PageSize).ToList<T>();
			return list;
		}

		protected abstract IOrderedQueryable<T> GetDefaultOrder(IQueryable<T> query);

		public IEnumerable<T> GetTop(int take)
		{
			IEnumerable<T> ts = this._dbSet.AsNoTracking<T>().Take<T>(take);
			return ts;
		}

		public IEnumerable<T> GetTopBy(int take, Expression<Func<T, bool>> where)
		{
			IEnumerable<T> ts = this._dbSet.AsNoTracking<T>().Where<T>(where).Take<T>(take);
			return ts;
		}

		public IEnumerable<T> GetTopBy<TKey>(int take, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderByClause)
		{
			IEnumerable<T> ts = this._dbSet.AsNoTracking<T>().OrderByDescending<T, TKey>(orderByClause).Where<T>(where).Take<T>(take);
			return ts;
		}

		public virtual void Update(T entity)
		{
			this._dbSet.Attach(entity);
			this._dataContext.Entry<T>(entity).State = EntityState.Modified;
		}


        public virtual IQueryable<T> Table
        {
            get
            {
                //if (_dataContext.ForceNoTracking)
                //{
                //    return this._dbSet.AsNoTracking();
                //}
                return this._dbSet;
            }
        }
    }
}