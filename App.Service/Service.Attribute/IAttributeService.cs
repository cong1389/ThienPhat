using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Attribute
{
	public interface IAttributeService : IBaseService<App.Domain.Entities.Attribute.Attribute>, IService
	{
		App.Domain.Entities.Attribute.Attribute GetById(int Id);

		IEnumerable<App.Domain.Entities.Attribute.Attribute> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}