using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Other;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Other
{
	public class LandingPageRepository : RepositoryBase<LandingPage>, ILandingPageRepository, IRepositoryBase<LandingPage>
	{
		public LandingPageRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<LandingPage> GetDefaultOrder(IQueryable<LandingPage> query)
		{
			IOrderedQueryable<LandingPage> landingPages = 
				from p in query
				orderby p.Id
				select p;
			return landingPages;
		}

		public IEnumerable<LandingPage> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<LandingPage>();
		}

		public IEnumerable<LandingPage> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<LandingPage, bool>> expression = PredicateBuilder.True<LandingPage>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<LandingPage>((LandingPage x) => x.FullName.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Email.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.PhoneNumber.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.DateOfBith.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}