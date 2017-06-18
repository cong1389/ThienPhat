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

namespace App.Infra.Data.Repository.News
{
	public class NewsRepository : RepositoryBase<App.Domain.Entities.Data.News>, INewsRepository, IRepositoryBase<App.Domain.Entities.Data.News>
	{
		public NewsRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public App.Domain.Entities.Data.News GetById(int Id)
		{
			App.Domain.Entities.Data.News news = this.FindBy((App.Domain.Entities.Data.News x) => x.Id == Id, false).FirstOrDefault<App.Domain.Entities.Data.News>();
			return news;
		}

		protected override IOrderedQueryable<App.Domain.Entities.Data.News> GetDefaultOrder(IQueryable<App.Domain.Entities.Data.News> query)
		{
			IOrderedQueryable<App.Domain.Entities.Data.News> news = 
				from p in query
				orderby p.Id
				select p;
			return news;
		}

		public IEnumerable<App.Domain.Entities.Data.News> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<App.Domain.Entities.Data.News>();
		}

		public IEnumerable<App.Domain.Entities.Data.News> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<App.Domain.Entities.Data.News, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Data.News>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<App.Domain.Entities.Data.News>((App.Domain.Entities.Data.News x) => x.Title.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}

		public IEnumerable<App.Domain.Entities.Data.News> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<App.Domain.Entities.Data.News, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Data.News>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<App.Domain.Entities.Data.News>((App.Domain.Entities.Data.News x) => x.VirtualCategoryId.Contains(sortBuider.Keywords) && x.Status == 1);
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}