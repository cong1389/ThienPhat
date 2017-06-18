using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.ContactInformation
{
	public interface IContactInfoRepository : IRepositoryBase<ContactInfomation>
	{
		ContactInfomation GetById(int Id);

		IEnumerable<ContactInfomation> PagedList(Paging page);

		IEnumerable<ContactInfomation> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}