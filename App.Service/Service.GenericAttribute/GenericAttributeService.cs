using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.GenericAttribute;
using App.Infra.Data.UOW.Interfaces;
using App.Service.GenericAttribute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Service.GenericAttribute
{
    public class GenericAttributeService : BaseService<App.Domain.Entities.Data.GenericAttribute>, IGenericAttributeService, IBaseService<App.Domain.Entities.Data.GenericAttribute>, IService
    {
        private readonly IGenericAttributeRepository _genericAttributeRepository;

        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        private readonly IUnitOfWork _unitOfWork;

        public GenericAttributeService(IUnitOfWork unitOfWork, IGenericAttributeRepository genericAttributeRepository) : base(unitOfWork, genericAttributeRepository)
        {
            this._unitOfWork = unitOfWork;
            this._genericAttributeRepository = genericAttributeRepository;
        }

        public void CreateGenericAttribute(App.Domain.Entities.Data.GenericAttribute genericAttribute)
        {
            this._genericAttributeRepository.Add(genericAttribute);
        }

        public App.Domain.Entities.Data.GenericAttribute GetGenericAttributeById(int Id)
        {
            return this._genericAttributeRepository.GetAttributeById(Id);
        }

        public App.Domain.Entities.Data.GenericAttribute GetGenericAttributeByKey(int entityId, string keyGroup, string key)
        {
            App.Domain.Entities.Data.GenericAttribute attr = this._genericAttributeRepository
                .Get((App.Domain.Entities.Data.GenericAttribute x) =>
                x.EntityId.Equals(entityId)
                 && x.KeyGroup.Equals(keyGroup)
                 && x.Key.Equals(key)
                , false);
            return attr;
        }

        public IEnumerable<App.Domain.Entities.Data.GenericAttribute> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._genericAttributeRepository.PagedSearchList(sortbuBuilder, page);
        }
              
        public int SaveGenericAttribute()
        {
            return this._unitOfWork.Commit();
        }
    }
}