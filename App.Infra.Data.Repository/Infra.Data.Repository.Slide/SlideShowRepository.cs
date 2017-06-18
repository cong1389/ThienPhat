using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Slide;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.Slide
{
	public class SlideShowRepository : RepositoryBase<SlideShow>, ISlideShowRepository, IRepositoryBase<SlideShow>
	{
		public SlideShowRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<SlideShow> GetDefaultOrder(IQueryable<SlideShow> query)
		{
			IOrderedQueryable<SlideShow> slideShows = 
				from p in query
				orderby p.Id
				select p;
			return slideShows;
		}

		public IEnumerable<SlideShow> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<SlideShow>();
		}

		public IEnumerable<SlideShow> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<SlideShow, bool>> expression = PredicateBuilder.True<SlideShow>();
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}