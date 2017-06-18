using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.SeoSetting
{
	public interface ISettingSeoGlobalService : IBaseService<SettingSeoGlobal>, IService
	{
		SettingSeoGlobal GetById(int Id);

		IEnumerable<SettingSeoGlobal> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}