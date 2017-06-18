using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.System;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.SystemApp
{
    public class SystemSettingService : BaseService<SystemSetting>, ISystemSettingService, IBaseService<SystemSetting>, IService
    {
        private readonly ISystemSettingRepository _systemSettingRepository;

        private readonly IUnitOfWork _unitOfWork;

        public SystemSettingService(IUnitOfWork unitOfWork, ISystemSettingRepository systemSettingRepository) : base(unitOfWork, systemSettingRepository)
        {
            this._unitOfWork = unitOfWork;
            this._systemSettingRepository = systemSettingRepository;
        }

        public SystemSetting GetById(int Id)
        {
            return this._systemSettingRepository.GetById(Id); 
        }

        public IEnumerable<SystemSetting> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._systemSettingRepository.PagedSearchList(sortbuBuilder, page);
        }
    }
}
