using App.Core.Common;
using App.Domain.Entities.Account;
using App.Domain.Entities.Identity;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Repository.Account;
using App.Infra.Data.UOW.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace App.Service.Account
{
	public class RoleStoreService : IRoleStore<IdentityRole, Guid>, IDisposable, IQueryableRoleStore<IdentityRole, Guid>
	{
		private readonly IRoleRepository _roleRepository;

		private readonly IUnitOfWorkAsync _unitOfWork;

		public IQueryable<IdentityRole> Roles
		{
			get
			{
				IQueryable<IdentityRole> identityRoles = (
					from x in this._roleRepository.GetAll()
					select this.GetIdentityRole(x)).AsQueryable<IdentityRole>();
				return identityRoles;
			}
		}

		public RoleStoreService(IUnitOfWorkAsync unitOfWork, IRoleRepository roleRepository)
		{
			this._unitOfWork = unitOfWork;
			this._roleRepository = roleRepository;
		}

		public Task CreateAsync(IdentityRole role)
		{
			if (role == null)
			{
				throw new ArgumentNullException("role");
			}
			Role role1 = this.getRole(role);
			this._roleRepository.Add(role1);
			return this._unitOfWork.CommitAsync();
		}

		public Task DeleteAsync(IdentityRole role)
		{
			if (role == null)
			{
				throw new ArgumentNullException("role");
			}
			Role role1 = this.getRole(role);
			this._roleRepository.Delete(role1);
			return this._unitOfWork.CommitAsync();
		}

		public void Dispose()
		{
		}

		public Task<IdentityRole> FindByIdAsync(Guid roleId)
		{
			Role role = this._roleRepository.FindById(roleId, false);
			return Task.FromResult<IdentityRole>(this.GetIdentityRole(role));
		}

		public Task<IdentityRole> FindByNameAsync(string roleName)
		{
			Role role = this._roleRepository.FindByName(roleName);
			return Task.FromResult<IdentityRole>(this.GetIdentityRole(role));
		}

		private IdentityRole GetIdentityRole(Role role)
		{
			IdentityRole identityRole;
			if (role != null)
			{
				identityRole = new IdentityRole()
				{
					Id = role.Id,
					Name = role.Name,
					Description = role.Description
				};
			}
			else
			{
				identityRole = null;
			}
			return identityRole;
		}

		private Role getRole(IdentityRole identityRole)
		{
			Role role;
			if (identityRole != null)
			{
				role = new Role()
				{
					Id = identityRole.Id,
					Name = identityRole.Name,
					Description = identityRole.Description
				};
			}
			else
			{
				role = null;
			}
			return role;
		}

		public Task UpdateAsync(IdentityRole role)
		{
			if (role == null)
			{
				throw new ArgumentNullException("role");
			}
			Role role1 = this.getRole(role);
			this._roleRepository.Update(role1);
			return this._unitOfWork.CommitAsync();
		}
	}
}