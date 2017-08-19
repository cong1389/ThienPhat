using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.GenericControl
{
	public interface IGenericControlService : IBaseService<App.Domain.Entities.GenericControl.GenericControl>, IService
	{
		App.Domain.Entities.GenericControl.GenericControl GetById(int Id);

		IEnumerable<App.Domain.Entities.GenericControl.GenericControl> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}