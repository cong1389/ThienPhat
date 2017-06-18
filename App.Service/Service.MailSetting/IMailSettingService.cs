using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.MailSetting
{
	public interface IMailSettingService : IBaseService<ServerMailSetting>, IService
	{
		ServerMailSetting GetById(int Id);

		IEnumerable<ServerMailSetting> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}