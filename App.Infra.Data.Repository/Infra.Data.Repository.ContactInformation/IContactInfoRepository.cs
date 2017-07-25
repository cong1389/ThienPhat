using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.ContactInformation
{
	public interface IContactInfoRepository : IRepositoryBase<Domain.Entities.GlobalSetting.ContactInformation>
	{
        Domain.Entities.GlobalSetting.ContactInformation GetById(int Id);

		IEnumerable<Domain.Entities.GlobalSetting.ContactInformation> PagedList(Paging page);

		IEnumerable<Domain.Entities.GlobalSetting.ContactInformation> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}