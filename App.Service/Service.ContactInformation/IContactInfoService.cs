using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.ContactInformation
{
	public interface IContactInfoService : IBaseService<Domain.Entities.GlobalSetting.ContactInformation>, IService
	{
        Domain.Entities.GlobalSetting.ContactInformation GetById(int Id);

		IEnumerable<Domain.Entities.GlobalSetting.ContactInformation> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}