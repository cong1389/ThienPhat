using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Attribute
{
	public interface IAttributeValueRepository : IRepositoryBase<AttributeValue>
	{
		AttributeValue GetById(int Id);

		IEnumerable<AttributeValue> PagedList(Paging page);

		IEnumerable<AttributeValue> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}