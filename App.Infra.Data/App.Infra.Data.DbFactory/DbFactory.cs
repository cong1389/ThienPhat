using App.Infra.Data.Context;
using System;
using System.Data.Entity;

namespace App.Infra.Data.DbFactory
{
	public class DbFactory : Disposable, IDbFactory, IDisposable
	{
		private App.Infra.Data.Context.AppContext _dbContext;

		public DbFactory()
		{
		}

		protected override void DisposeCore()
		{
			if (this._dbContext != null)
			{
				this._dbContext.Dispose();
			}
		}

		public App.Infra.Data.Context.AppContext Init()
		{
			App.Infra.Data.Context.AppContext appContext = this._dbContext;
			if (appContext == null)
			{
				App.Infra.Data.Context.AppContext appContext1 = new App.Infra.Data.Context.AppContext();
				App.Infra.Data.Context.AppContext appContext2 = appContext1;
				this._dbContext = appContext1;
				appContext = appContext2;
			}
			return appContext;
		}
	}
}