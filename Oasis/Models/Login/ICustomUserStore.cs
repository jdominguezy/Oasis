using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Oasis.Models.Login
{
    public interface ICustomUserStore<TUser> where TUser : usuarioOasis
    {
        as2oasis Database { get; }
        IQueryable<TUser> Users { get; }

        Task AddClaimAsync(TUser user, Claim claim);
        Task AddLoginAsync(TUser user, UserLoginInfo login);
        Task AddToRoleAsync(TUser user, string roleName);
        Task CreateAsync(TUser user);
        Task DeleteAsync(TUser user);
        void Dispose();
        Task<TUser> FindAsync(UserLoginInfo login);
        Task<TUser> FindByEmailAsync(string email);
        Task<TUser> FindByIdAsync(string userId);
        Task<TUser> FindByNameAsync(string userName);
        Task<int> GetAccessFailedCountAsync(TUser user);
        Task<IList<Claim>> GetClaimsAsync(TUser user);
        Task<string> GetEmailAsync(TUser user);
        Task<bool> GetEmailConfirmedAsync(TUser user);
        Task<bool> GetLockoutEnabledAsync(TUser user);
        Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user);
        Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user);
        Task<string> GetPasswordHashAsync(TUser user);
        Task<string> GetPhoneNumberAsync(TUser user);
        Task<bool> GetPhoneNumberConfirmedAsync(TUser user);
        Task<IList<string>> GetRolesAsync(TUser user);
        Task<string> GetSecurityStampAsync(TUser user);
        Task<bool> GetTwoFactorEnabledAsync(TUser user);
        Task<bool> HasPasswordAsync(TUser user);
        Task<int> IncrementAccessFailedCountAsync(TUser user);
        Task<bool> IsInRoleAsync(TUser user, string roleName);
        Task RemoveClaimAsync(TUser user, Claim claim);
        Task RemoveFromRoleAsync(TUser user, string roleName);
        Task RemoveLoginAsync(TUser user, UserLoginInfo login);
        Task ResetAccessFailedCountAsync(TUser user);
        Task SetEmailAsync(TUser user, string email);
        Task SetEmailConfirmedAsync(TUser user, bool confirmed);
        Task SetLockoutEnabledAsync(TUser user, bool enabled);
        Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd);
        Task SetPasswordHashAsync(TUser user, string passwordHash);
        Task SetPhoneNumberAsync(TUser user, string phoneNumber);
        Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed);
        Task SetSecurityStampAsync(TUser user, string stamp);
        Task SetTwoFactorEnabledAsync(TUser user, bool enabled);
        Task UpdateAsync(TUser user);
    }
}