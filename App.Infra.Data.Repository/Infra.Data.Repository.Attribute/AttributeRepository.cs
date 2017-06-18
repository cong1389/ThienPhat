using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Attribute
{
	public class AttributeRepository : RepositoryBase<App.Domain.Entities.Attribute.Attribute>, IAttributeRepository, IRepositoryBase<App.Domain.Entities.Attribute.Attribute>
	{
		public AttributeRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public App.Domain.Entities.Attribute.Attribute GetById(int Id)
		{
			App.Domain.Entities.Attribute.Attribute attribute = this.FindBy((App.Domain.Entities.Attribute.Attribute x) => x.Id == Id, false).FirstOrDefault<App.Domain.Entities.Attribute.Attribute>();
			return attribute;
		}

		protected override IOrderedQueryable<App.Domain.Entities.Attribute.Attribute> GetDefaultOrder(IQueryable<App.Domain.Entities.Attribute.Attribute> query)
		{
			IOrderedQueryable<App.Domain.Entities.Attribute.Attribute> attributes = 
				from p in query
				orderby p.Id
				select p;
			return attributes;
		}

		public IEnumerable<App.Domain.Entities.Attribute.Attribute> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<App.Domain.Entities.Attribute.Attribute>();
		}

		public IEnumerable<App.Domain.Entities.Attribute.Attribute> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<App.Domain.Entities.Attribute.Attribute, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Attribute.Attribute>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<App.Domain.Entities.Attribute.Attribute>((App.Domain.Entities.Attribute.Attribute x) => x.AttributeName.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}