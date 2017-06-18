using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;

namespace App.AsyncService.Post
{
	public interface IPostAsynService : IBaseAsyncService<App.Domain.Entities.Data.Post>, IService
	{

	}
}