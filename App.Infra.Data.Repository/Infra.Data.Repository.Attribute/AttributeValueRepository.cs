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
	public class AttributeValueRepository : RepositoryBase<AttributeValue>, IAttributeValueRepository, IRepositoryBase<AttributeValue>
	{
		public AttributeValueRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public AttributeValue GetById(int Id)
		{
			AttributeValue attributeValue = this.FindBy((AttributeValue x) => x.Id == Id, false).FirstOrDefault<AttributeValue>();
			return attributeValue;
		}

		protected override IOrderedQueryable<AttributeValue> GetDefaultOrder(IQueryable<AttributeValue> query)
		{
			IOrderedQueryable<AttributeValue> attributeValues = 
				from p in query
				orderby p.Id
				select p;
			return attributeValues;
		}

		public IEnumerable<AttributeValue> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<AttributeValue>();
		}

		public IEnumerable<AttributeValue> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<AttributeValue, bool>> expression = PredicateBuilder.True<AttributeValue>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<AttributeValue>((AttributeValue x) => x.ValueName.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Attribute.AttributeName.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}