using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.MailSetting
{
	public interface IMailSettingRepository : IRepositoryBase<ServerMailSetting>
	{
		ServerMailSetting GetById(int Id);

		IEnumerable<ServerMailSetting> PagedList(Paging page);

		IEnumerable<ServerMailSetting> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}