using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.Order
{
    public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository, IRepositoryBase<OrderItem>
    {
        public OrderItemRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        protected override IOrderedQueryable<OrderItem> GetDefaultOrder(IQueryable<OrderItem> query)
        {
            IOrderedQueryable<OrderItem> orderItems =
                from p in query
                orderby p.Id
                select p;
            return orderItems;
        }
    }
}