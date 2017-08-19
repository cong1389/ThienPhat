using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.GenericControl
{
	public interface IGenericControlValueRepository : IRepositoryBase<GenericControlValue>
	{
		GenericControlValue GetById(int Id);

		IEnumerable<GenericControlValue> PagedList(Paging page);

		IEnumerable<GenericControlValue> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}