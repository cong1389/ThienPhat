using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.News;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Service.News
{
	public class NewsService : BaseService<App.Domain.Entities.Data.News>, INewsService, IBaseService<App.Domain.Entities.Data.News>, IService
	{
		private readonly INewsRepository _newsRepository;

		private readonly IUnitOfWork _unitOfWork;

		public NewsService(IUnitOfWork unitOfWork, INewsRepository newsRepository) : base(unitOfWork, newsRepository)
		{
			this._unitOfWork = unitOfWork;
			this._newsRepository = newsRepository;
		}

		public App.Domain.Entities.Data.News GetById(int Id)
		{
			return this._newsRepository.GetById(Id);
		}

		public IEnumerable<App.Domain.Entities.Data.News> GetBySeoUrl(string seoUrl)
		{
			IEnumerable<App.Domain.Entities.Data.News> news = this._newsRepository.FindBy((App.Domain.Entities.Data.News x) => x.SeoUrl.Equals(seoUrl), false);
			return news;
		}

		public IEnumerable<App.Domain.Entities.Data.News> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._newsRepository.PagedSearchList(sortbuBuilder, page);
		}

		public IEnumerable<App.Domain.Entities.Data.News> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page)
		{
			return this._newsRepository.PagedSearchListByMenu(sortBuider, page);
		}
	}
}