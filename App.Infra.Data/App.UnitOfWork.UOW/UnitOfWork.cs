using App.Infra.Data.Context;
using App.Infra.Data.DbFactory;
using App.Infra.Data.UOW.Interfaces;
using System;

namespace App.UnitOfWork.UOW
{
	public class UnitOfWork : IUnitOfWork
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

		public UnitOfWork(IDbFactory dbFactory)
		{
			this._dbFactory = dbFactory;
		}

		public int Commit()
		{
			return this.DbContext.Commit();
		}
	}
}