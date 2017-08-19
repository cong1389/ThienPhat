using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.GenericControl
{
	public interface IGenericControlRepository : IRepositoryBase<App.Domain.Entities.GenericControl.GenericControl>
	{
		App.Domain.Entities.GenericControl.GenericControl GetById(int Id);

		IEnumerable<App.Domain.Entities.GenericControl.GenericControl> PagedList(Paging page);

		IEnumerable<App.Domain.Entities.GenericControl.GenericControl> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}