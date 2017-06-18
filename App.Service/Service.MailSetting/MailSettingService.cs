using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.MailSetting;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.MailSetting
{
	public class MailSettingService : BaseService<ServerMailSetting>, IMailSettingService, IBaseService<ServerMailSetting>, IService
	{
		private readonly IMailSettingRepository _mailSettingRepository;

		private readonly IUnitOfWork _unitOfWork;

		public MailSettingService(IUnitOfWork unitOfWork, IMailSettingRepository mailSettingRepository) : base(unitOfWork, mailSettingRepository)
		{
			this._unitOfWork = unitOfWork;
			this._mailSettingRepository = mailSettingRepository;
		}

		public ServerMailSetting GetById(int Id)
		{
			return this._mailSettingRepository.GetById(Id);
		}

		public IEnumerable<ServerMailSetting> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._mailSettingRepository.PagedSearchList(sortbuBuilder, page);
		}

		public int Save()
		{
			return this._unitOfWork.Commit();
		}
	}
}