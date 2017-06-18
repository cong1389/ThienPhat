using App.Domain.Entities.Account;
using App.Domain.Interfaces.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Infra.Data.Repository.Account
{
	public interface IExternalLoginRepository : IRepositoryBaseAsync<ExternalLogin>
	{
		ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey);

		Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey);

		Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider, string providerKey);
	}
}