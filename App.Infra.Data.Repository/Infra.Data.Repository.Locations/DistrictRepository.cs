using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Location;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Locations
{
	public class DistrictRepository : RepositoryBase<District>, IDistrictRepository, IRepositoryBase<District>
	{
		public DistrictRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public District GetById(int id)
		{
			District district = this.FindBy((District x) => x.Id == id, false).FirstOrDefault<District>();
			return district;
		}

		protected override IOrderedQueryable<District> GetDefaultOrder(IQueryable<District> query)
		{
			IOrderedQueryable<District> districts = 
				from p in query
				orderby p.Id
				select p;
			return districts;
		}

		public IEnumerable<District> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<District>();
		}

		public IEnumerable<District> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<District, bool>> expression = PredicateBuilder.True<District>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<District>((District x) => x.Name.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}