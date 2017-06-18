using App.Framework.FluentValidation;
using App.Framework.Ioc;
using App.Framework.Mappings;
using Autofac;
using Autofac.Builder;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FluentValidation.Mvc;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;

public class Bootstrapper
{
	public Bootstrapper()
	{
	}

	public static void Run()
	{
		Bootstrapper.SetAutofacContainer();
		AutoMapperConfiguration.Configure();
		FluentValidationModelValidatorProvider.Configure((FluentValidationModelValidatorProvider provider) => provider.ValidatorFactory = new FluentValidationConfig());
	}

	private static void SetAutofacContainer()
	{
		ContainerBuilder containerBuilder = new ContainerBuilder();
		Assembly[] array = (
			from Assembly p in BuildManager.GetReferencedAssemblies()
			where p.ManifestModule.Name.StartsWith("App.")
			select p).ToArray<Assembly>();

		containerBuilder.RegisterControllers(array);
		containerBuilder.RegisterAssemblyModules(array);
		containerBuilder.RegisterApiControllers(array);
		containerBuilder.RegisterFilterProvider();
		containerBuilder.RegisterSource(new ViewRegistrationSource());
		containerBuilder.RegisterModule(new EFModule());
		containerBuilder.RegisterModule(new RepositoryModule());
		containerBuilder.RegisterModule(new IdentityModule());
		containerBuilder.RegisterModule(new ServiceModule());
		IContainer container = containerBuilder.Build(ContainerBuildOptions.IgnoreStartableComponents);
        //GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        DependencyResolver.SetResolver((IDependencyResolver)(new AutofacDependencyResolver(container)));
	}
}