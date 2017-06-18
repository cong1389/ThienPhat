using App.Core.Common;
using App.Domain.Entities.Account;
using App.Domain.Entities.Identity;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Repository.Account;
using App.Infra.Data.UOW.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Service.Account
{
    public class UserStoreService : IUserLoginStore<IdentityUser, Guid>, IUserStore<IdentityUser, Guid>, IDisposable, IUserClaimStore<IdentityUser, Guid>, IUserRoleStore<IdentityUser, Guid>, IUserPasswordStore<IdentityUser, Guid>, IUserSecurityStampStore<IdentityUser, Guid>
    {
        private readonly IExternalLoginRepository _externalLoginRepository;

        private readonly IRoleRepository _roleRepository;

        private readonly IUnitOfWorkAsync _unitOfWork;

        private readonly IUserRepository _userRepository;

        public UserStoreService(IUnitOfWorkAsync unitOfWork, IUserRepository userRepository, IExternalLoginRepository externalLoginRepository, IRoleRepository roleRepository)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = userRepository;
            this._externalLoginRepository = externalLoginRepository;
            this._roleRepository = roleRepository;
        }

        public Task AddClaimAsync(IdentityUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            Domain.Entities.Account.Claim claim1 = new Domain.Entities.Account.Claim()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = user1
            };
            user1.Claims.Add(claim1);
            this._userRepository.Update(user1);
            return this._unitOfWork.CommitAsync();
        }

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            ExternalLogin externalLogin = new ExternalLogin()
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = user1
            };
            user1.Logins.Add(externalLogin);
            this._userRepository.Update(user1);
            return this._unitOfWork.CommitAsync();
        }

        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");
            }
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            Role role = this._roleRepository.FindByName(roleName);
            if (role == null)
            {
                throw new ArgumentException("roleName does not correspond to a Role entity.", "roleName");
            }
            user1.Roles.Add(role);
            this._userRepository.Update(user1);
            return this._unitOfWork.CommitAsync();
        }

        public Task CreateAsync(IdentityUser user)
        {
            User user1 = this.GetUser(user);
            this._userRepository.Add(user1);
            return this._unitOfWork.CommitAsync();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            User user1 = this.GetUser(user);
            this._userRepository.Delete(user1);
            return this._unitOfWork.CommitAsync();
        }

        public void Dispose()
        {
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            IdentityUser identityUser = null;
            ExternalLogin byProviderAndKey = this._externalLoginRepository.GetByProviderAndKey(login.LoginProvider, login.ProviderKey);
            if (byProviderAndKey != null)
            {
                identityUser = this.GetIdentityUser(byProviderAndKey.User);
            }
            return Task.FromResult<IdentityUser>(identityUser);
        }

        public Task<IdentityUser> FindByIdAsync(Guid userId)
        {
            User user = this._userRepository.FindById(userId, false);
            return Task.FromResult<IdentityUser>(this.GetIdentityUser(user));
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            User user = this._userRepository.FindByUserName(userName);
            return Task.FromResult<IdentityUser>(this.GetIdentityUser(user));
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(IdentityUser user)
        {
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            Task<IList<System.Security.Claims.Claim>> task = Task.FromResult<IList<System.Security.Claims.Claim>>((
                from x in user1.Claims
                select new System.Security.Claims.Claim(x.ClaimType, x.ClaimValue)).ToList<System.Security.Claims.Claim>());
            return task;
        }

        private IdentityUser GetIdentityUser(User user)
        {
            IdentityUser identityUser;
            if (user != null)
            {
                IdentityUser identityUser1 = new IdentityUser();
                this.PopulateIdentityUser(identityUser1, user);
                identityUser = identityUser1;
            }
            else
            {
                identityUser = null;
            }
            return identityUser;
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            Task<IList<UserLoginInfo>> task = Task.FromResult<IList<UserLoginInfo>>((
                from x in user1.Logins
                select new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList<UserLoginInfo>());
            return task;
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            Task<IList<string>> task = Task.FromResult<IList<string>>((
                from x in user1.Roles
                select x.Name).ToList<string>());
            return task;
        }

        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            return Task.FromResult<string>(user.SecurityStamp);
        }

        private User GetUser(IdentityUser identityUser)
        {
            User user = new User();
            this.PopulateUser(user, identityUser);
            return user;
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            Task<bool> task = Task.FromResult<bool>(!string.IsNullOrWhiteSpace(user.PasswordHash));
            return task;
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");
            }
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            Task<bool> task = Task.FromResult<bool>(user1.Roles.Any<Role>((Role x) => x.Name == roleName));
            return task;
        }

        private void PopulateIdentityUser(IdentityUser identityUser, User user)
        {
            identityUser.Id = user.Id;
            identityUser.UserName = user.UserName;
            identityUser.PasswordHash = user.PasswordHash;
            identityUser.SecurityStamp = user.SecurityStamp;
            identityUser.Email = user.Email;
            identityUser.FirstName = user.FirstName;
            identityUser.MiddleName = user.MiddleName;
            identityUser.LastName = user.LastName;
            identityUser.State = user.State;
            identityUser.City = user.City;
            identityUser.Phone = user.Phone;
            identityUser.Address = user.Address;
            identityUser.IsSuperAdmin = user.IsSuperAdmin;
            identityUser.IsLockedOut = user.IsLockedOut;
            identityUser.Created = user.Created;
        }

        private void PopulateUser(User user, IdentityUser identityUser)
        {
            user.Id = identityUser.Id;
            user.UserName = identityUser.UserName;
            user.PasswordHash = identityUser.PasswordHash;
            user.SecurityStamp = identityUser.SecurityStamp;
            user.Email = identityUser.Email;
            user.FirstName = identityUser.FirstName;
            user.MiddleName = identityUser.MiddleName;
            user.LastName = identityUser.LastName;
            user.State = identityUser.State;
            user.City = identityUser.City;
            user.Phone = identityUser.Phone;
            user.Address = identityUser.Address;
            user.IsSuperAdmin = identityUser.IsSuperAdmin;
            user.IsLockedOut = identityUser.IsLockedOut;
            user.Created = identityUser.Created;
        }

        public Task RemoveClaimAsync(IdentityUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            Domain.Entities.Account.Claim claim1 = user1.Claims.FirstOrDefault<Domain.Entities.Account.Claim>((Domain.Entities.Account.Claim x) => (x.ClaimType != claim.Type ? false : x.ClaimValue == claim.Value));
            user1.Claims.Remove(claim1);
            this._userRepository.Update(user1);
            return this._unitOfWork.CommitAsync();
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");
            }
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            Role role = user1.Roles.FirstOrDefault<Role>((Role x) => x.Name == roleName);
            user1.Roles.Remove(role);
            this._userRepository.Update(user1);
            return this._unitOfWork.CommitAsync();
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            ExternalLogin externalLogin = user1.Logins.FirstOrDefault<ExternalLogin>((ExternalLogin x) => (x.LoginProvider != login.LoginProvider ? false : x.ProviderKey == login.ProviderKey));
            user1.Logins.Remove(externalLogin);
            this._userRepository.Update(user1);
            return this._unitOfWork.CommitAsync();
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<int>(0);
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult<int>(0);
        }

        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("user");
            }
            User user1 = this._userRepository.FindById(user.Id, false);
            if (user1 == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }
            this.PopulateUser(user1, user);
            this._userRepository.Update(user1);
            return this._unitOfWork.CommitAsync();
        }
    }
}