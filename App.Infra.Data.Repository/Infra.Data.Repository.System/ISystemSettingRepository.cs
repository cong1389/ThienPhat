using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.System
{
	public interface ISystemSettingRepository : IRepositoryBase<SystemSetting>
	{
		SystemSetting GetById(int Id);

		IEnumerable<SystemSetting> PagedList(Paging page);

		IEnumerable<SystemSetting> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}