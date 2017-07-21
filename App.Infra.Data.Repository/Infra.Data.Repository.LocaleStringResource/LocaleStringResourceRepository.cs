using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.LocaleStringResource
{
    public class LocaleStringResourceRepository : RepositoryBase<App.Domain.Entities.Language.LocaleStringResource>, ILocaleStringResourceRepository, IRepositoryBase<App.Domain.Entities.Language.LocaleStringResource>
	{
		public LocaleStringResourceRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

        public App.Domain.Entities.Language.LocaleStringResource GetLocaleStringResourceById(int Id)
        {
            App.Domain.Entities.Language.LocaleStringResource attribute = this.FindBy((App.Domain.Entities.Language.LocaleStringResource x) => x.Id == Id, false).FirstOrDefault<App.Domain.Entities.Language.LocaleStringResource>();
            return attribute;
        }

        protected override IOrderedQueryable<App.Domain.Entities.Language.LocaleStringResource> GetDefaultOrder(IQueryable<App.Domain.Entities.Language.LocaleStringResource> query)
        {
            IOrderedQueryable<App.Domain.Entities.Language.LocaleStringResource> attributes =
                from p in query
                orderby p.Id
                select p;
            return attributes;
        }

        public IEnumerable<App.Domain.Entities.Language.LocaleStringResource> PagedList(Paging page)
        {
            return this.GetAllPagedList(page).ToList<App.Domain.Entities.Language.LocaleStringResource>();
        }

        public IEnumerable<App.Domain.Entities.Language.LocaleStringResource> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
        {
            Expression<Func<App.Domain.Entities.Language.LocaleStringResource, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Language.LocaleStringResource>();
            if (!string.IsNullOrEmpty(sortBuider.Keywords))
            {
                expression = expression.And<App.Domain.Entities.Language.LocaleStringResource>((App.Domain.Entities.Language.LocaleStringResource x) => x.ResourceName.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.ResourceName.ToLower().Contains(sortBuider.Keywords.ToLower()));
            }
            return this.FindAndSort(expression, sortBuider.Sorts, page);
        }
    }
}