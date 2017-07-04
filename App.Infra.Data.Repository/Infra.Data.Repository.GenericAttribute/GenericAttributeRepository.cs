using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.GenericAttribute
{
    public class GenericAttributeRepository : RepositoryBase<App.Domain.Entities.Data.GenericAttribute>, IGenericAttributeRepository, IRepositoryBase<App.Domain.Entities.Data.GenericAttribute>
	{
		public GenericAttributeRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

        public App.Domain.Entities.Data.GenericAttribute GetAttributeById(int Id)
        {
            App.Domain.Entities.Data.GenericAttribute attribute = this.FindBy((App.Domain.Entities.Data.GenericAttribute x) => x.Id == Id, false).FirstOrDefault<App.Domain.Entities.Data.GenericAttribute>();
            return attribute;
        }

        protected override IOrderedQueryable<App.Domain.Entities.Data.GenericAttribute> GetDefaultOrder(IQueryable<App.Domain.Entities.Data.GenericAttribute> query)
        {
            IOrderedQueryable<App.Domain.Entities.Data.GenericAttribute> attributes =
                from p in query
                orderby p.Id
                select p;
            return attributes;
        }

        public IEnumerable<App.Domain.Entities.Data.GenericAttribute> PagedList(Paging page)
        {
            return this.GetAllPagedList(page).ToList<App.Domain.Entities.Data.GenericAttribute>();
        }

        public IEnumerable<App.Domain.Entities.Data.GenericAttribute> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
        {
            Expression<Func<App.Domain.Entities.Data.GenericAttribute, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Data.GenericAttribute>();
            if (!string.IsNullOrEmpty(sortBuider.Keywords))
            {
                expression = expression.And<App.Domain.Entities.Data.GenericAttribute>((App.Domain.Entities.Data.GenericAttribute x) => x.Key.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Value.ToLower().Contains(sortBuider.Keywords.ToLower()));
            }
            return this.FindAndSort(expression, sortBuider.Sorts, page);
        }
    }
}