using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.RepositoryAsync.Post;
using App.Infra.Data.UOW.Interfaces;
using System;

namespace App.AsyncService.Post
{
	public class PostAsynService : BaseAsyncService<App.Domain.Entities.Data.Post>, IPostAsynService, IBaseAsyncService<App.Domain.Entities.Data.Post>, IService
	{
		private readonly IPostRepositoryAsync _postRepository;

		private readonly IUnitOfWorkAsync _unitOfWork;

		public PostAsynService(IPostRepositoryAsync postRepository, IUnitOfWorkAsync unitOfWork) : base(postRepository, unitOfWork)
		{
			this._unitOfWork = unitOfWork;
			this._postRepository = postRepository;
		}
	}
}