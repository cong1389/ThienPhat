using App.Infra.Data.Context;
using App.Infra.Data.DbFactory;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Infra.Data.UOW
{
	public class UnitOfWorkAsync : IUnitOfWorkAsync
	{
		private readonly IDbFactory _dbFactory;

		private App.Infra.Data.Context.AppContext _dbContext;

		public App.Infra.Data.Context.AppContext DbContext
		{
			get
			{
				App.Infra.Data.Context.AppContext appContext = this._dbContext;
				if (appContext == null)
				{
					App.Infra.Data.Context.AppContext appContext1 = this._dbFactory.Init();
					App.Infra.Data.Context.AppContext appContext2 = appContext1;
					this._dbContext = appContext1;
					appContext = appContext2;
				}
				return appContext;
			}
		}

		public UnitOfWorkAsync(IDbFactory dbFactory)
		{
			this._dbFactory = dbFactory;
		}

		public Task<int> CommitAsync()
		{
			return this.DbContext.CommitAsync();
		}

		public Task<int> CommitAsync(CancellationToken cancellationToken)
		{
			return this.DbContext.CommitAsync();
		}
	}
}