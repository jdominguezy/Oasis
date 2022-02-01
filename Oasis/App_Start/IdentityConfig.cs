using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Oasis.Models;
using Oasis.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Oasis.App_Start
{
    //public class CustomSignInManager : SignInManager<usuarioOasis, string>
    //{
    //    public CustomSignInManager(CustomUserManager userManager, IAuthenticationManager authenticationManager)
    //        : base(userManager, authenticationManager)
    //    {
    //    }

    //    public override Task<ClaimsIdentity> CreateUserIdentityAsync(usuarioOasis user)
    //    {
    //        return user.GenerateUserIdentityAsync((CustomUserManager)UserManager);
    //    }

    //    public static CustomSignInManager Create(IdentityFactoryOptions<CustomSignInManager> options, IOwinContext context)
    //    {
    //        return new CustomSignInManager(context.GetUserManager<CustomUserManager>(), context.Authentication);
    //    }
    //}
}