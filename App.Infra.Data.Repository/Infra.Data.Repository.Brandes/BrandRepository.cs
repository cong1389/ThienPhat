using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Brandes;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Brandes
{
	public class BrandRepository : RepositoryBase<Brand>, IBrandRepository, IRepositoryBase<Brand>
	{
		public BrandRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public Brand GetById(int id)
		{
			Brand province = this.FindBy((Brand x) => x.Id == id, false).FirstOrDefault<Brand>();
			return province;
		}

		protected override IOrderedQueryable<Brand> GetDefaultOrder(IQueryable<Brand> query)
		{
			IOrderedQueryable<Brand> Brand = 
				from p in query
				orderby p.Id
				select p;
			return Brand;
		}

		public IEnumerable<Brand> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<Brand>();
		}

		public IEnumerable<Brand> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<Brand, bool>> expression = PredicateBuilder.True<Brand>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<Brand>((Brand x) => x.Name.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}