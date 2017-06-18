using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Language;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Language
{
	public class LanguageService : BaseService<App.Domain.Entities.Language.Language>, ILanguageService, IBaseService<App.Domain.Entities.Language.Language>, IService
	{
		private readonly ILanguageRepository _languageRepository;

		private readonly IUnitOfWork _unitOfWork;

		public LanguageService(IUnitOfWork unitOfWork, ILanguageRepository languageRepository) : base(unitOfWork, languageRepository)
		{
			this._unitOfWork = unitOfWork;
			this._languageRepository = languageRepository;
		}

		public void CreateLanguage(App.Domain.Entities.Language.Language language)
		{
			this._languageRepository.Add(language);
		}

		public App.Domain.Entities.Language.Language GetLanguageById(int Id)
		{
			return this._languageRepository.GetLanguageById(Id);
		}

		public IEnumerable<App.Domain.Entities.Language.Language> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._languageRepository.PagedSearchList(sortbuBuilder, page);
		}

		public int SaveLanguage()
		{
			return this._unitOfWork.Commit();
		}
	}
}