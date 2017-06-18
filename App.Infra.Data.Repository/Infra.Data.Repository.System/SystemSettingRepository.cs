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

namespace App.Infra.Data.Repository.System
{
	public class SystemSettingRepository : RepositoryBase<SystemSetting>, ISystemSettingRepository, IRepositoryBase<SystemSetting>
	{
		public SystemSettingRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public SystemSetting GetById(int Id)
		{
			SystemSetting systemSetting = this.FindBy((SystemSetting x) => x.Id == Id, false).FirstOrDefault<SystemSetting>();
			return systemSetting;
		}

		protected override IOrderedQueryable<SystemSetting> GetDefaultOrder(IQueryable<SystemSetting> query)
		{
			IOrderedQueryable<SystemSetting> systemSettings = 
				from p in query
				orderby p.Id
				select p;
			return systemSettings;
		}

		public IEnumerable<SystemSetting> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<SystemSetting>();
		}

		public IEnumerable<SystemSetting> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<SystemSetting, bool>> expression = PredicateBuilder.True<SystemSetting>();
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}