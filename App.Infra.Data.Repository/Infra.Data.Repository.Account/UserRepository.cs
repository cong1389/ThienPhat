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
	public class UserRepository : RepositoryBaseAsync<User>, IUserRepository, IRepositoryBaseAsync<User>
	{
		public UserRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public User FindByEmail(string email)
		{
			User user = base.Get((User x) => x.Email.Equals(email), false);
			return user;
		}

		public async Task<User> FindByEmailAsync(string email)
		{
			UserRepository userRepository = this;
			User async = await userRepository.GetAsync((User x) => x.Email.Equals(email), false);
			return async;
		}

		public Task<User> FindByEmailAsync(CancellationToken cancellationToken, string email)
		{
			Task<User> async = base.GetAsync(cancellationToken, (User x) => x.Email.Equals(email), false);
			return async;
		}

		public User FindByUserName(string username)
		{
			User user = base.Get((User x) => x.UserName.Equals(username), false);
			return user;
		}

		public async Task<User> FindByUserNameAsync(string username)
		{
			UserRepository userRepository = this;
			User async = await userRepository.GetAsync((User x) => x.UserName.Equals(username), false);
			return async;
		}

		public async Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username)
		{
			UserRepository userRepository = this;
			CancellationToken cancellationToken1 = cancellationToken;
			User async = await userRepository.GetAsync(cancellationToken1, (User x) => x.UserName.Equals(username), false);
			return async;
		}

		protected override IOrderedQueryable<User> GetDefaultOrder(IQueryable<User> query)
		{
			IOrderedQueryable<User> users = 
				from x in query
				orderby x.UserName
				select x;
			return users;
		}

		public async Task<IEnumerable<User>> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<User, bool>> expression = PredicateBuilder.True<User>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				Expression<Func<User, bool>> expression1 = expression;
				expression = expression1.And<User>((User x) => x.UserName.Contains(sortBuider.Keywords));
			}
			return await this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}