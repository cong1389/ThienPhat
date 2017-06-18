using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.ContactInformation
{
	public interface IContactInfoService : IBaseService<ContactInfomation>, IService
	{
		ContactInfomation GetById(int Id);

		IEnumerable<ContactInfomation> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}