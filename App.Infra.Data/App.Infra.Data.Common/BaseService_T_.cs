using App.Core.Common;
using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.Infra.Data.Common
{
	public abstract class BaseService<T> : IBaseService<T>, IService
	where T : BaseEntity
	{
		private readonly IRepositoryBase<T> _repository;

		private readonly IUnitOfWork _unitOfWork;

		protected BaseService(IUnitOfWork unitOfWork, IRepositoryBase<T> repository)
		{
			this._unitOfWork = unitOfWork;
			this._repository = repository;
		}

		public void BatchCreate(IEnumerable<T> entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			this._repository.BactchAdd(entity);
			this._unitOfWork.Commit();
		}

		public void BatchDelete(IEnumerable<T> entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			foreach (T t in entity)
			{
				this._repository.Delete(t);
			}
			this._unitOfWork.Commit();
		}

		public virtual void Create(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			this._repository.Add(entity);
			this._unitOfWork.Commit();
		}

		public virtual void Delete(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			this._repository.Delete(entity);
			this._unitOfWork.Commit();
		}

		public IEnumerable<T> FindAndSort(Expression<Func<T, bool>> whereClause, SortBuilder sortBuilder, Paging page)
		{
			return this._repository.FindAndSort(whereClause, sortBuilder, page);
		}

		public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, bool @readonly = false)
		{
			return this._repository.FindBy(predicate, @readonly);
		}

		public T Get(Expression<Func<T, bool>> whereClause, bool @readonly = false)
		{
			return this._repository.Get(whereClause, @readonly);
		}

		public virtual IEnumerable<T> GetAll()
		{
			return this._repository.GetAll();
		}

		public IEnumerable<T> GetTop(int take)
		{
			return this._repository.GetTop(take);
		}

		public IEnumerable<T> GetTop(int take, Expression<Func<T, bool>> whereClause)
		{
			return this._repository.GetTopBy(take, whereClause);
		}

		public IEnumerable<T> GetTop<TKey>(int take, Expression<Func<T, bool>> whereClause, Expression<Func<T, TKey>> orderByClause)
		{
			return this._repository.GetTopBy<TKey>(take, whereClause, orderByClause);
		}

		public virtual void Update(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			this._repository.Update(entity);
			this._unitOfWork.Commit();
		}
	}
}