using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Attribute
{
	public interface IAttributeValueService : IBaseService<AttributeValue>, IService
	{
		AttributeValue GetById(int Id);

		IEnumerable<AttributeValue> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}