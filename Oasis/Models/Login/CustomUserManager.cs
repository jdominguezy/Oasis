//using Microsoft.AspNet.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;

//namespace Oasis.Models.Login
//{
//    public class CustomUserManager : UserManager<usuarioOasis>
//    {
//        public CustomUserManager(ICustomUserStore<usuarioOasis> store ) : 
//            base(new CustomUserStore<usuarioOasis>(new as2oasis()))
//        {
//        }

//        //public CustomUserManager(
//        //    IUserStore<usuarioOasis> store)
//        ////: base(store)
//        //{
//        //}

//        public static CustomUserManager Create(IdentityFactoryOptions<CustomUserManager> options, IOwinContext context)
//        {
//            var manager = new CustomUserManager(new CustomUserStore<usuarioOasis>(context.Get<as2oasis>()));
//            // Configure validation logic for usernames
//            manager.UserValidator = new UserValidator<usuarioOasis>(manager)
//            {
//                AllowOnlyAlphanumericUserNames = false,
//                RequireUniqueEmail = false
//            };

//            // Configure validation logic for passwords
//            manager.PasswordValidator = new PasswordValidator
//            {
//                RequiredLength = 5,
//                RequireNonLetterOrDigit = false,
//                RequireDigit = false,
//                RequireLowercase = false,
//                RequireUppercase = false,
//            };

//            // Configure user lockout defaults
//            manager.UserLockoutEnabledByDefault = true;
//            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
//            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

//            var dataProtectionProvider = options.DataProtectionProvider;
//            if (dataProtectionProvider != null)
//            {
//                manager.UserTokenProvider =
//                    new DataProtectorTokenProvider<usuarioOasis>(dataProtectionProvider.Create("ASP.NET Identity"));
//            }
//            return manager;
//        }


//        public override bool SupportsUserTwoFactor => base.SupportsUserTwoFactor;

//        public override bool SupportsUserPassword => base.SupportsUserPassword;

//        public override bool SupportsUserSecurityStamp => base.SupportsUserSecurityStamp;

//        public override bool SupportsUserRole => base.SupportsUserRole;

//        public override bool SupportsUserLogin => base.SupportsUserLogin;

//        public override bool SupportsUserEmail => base.SupportsUserEmail;

//        public override bool SupportsUserPhoneNumber => base.SupportsUserPhoneNumber;

//        public override bool SupportsUserClaim => base.SupportsUserClaim;

//        public override bool SupportsUserLockout => base.SupportsUserLockout;

//        public override bool SupportsQueryableUsers => base.SupportsQueryableUsers;

//        public override IQueryable<usuarioOasis> Users => base.Users;

//        public override Task<IdentityResult> AccessFailedAsync(string userId)
//        {
//            return base.AccessFailedAsync(userId);
//        }

//        public override Task<IdentityResult> AddClaimAsync(string userId, Claim claim)
//        {
//            return base.AddClaimAsync(userId, claim);
//        }

//        public override Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
//        {
//            return base.AddLoginAsync(userId, login);
//        }

//        public override Task<IdentityResult> AddPasswordAsync(string userId, string password)
//        {
//            return base.AddPasswordAsync(userId, password);
//        }

//        public override Task<IdentityResult> AddToRoleAsync(string userId, string role)
//        {
//            return base.AddToRoleAsync(userId, role);
//        }

//        public override Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles)
//        {
//            return base.AddToRolesAsync(userId, roles);
//        }

//        public override Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
//        {
//            return base.ChangePasswordAsync(userId, currentPassword, newPassword);
//        }

//        public override Task<IdentityResult> ChangePhoneNumberAsync(string userId, string phoneNumber, string token)
//        {
//            return base.ChangePhoneNumberAsync(userId, phoneNumber, token);
//        }

//        public override Task<bool> CheckPasswordAsync(usuarioOasis user, string password)
//        {
//            return base.CheckPasswordAsync(user, password);
//        }

//        public override Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
//        {
//            return base.ConfirmEmailAsync(userId, token);
//        }

//        public override Task<IdentityResult> CreateAsync(usuarioOasis user)
//        {
//            return base.CreateAsync(user);
//        }

//        public override Task<IdentityResult> CreateAsync(usuarioOasis user, string password)
//        {
//            return base.CreateAsync(user, password);
//        }

//        public override Task<ClaimsIdentity> CreateIdentityAsync(usuarioOasis user, string authenticationType)
//        {
//            IList<Claim> claimCollection = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, user.username),
//                new Claim(ClaimTypes.Email, user.email)
//            };

//            var claimsIdentity = new ClaimsIdentity(claimCollection, "Oasis Portal");

//            return Task.Run(() => claimsIdentity);

//            //return base.CreateIdentityAsync(user, authenticationType);
//        }

//        public override Task<IdentityResult> DeleteAsync(usuarioOasis user)
//        {
//            return base.DeleteAsync(user);
//        }

//        public override bool Equals(object obj)
//        {
//            return base.Equals(obj);
//        }

//        public override Task<usuarioOasis> FindAsync(string userName, string password)
//        {
//            return Task.Run(() =>
//            {
//                using (var db = new as2oasis())
//                {
//                    var pwd = new LoginRequest() { Password = password };
//                    var pwd_hash = pwd.PasswordEncriptada();
//                    var usuario = db
//                        .usuario
//                        .Where(x => x.username == userName && x.password == pwd_hash)
//                        .FirstOrDefault();
//                    return usuario;
//                }
//            });
            
//        }

//        public override Task<usuarioOasis> FindAsync(UserLoginInfo login)
//        {
//            return base.FindAsync(login);
//        }

//        public override Task<usuarioOasis> FindByEmailAsync(string email)
//        {
//            return base.FindByEmailAsync(email);
//        }

//        public override Task<usuarioOasis> FindByIdAsync(string userId)
//        {
//            return base.FindByIdAsync(userId);
//        }

//        public override Task<usuarioOasis> FindByNameAsync(string userName)
//        {
//            return base.FindByNameAsync(userName);
//        }

//        public override Task<string> GenerateChangePhoneNumberTokenAsync(string userId, string phoneNumber)
//        {
//            return base.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber);
//        }

//        public override Task<string> GenerateEmailConfirmationTokenAsync(string userId)
//        {
//            return base.GenerateEmailConfirmationTokenAsync(userId);
//        }

//        public override Task<string> GeneratePasswordResetTokenAsync(string userId)
//        {
//            return base.GeneratePasswordResetTokenAsync(userId);
//        }

//        public override Task<string> GenerateTwoFactorTokenAsync(string userId, string twoFactorProvider)
//        {
//            return base.GenerateTwoFactorTokenAsync(userId, twoFactorProvider);
//        }

//        public override Task<string> GenerateUserTokenAsync(string purpose, string userId)
//        {
//            return base.GenerateUserTokenAsync(purpose, userId);
//        }

//        public override Task<int> GetAccessFailedCountAsync(string userId)
//        {
//            return base.GetAccessFailedCountAsync(userId);
//        }

//        public override Task<IList<Claim>> GetClaimsAsync(string userId)
//        {
//            return base.GetClaimsAsync(userId);
//        }

//        public override Task<string> GetEmailAsync(string userId)
//        {
//            return base.GetEmailAsync(userId);
//        }

//        public override int GetHashCode()
//        {
//            return base.GetHashCode();
//        }

//        public override Task<bool> GetLockoutEnabledAsync(string userId)
//        {
//            return base.GetLockoutEnabledAsync(userId);
//        }

//        public override Task<DateTimeOffset> GetLockoutEndDateAsync(string userId)
//        {
//            return base.GetLockoutEndDateAsync(userId);
//        }

//        public override Task<IList<UserLoginInfo>> GetLoginsAsync(string userId)
//        {
//            return base.GetLoginsAsync(userId);
//        }

//        public override Task<string> GetPhoneNumberAsync(string userId)
//        {
//            return base.GetPhoneNumberAsync(userId);
//        }

//        public override Task<IList<string>> GetRolesAsync(string userId)
//        {
//            return base.GetRolesAsync(userId);
//        }

//        public override Task<string> GetSecurityStampAsync(string userId)
//        {
//            return base.GetSecurityStampAsync(userId);
//        }

//        public override Task<bool> GetTwoFactorEnabledAsync(string userId)
//        {
//            return base.GetTwoFactorEnabledAsync(userId);
//        }

//        public override Task<IList<string>> GetValidTwoFactorProvidersAsync(string userId)
//        {
//            return base.GetValidTwoFactorProvidersAsync(userId);
//        }

//        public override Task<bool> HasPasswordAsync(string userId)
//        {
//            return base.HasPasswordAsync(userId);
//        }

//        public override Task<bool> IsEmailConfirmedAsync(string userId)
//        {
//            return base.IsEmailConfirmedAsync(userId);
//        }

//        public override Task<bool> IsInRoleAsync(string userId, string role)
//        {
//            return base.IsInRoleAsync(userId, role);
//        }

//        public override Task<bool> IsLockedOutAsync(string userId)
//        {
//            return base.IsLockedOutAsync(userId);
//        }

//        public override Task<bool> IsPhoneNumberConfirmedAsync(string userId)
//        {
//            return base.IsPhoneNumberConfirmedAsync(userId);
//        }

//        public override Task<IdentityResult> NotifyTwoFactorTokenAsync(string userId, string twoFactorProvider, string token)
//        {
//            return base.NotifyTwoFactorTokenAsync(userId, twoFactorProvider, token);
//        }

//        public override void RegisterTwoFactorProvider(string twoFactorProvider, IUserTokenProvider<usuarioOasis, string> provider)
//        {
//            base.RegisterTwoFactorProvider(twoFactorProvider, provider);
//        }

//        public override Task<IdentityResult> RemoveClaimAsync(string userId, Claim claim)
//        {
//            return base.RemoveClaimAsync(userId, claim);
//        }

//        public override Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
//        {
//            return base.RemoveFromRoleAsync(userId, role);
//        }

//        public override Task<IdentityResult> RemoveFromRolesAsync(string userId, params string[] roles)
//        {
//            return base.RemoveFromRolesAsync(userId, roles);
//        }

//        public override Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login)
//        {
//            return base.RemoveLoginAsync(userId, login);
//        }

//        public override Task<IdentityResult> RemovePasswordAsync(string userId)
//        {
//            return base.RemovePasswordAsync(userId);
//        }

//        public override Task<IdentityResult> ResetAccessFailedCountAsync(string userId)
//        {
//            return base.ResetAccessFailedCountAsync(userId);
//        }

//        public override Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
//        {
//            return base.ResetPasswordAsync(userId, token, newPassword);
//        }

//        public override Task SendEmailAsync(string userId, string subject, string body)
//        {
//            return base.SendEmailAsync(userId, subject, body);
//        }

//        public override Task SendSmsAsync(string userId, string message)
//        {
//            return base.SendSmsAsync(userId, message);
//        }

//        public override Task<IdentityResult> SetEmailAsync(string userId, string email)
//        {
//            return base.SetEmailAsync(userId, email);
//        }

//        public override Task<IdentityResult> SetLockoutEnabledAsync(string userId, bool enabled)
//        {
//            return base.SetLockoutEnabledAsync(userId, enabled);
//        }

//        public override Task<IdentityResult> SetLockoutEndDateAsync(string userId, DateTimeOffset lockoutEnd)
//        {
//            return base.SetLockoutEndDateAsync(userId, lockoutEnd);
//        }

//        public override Task<IdentityResult> SetPhoneNumberAsync(string userId, string phoneNumber)
//        {
//            return base.SetPhoneNumberAsync(userId, phoneNumber);
//        }

//        public override Task<IdentityResult> SetTwoFactorEnabledAsync(string userId, bool enabled)
//        {
//            return base.SetTwoFactorEnabledAsync(userId, enabled);
//        }

//        public override string ToString()
//        {
//            return base.ToString();
//        }

//        public override Task<IdentityResult> UpdateAsync(usuarioOasis user)
//        {
//            return base.UpdateAsync(user);
//        }

//        public override Task<IdentityResult> UpdateSecurityStampAsync(string userId)
//        {
//            return base.UpdateSecurityStampAsync(userId);
//        }

//        public override Task<bool> VerifyChangePhoneNumberTokenAsync(string userId, string token, string phoneNumber)
//        {
//            return base.VerifyChangePhoneNumberTokenAsync(userId, token, phoneNumber);
//        }

//        public override Task<bool> VerifyTwoFactorTokenAsync(string userId, string twoFactorProvider, string token)
//        {
//            return base.VerifyTwoFactorTokenAsync(userId, twoFactorProvider, token);
//        }

//        public override Task<bool> VerifyUserTokenAsync(string userId, string purpose, string token)
//        {
//            return base.VerifyUserTokenAsync(userId, purpose, token);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            base.Dispose(disposing);
//        }

//        protected override Task<IdentityResult> UpdatePassword(IUserPasswordStore<usuarioOasis, string> passwordStore, usuarioOasis user, string newPassword)
//        {
//            return base.UpdatePassword(passwordStore, user, newPassword);
//        }

//        protected override Task<bool> VerifyPasswordAsync(IUserPasswordStore<usuarioOasis, string> store, usuarioOasis user, string password)
//        {
//            var pwd = new LoginRequest() { Password = password }.PasswordEncriptada();
//            return Task.Run(() => {
//                using (var db = new as2oasis())
//                {
//                    return db.usuario.Where(x => x.username == user.username && x.password == pwd).Any();
//                };
//            });                
            
//        }
//    }
//}