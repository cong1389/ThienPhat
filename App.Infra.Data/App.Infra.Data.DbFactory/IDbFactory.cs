using App.Infra.Data.Context;
using System;

namespace App.Infra.Data.DbFactory
{
	public interface IDbFactory : IDisposable
	{
		App.Infra.Data.Context.AppContext Init();
	}
}