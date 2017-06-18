using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Static;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Service.Static
{
	public class StaticContentService : BaseService<StaticContent>, IStaticContentService, IBaseService<StaticContent>, IService
	{
		private readonly IStaticContentRepository _staticContentRepository;

		private readonly IUnitOfWork _unitOfWork;

		public StaticContentService(IUnitOfWork unitOfWork, IStaticContentRepository staticContentRepository) : base(unitOfWork, staticContentRepository)
		{
			this._unitOfWork = unitOfWork;
			this._staticContentRepository = staticContentRepository;
		}

		public StaticContent GetById(int Id)
		{
			return this._staticContentRepository.GetById(Id);
		}

		public IEnumerable<StaticContent> GetBySeoUrl(string seoUrl)
		{
			IEnumerable<StaticContent> staticContents = this._staticContentRepository.FindBy((StaticContent x) => x.SeoUrl.Equals(seoUrl), false);
			return staticContents;
		}

		public IEnumerable<StaticContent> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._staticContentRepository.PagedSearchList(sortbuBuilder, page);
		}

		public IEnumerable<StaticContent> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page)
		{
			return this._staticContentRepository.PagedSearchListByMenu(sortBuider, page);
		}
	}
}