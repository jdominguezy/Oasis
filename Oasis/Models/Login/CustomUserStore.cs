//using Microsoft.AspNet.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.Web;

//namespace Oasis.Models.Login
//{
//    public class CustomUserStore<TUser> : IUserLoginStore<TUser>,
//        IUserClaimStore<TUser>,
//        IUserRoleStore<TUser>,
//        IUserPasswordStore<TUser>,
//        IUserSecurityStampStore<TUser>,
//        IQueryableUserStore<TUser>,
//        IUserEmailStore<TUser>,
//        IUserPhoneNumberStore<TUser>,
//        IUserTwoFactorStore<TUser, string>,
//        IUserStore<TUser>,
//        ICustomUserStore<TUser>,
//        IUserLockoutStore<TUser,string>
//        where TUser : usuarioOasis
//    {
//        private UserTable<TUser> userTable;
//        private roles roleTable;
//        private detalle_roles_usuario userRolesTable;
//        private bool _disposed;
        
//        //private usuarioOasis<TUser> userTable;
//        //private RoleTable roleTable;
//        //private UserRolesTable userRolesTable;
//        //private UserClaimsTable userClaimsTable;
//        //private UserLoginsTable userLoginsTable;
//        public as2oasis Database { get; private set; }


//        public CustomUserStore()
//        {
//            new CustomUserStore<TUser>(new as2oasis());
//        }

//        public CustomUserStore(as2oasis db)
//        {
//            Database = db;
//            userTable = new UserTable<TUser>(db);
//        }

//        public IQueryable<TUser> Users => throw new NotImplementedException();

//        public Task AddClaimAsync(TUser user, Claim claim)
//        {
//            throw new NotImplementedException();
//        }

//        public Task AddLoginAsync(TUser user, UserLoginInfo login)
//        {
//            throw new NotImplementedException();
//        }

//        public Task AddToRoleAsync(TUser user, string roleName)
//        {
//            throw new NotImplementedException();
//        }

//        public Task CreateAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task DeleteAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        //public override void Dispose()
//        //{
//        //    base.Dispose();
//        //    //throw new NotImplementedException();
//        //}

//        public Task<TUser> FindAsync(UserLoginInfo login)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<TUser> FindByEmailAsync(string email)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<TUser> FindByIdAsync(string userId)
//        {
//            return Task.FromResult<TUser>(userTable.GetUserById(userId));
//        }

//        public Task<TUser> FindByNameAsync(string userName)
//        {
//            if (string.IsNullOrEmpty(userName))
//            {
//                throw new ArgumentException("Null or empty argument: userName");
//            }

//            List<TUser> result = userTable.GetUserByName(userName) as List<TUser>;

//            // Should I throw if > 1 user?
//            if (result != null && result.Count == 1)
//            {
//                result[0].Id = result[0].id_usuario.ToString();
//                return Task.FromResult<TUser>(result[0]);
//            }

//            return Task.FromResult<TUser>(null);

//        }

//        public Task<int> GetAccessFailedCountAsync(TUser user)
//        {
//            this.ThrowIfDisposed();
//            if (user == null)
//                throw new ArgumentNullException("user");

//            return Task.FromResult(user.AccessFailedCount);
//        }

//        public Task<IList<Claim>> GetClaimsAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<string> GetEmailAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> GetEmailConfirmedAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }


//        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<string> GetPasswordHashAsync(TUser user)
//        {
//            string passwordHash = userTable.GetPasswordHash(user.id_usuario);

//            return Task.FromResult<string>(passwordHash);
//        }

//        public Task<string> GetPhoneNumberAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<IList<string>> GetRolesAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<string> GetSecurityStampAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
//        {
//            this.ThrowIfDisposed();
//            if (user == null)
//                throw new ArgumentNullException("user");

//            return Task.FromResult(false);
//        }

//        public Task<bool> HasPasswordAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<int> IncrementAccessFailedCountAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> IsInRoleAsync(TUser user, string roleName)
//        {
//            throw new NotImplementedException();
//        }

//        public Task RemoveClaimAsync(TUser user, Claim claim)
//        {
//            throw new NotImplementedException();
//        }

//        public Task RemoveFromRoleAsync(TUser user, string roleName)
//        {
//            throw new NotImplementedException();
//        }

//        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
//        {
//            throw new NotImplementedException();
//        }

//        public Task ResetAccessFailedCountAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetEmailAsync(TUser user, string email)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetPasswordHashAsync(TUser user, string passwordHash)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetSecurityStampAsync(TUser user, string stamp)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
//        {
//            throw new NotImplementedException();
//        }

//        public Task UpdateAsync(TUser user)
//        {
//            throw new NotImplementedException();
//        }

//        public void Dispose()
//        {
//            this._disposed = true;
//        }

//        public Task<bool> GetLockoutEnabledAsync(TUser user)
//        {
//            this.ThrowIfDisposed();
//            if (user == null)
//                throw new ArgumentNullException("user");

//            return Task.FromResult(user.LockoutEnabled);
//        }

//        private void ThrowIfDisposed()
//        {
//            if (this._disposed)
//                throw new ObjectDisposedException(this.GetType().Name);
//        }
//    }
//}