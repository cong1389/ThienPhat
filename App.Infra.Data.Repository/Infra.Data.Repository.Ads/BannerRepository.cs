using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Ads;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Ads
{
	public class BannerRepository : RepositoryBase<Banner>, IBannerRepository, IRepositoryBase<Banner>
	{
		public BannerRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public Banner GetById(int Id)
		{
			Banner banner = this.FindBy((Banner x) => x.Id == Id, false).FirstOrDefault<Banner>();
			return banner;
		}

		protected override IOrderedQueryable<Banner> GetDefaultOrder(IQueryable<Banner> query)
		{
			IOrderedQueryable<Banner> banners = 
				from p in query
				orderby p.Id
				select p;
			return banners;
		}

		public IEnumerable<Banner> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<Banner>();
		}

		public IEnumerable<Banner> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<Banner, bool>> expression = PredicateBuilder.True<Banner>();
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}