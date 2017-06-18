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

namespace App.Infra.Data.Repository.Post
{
	public class PostRepository : RepositoryBase<App.Domain.Entities.Data.Post>, IPostRepository, IRepositoryBase<App.Domain.Entities.Data.Post>
	{
		public PostRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public override App.Domain.Entities.Data.Post Add(App.Domain.Entities.Data.Post entity)
		{
			return base.Add(entity);
		}

		public App.Domain.Entities.Data.Post GetById(int Id)
		{
			App.Domain.Entities.Data.Post post = this.FindBy((App.Domain.Entities.Data.Post x) => x.Id == Id, false).FirstOrDefault<App.Domain.Entities.Data.Post>();
			return post;
		}

		protected override IOrderedQueryable<App.Domain.Entities.Data.Post> GetDefaultOrder(IQueryable<App.Domain.Entities.Data.Post> query)
		{
			IOrderedQueryable<App.Domain.Entities.Data.Post> posts = 
				from p in query
				orderby p.Id
				select p;
			return posts;
		}

		public IEnumerable<App.Domain.Entities.Data.Post> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<App.Domain.Entities.Data.Post>();
		}

		public IEnumerable<App.Domain.Entities.Data.Post> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<App.Domain.Entities.Data.Post, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Data.Post>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<App.Domain.Entities.Data.Post>((App.Domain.Entities.Data.Post x) => x.Title.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.ProductCode.Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}

		public IEnumerable<App.Domain.Entities.Data.Post> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<App.Domain.Entities.Data.Post, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Data.Post>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<App.Domain.Entities.Data.Post>((App.Domain.Entities.Data.Post x) => x.VirtualCategoryId.Contains(sortBuider.Keywords) && x.Status == 1);
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}