using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.RepositoryAsync.Post
{
	public class PostRepositoryAsync : RepositoryBaseAsync<App.Domain.Entities.Data.Post>, IPostRepositoryAsync, IRepositoryBaseAsync<App.Domain.Entities.Data.Post>
	{
		public PostRepositoryAsync(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<App.Domain.Entities.Data.Post> GetDefaultOrder(IQueryable<App.Domain.Entities.Data.Post> query)
		{
			return 
				from p in query
				orderby p.Id
				select p;
		}
	}
}