using App.Infra.Data.DbFactory;
using App.Infra.Data.UOW;
using App.Infra.Data.UOW.Interfaces;
using App.UnitOfWork.UOW;
using Autofac;
using Autofac.Builder;
using System;

namespace App.Framework.Ioc
{
	public class EFModule : Module
	{
		public EFModule()
		{
		}

		protected override void Load(ContainerBuilder builder)
		{
            builder.RegisterType<App.UnitOfWork.UOW.UnitOfWork>().As<IUnitOfWork>().InstancePerRequest<App.UnitOfWork.UOW.UnitOfWork, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<UnitOfWorkAsync>().As<IUnitOfWorkAsync>().InstancePerRequest<UnitOfWorkAsync, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest<DbFactory, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new object[0]);
        }
	}
}