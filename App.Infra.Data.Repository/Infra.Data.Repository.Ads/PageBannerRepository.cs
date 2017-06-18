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
	public class PageBannerRepository : RepositoryBase<PageBanner>, IPageBannerRepository, IRepositoryBase<PageBanner>
	{
		public PageBannerRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public PageBanner GetById(int Id)
		{
			PageBanner pageBanner = this.FindBy((PageBanner x) => x.Id == Id, false).FirstOrDefault<PageBanner>();
			return pageBanner;
		}

		protected override IOrderedQueryable<PageBanner> GetDefaultOrder(IQueryable<PageBanner> query)
		{
			IOrderedQueryable<PageBanner> pageBanners = 
				from p in query
				orderby p.Id
				select p;
			return pageBanners;
		}

		public IEnumerable<PageBanner> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<PageBanner>();
		}

		public IEnumerable<PageBanner> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<PageBanner, bool>> expression = PredicateBuilder.True<PageBanner>();
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}