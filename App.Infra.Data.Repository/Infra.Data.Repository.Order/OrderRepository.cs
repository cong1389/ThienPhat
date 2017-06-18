using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Order
{
	public class OrderRepository : RepositoryBase<App.Domain.Entities.Data.Order>, IOrderRepository, IRepositoryBase<App.Domain.Entities.Data.Order>
	{
		public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<App.Domain.Entities.Data.Order> GetDefaultOrder(IQueryable<App.Domain.Entities.Data.Order> query)
		{
			IOrderedQueryable<App.Domain.Entities.Data.Order> orders = 
				from p in query
				orderby p.Id
				select p;
			return orders;
		}

		public IEnumerable<App.Domain.Entities.Data.Order> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<App.Domain.Entities.Data.Order>();
		}

		public IEnumerable<App.Domain.Entities.Data.Order> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<App.Domain.Entities.Data.Order, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Data.Order>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<App.Domain.Entities.Data.Order>((App.Domain.Entities.Data.Order x) => x.OrderCode.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.CustomerCode.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.CustomerName.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.CreatedDate.ToString("dd/MM/yyyy").ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}