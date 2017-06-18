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

namespace App.Infra.Data.Repository.Static
{
	public class StaticContentRepository : RepositoryBase<StaticContent>, IStaticContentRepository, IRepositoryBase<StaticContent>
	{
		public StaticContentRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public StaticContent GetById(int Id)
		{
			StaticContent staticContent = this.FindBy((StaticContent x) => x.Id == Id, false).FirstOrDefault<StaticContent>();
			return staticContent;
		}

		protected override IOrderedQueryable<StaticContent> GetDefaultOrder(IQueryable<StaticContent> query)
		{
			IOrderedQueryable<StaticContent> staticContents = 
				from p in query
				orderby p.Id
				select p;
			return staticContents;
		}

		public IEnumerable<StaticContent> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<StaticContent>();
		}

		public IEnumerable<StaticContent> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<StaticContent, bool>> expression = PredicateBuilder.True<StaticContent>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<StaticContent>((StaticContent x) => x.Title.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}

		public IEnumerable<StaticContent> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<StaticContent, bool>> expression = PredicateBuilder.True<StaticContent>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<StaticContent>((StaticContent x) => x.VirtualCategoryId.Contains(sortBuider.Keywords) && x.Status == 1);
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}