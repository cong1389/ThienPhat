using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.GenericAttribute
{
	public interface IGenericAttributeRepository : IRepositoryBase<App.Domain.Entities.Data.GenericAttribute>
	{
		App.Domain.Entities.Data.GenericAttribute GetAttributeById(int Id);

		IEnumerable<App.Domain.Entities.Data.GenericAttribute> PagedList(Paging page);

		IEnumerable<App.Domain.Entities.Data.GenericAttribute> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}