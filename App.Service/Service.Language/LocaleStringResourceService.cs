using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.LocaleStringResource;
using App.Infra.Data.UOW.Interfaces;
using App.Service.Common;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.LocaleStringResource
{
    public class LocaleStringResourceService : BaseService<App.Domain.Entities.Language.LocaleStringResource>, ILocaleStringResourceService, IBaseService<App.Domain.Entities.Language.LocaleStringResource>, IService
    {
        private readonly IWorkContext _workContext;

        private readonly ILocaleStringResourceRepository _LocaleStringResourceRepository;

        private readonly IUnitOfWork _unitOfWork;

        public LocaleStringResourceService(IUnitOfWork unitOfWork, ILocaleStringResourceRepository LocaleStringResourceRepository, IWorkContext workContext) : base(unitOfWork, LocaleStringResourceRepository)
        {
            this._unitOfWork = unitOfWork;
            this._LocaleStringResourceRepository = LocaleStringResourceRepository;
            this._workContext = workContext;
        }

        public void CreateLocaleStringResource(App.Domain.Entities.Language.LocaleStringResource LocaleStringResource)
        {
            this._LocaleStringResourceRepository.Add(LocaleStringResource);
        }

        public App.Domain.Entities.Language.LocaleStringResource GetLocaleStringResourceByName(int languageId, string resourceName)
        {
            return this._LocaleStringResourceRepository.Get((Domain.Entities.Language.LocaleStringResource x)
                => x.LanguageId == languageId && x.ResourceName == resourceName);
        }

        public IEnumerable<App.Domain.Entities.Language.LocaleStringResource> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._LocaleStringResourceRepository.PagedSearchList(sortbuBuilder, page);
        }

        public int SaveLocaleStringResource()
        {
            return this._unitOfWork.Commit();
        }

        public App.Domain.Entities.Language.LocaleStringResource GetById(int id)
        {
            App.Domain.Entities.Language.LocaleStringResource locale = _LocaleStringResourceRepository.GetLocaleStringResourceById(id);

            return locale;
        }

        public IEnumerable<App.Domain.Entities.Language.LocaleStringResource> GetByLanguageId(int languageId)
        {
            var locale = _LocaleStringResourceRepository.FindBy(
                (App.Domain.Entities.Language.LocaleStringResource x) =>x.LanguageId==languageId,false);

            return locale;
        }

        public virtual IQueryable<App.Domain.Entities.Language.LocaleStringResource> GetAll(int languageId)
        {
            var query = from lsr in _LocaleStringResourceRepository.Table
                        orderby lsr.ResourceName
                        where lsr.LanguageId == languageId
                        select lsr;

            return query;
        }

        public virtual string GetResource(
          string resourceKey,
          int languageId = 0,
          bool logIfNotFound = true,
          string defaultValue = "",
          bool returnEmptyIfNotFound = false)
        {
            if (languageId <= 0)
            {
                if (_workContext.WorkingLanguage == null)
                {
                    return defaultValue;
                }

                languageId = _workContext.WorkingLanguage.Id;
            }

            string result = string.Empty;

            App.Domain.Entities.Language.LocaleStringResource locale = this.GetLocaleStringResourceByName(languageId, resourceKey);
            if (locale !=null)
            {
                result = locale.ResourceValue;
            }

            return result;
        }
    }
}