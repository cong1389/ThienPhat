using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.SeoSetting
{
	public interface ISettingSeoGlobalRepository : IRepositoryBase<SettingSeoGlobal>
	{
		SettingSeoGlobal GetById(int Id);

		IEnumerable<SettingSeoGlobal> PagedList(Paging page);

		IEnumerable<SettingSeoGlobal> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}