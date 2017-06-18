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
	public class OrderGalleryRepository : RepositoryBase<OrderGallery>, IOrderGalleryRepository, IRepositoryBase<OrderGallery>
	{
		public OrderGalleryRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<OrderGallery> GetDefaultOrder(IQueryable<OrderGallery> query)
		{
			IOrderedQueryable<OrderGallery> orderGalleries = 
				from p in query
				orderby p.Id
				select p;
			return orderGalleries;
		}
	}
}