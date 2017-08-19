using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.GenericControl
{
	public interface IGenericControlValueService : IBaseService<GenericControlValue>, IService
	{
		GenericControlValue GetById(int Id);

		IEnumerable<GenericControlValue> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}