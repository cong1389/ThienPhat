using App.Core.Utils;
using App.Domain.Entities.Account;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace App.Infra.Data.Repository.Account
{
	public class RoleRepository : RepositoryBaseAsync<Role>, IRoleRepository, IRepositoryBaseAsync<Role>
	{
		public RoleRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public Role FindByName(string roleName)
		{
			Role role = base.Get((Role x) => x.Name.Equals(roleName), false);
			return role;
		}

		public Task<Role> FindByNameAsync(string roleName)
		{
			Task<Role> async = base.GetAsync((Role x) => x.Name.Equals(roleName), false);
			return async;
		}

		public Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName)
		{
			Task<Role> async = base.GetAsync(cancellationToken, (Role x) => x.Name.Equals(roleName), false);
			return async;
		}

		protected override IOrderedQueryable<Role> GetDefaultOrder(IQueryable<Role> query)
		{
			IOrderedQueryable<Role> roles = 
				from p in query
				orderby p.Name
				select p;
			return roles;
		}

		public async Task<IEnumerable<Role>> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<Role, bool>> expression = PredicateBuilder.True<Role>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				Expression<Func<Role, bool>> expression1 = expression;
				expression = expression1.And<Role>((Role x) => x.Name.Contains(sortBuider.Keywords));
			}
			return await this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}