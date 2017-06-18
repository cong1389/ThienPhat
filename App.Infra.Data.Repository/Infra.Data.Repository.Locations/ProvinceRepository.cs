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
	public class ProvinceRepository : RepositoryBase<Province>, IProvinceRepository, IRepositoryBase<Province>
	{
		public ProvinceRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public Province GetById(int id)
		{
			Province province = this.FindBy((Province x) => x.Id == id, false).FirstOrDefault<Province>();
			return province;
		}

		protected override IOrderedQueryable<Province> GetDefaultOrder(IQueryable<Province> query)
		{
			IOrderedQueryable<Province> provinces = 
				from p in query
				orderby p.Id
				select p;
			return provinces;
		}

		public IEnumerable<Province> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<Province>();
		}

		public IEnumerable<Province> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<Province, bool>> expression = PredicateBuilder.True<Province>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<Province>((Province x) => x.Name.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}