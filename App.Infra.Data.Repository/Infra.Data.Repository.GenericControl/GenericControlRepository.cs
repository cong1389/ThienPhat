using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.GenericControl
{
	public class GenericControlRepository : RepositoryBase<App.Domain.Entities.GenericControl.GenericControl>, IGenericControlRepository, IRepositoryBase<App.Domain.Entities.GenericControl.GenericControl>
	{
		public GenericControlRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public App.Domain.Entities.GenericControl.GenericControl GetById(int Id)
		{
			App.Domain.Entities.GenericControl.GenericControl GenericControl = this.FindBy((App.Domain.Entities.GenericControl.GenericControl x) => x.Id == Id, false).FirstOrDefault<App.Domain.Entities.GenericControl.GenericControl>();
			return GenericControl;
		}

		protected override IOrderedQueryable<App.Domain.Entities.GenericControl.GenericControl> GetDefaultOrder(IQueryable<App.Domain.Entities.GenericControl.GenericControl> query)
		{
			IOrderedQueryable<App.Domain.Entities.GenericControl.GenericControl> GenericControls = 
				from p in query
				orderby p.Id
				select p;
			return GenericControls;
		}

		public IEnumerable<App.Domain.Entities.GenericControl.GenericControl> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<App.Domain.Entities.GenericControl.GenericControl>();
		}

		public IEnumerable<App.Domain.Entities.GenericControl.GenericControl> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<App.Domain.Entities.GenericControl.GenericControl, bool>> expression = PredicateBuilder.True<App.Domain.Entities.GenericControl.GenericControl>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<App.Domain.Entities.GenericControl.GenericControl>((App.Domain.Entities.GenericControl.GenericControl x) => x.Name.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}