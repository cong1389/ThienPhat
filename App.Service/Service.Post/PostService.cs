using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Post;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Service.Post
{
	public class PostService : BaseService<App.Domain.Entities.Data.Post>, IPostService, IBaseService<App.Domain.Entities.Data.Post>, IService
	{
		private readonly IPostRepository _postRepository;

		private readonly IUnitOfWork _unitOfWork;

		public PostService(IUnitOfWork unitOfWork, IPostRepository postRepository) : base(unitOfWork, postRepository)
		{
			this._unitOfWork = unitOfWork;
			this._postRepository = postRepository;
		}

		public App.Domain.Entities.Data.Post GetById(int Id)
		{
			return this._postRepository.GetById(Id);
		}

		public IEnumerable<App.Domain.Entities.Data.Post> GetBySeoUrl(string seoUrl)
		{
			IEnumerable<App.Domain.Entities.Data.Post> posts = this._postRepository.FindBy((App.Domain.Entities.Data.Post x) => x.SeoUrl.Equals(seoUrl), false);
			return posts;
		}

		public IEnumerable<App.Domain.Entities.Data.Post> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._postRepository.PagedSearchList(sortbuBuilder, page);
		}

		public IEnumerable<App.Domain.Entities.Data.Post> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page)
		{
			return this._postRepository.PagedSearchListByMenu(sortBuider, page);
		}
	}
}