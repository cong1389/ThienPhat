using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.ContactInformation;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.ContactInformation
{
	public class ContactInfoService : BaseService<ContactInfomation>, IContactInfoService, IBaseService<ContactInfomation>, IService
	{
		private readonly IContactInfoRepository _contactInfoRepository;

		private readonly IUnitOfWork _unitOfWork;

		public ContactInfoService(IUnitOfWork unitOfWork, IContactInfoRepository contactInfoRepository) : base(unitOfWork, contactInfoRepository)
		{
			this._unitOfWork = unitOfWork;
			this._contactInfoRepository = contactInfoRepository;
		}

		public ContactInfomation GetById(int Id)
		{
			return this._contactInfoRepository.GetById(Id);
		}

		public IEnumerable<ContactInfomation> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._contactInfoRepository.PagedSearchList(sortbuBuilder, page);
		}

		public int Save()
		{
			return this._unitOfWork.Commit();
		}
	}
}