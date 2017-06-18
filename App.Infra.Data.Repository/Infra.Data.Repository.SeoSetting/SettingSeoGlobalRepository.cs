using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.SeoSetting
{
	public class SettingSeoGlobalRepository : RepositoryBase<SettingSeoGlobal>, ISettingSeoGlobalRepository, IRepositoryBase<SettingSeoGlobal>
	{
		public SettingSeoGlobalRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public SettingSeoGlobal GetById(int Id)
		{
			SettingSeoGlobal settingSeoGlobal = this.FindBy((SettingSeoGlobal x) => x.Id == Id, false).FirstOrDefault<SettingSeoGlobal>();
			return settingSeoGlobal;
		}

		protected override IOrderedQueryable<SettingSeoGlobal> GetDefaultOrder(IQueryable<SettingSeoGlobal> query)
		{
			IOrderedQueryable<SettingSeoGlobal> settingSeoGlobals = 
				from p in query
				orderby p.Id
				select p;
			return settingSeoGlobals;
		}

		public IEnumerable<SettingSeoGlobal> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<SettingSeoGlobal>();
		}

		public IEnumerable<SettingSeoGlobal> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<SettingSeoGlobal, bool>> expression = PredicateBuilder.True<SettingSeoGlobal>();
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}