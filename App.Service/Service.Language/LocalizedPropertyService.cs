using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Language;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.LocalizedProperty
{
	public class LocalizedPropertyService : BaseService<App.Domain.Entities.Language.LocalizedProperty>, ILocalizedPropertyService, IBaseService<App.Domain.Entities.Language.LocalizedProperty>, IService
	{
		private readonly ILocalizedPropertyRepository _localizedPropertyRepository;

		private readonly IUnitOfWork _unitOfWork;

		public LocalizedPropertyService(IUnitOfWork unitOfWork, ILocalizedPropertyRepository LocalizedPropertyRepository) : base(unitOfWork, LocalizedPropertyRepository)
		{
			this._unitOfWork = unitOfWork;
			this._localizedPropertyRepository = LocalizedPropertyRepository;
		}

		public void CreateLocalizedProperty(App.Domain.Entities.Language.LocalizedProperty LocalizedProperty)
		{
			this._localizedPropertyRepository.Add(LocalizedProperty);
		}

		public App.Domain.Entities.Language.LocalizedProperty GetLocalizedPropertyById(int Id)
		{
			return this._localizedPropertyRepository.GetId(Id);
		}

		public IEnumerable<App.Domain.Entities.Language.LocalizedProperty> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._localizedPropertyRepository.PagedSearchList(sortbuBuilder, page);
		}

		public int SaveLocalizedProperty()
		{
			return this._unitOfWork.Commit();
		}
	}
}