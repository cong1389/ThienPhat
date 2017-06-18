using App.Domain.Common;
using App.Domain.Entities.Account;
using App.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppContext>
    {
        public Configuration()
        {
            base.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AppContext context)
        {
            IEnumerable<string> constantsValues = typeof(ApplicationRoles).GetConstantsValues<string>();
            if ((constantsValues == null ? false : constantsValues.Any<string>()))
            {
                foreach (string constantsValue in constantsValues)
                {
                    if (context.Roles.FirstOrDefault<Role>((Role x) => x.Name == constantsValue) == null)
                    {
                        context.Roles.AddOrUpdate<Role>(new Role[] { new Role()
                        {
                            Id = Guid.NewGuid(),
                            Name = constantsValue
                        } });
                    }
                }
            }
        }
    }
}