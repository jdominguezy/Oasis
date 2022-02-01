using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Oasis.App_Start;
using Microsoft.AspNet.Identity.Owin;

namespace Oasis.Models.Login
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        //public CustomAuthorizeAttribute() { }

        //private CustomSignInManager _signInManager;

        //public CustomAuthorizeAttribute(
        //    CustomSignInManager signInManager)
        //{
        //    SignInManager = signInManager;

        //}
        //public CustomSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? HttpContext.Current.GetOwinContext().Get<CustomSignInManager>();
        //    }
        //    private set
        //    {
        //        _signInManager = value;
        //    }
        //}

        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        //protected virtual IPrincipal CurrentUser { get { return User} }

        //#if DEBUG
        //        protected override bool AuthorizeCore(HttpContextBase httpContext)
        //        {
        //            return true;
        //        }
        //#endif

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //var test = SignInManager
            //    .AuthenticationManager
            //    .AuthenticationResponseGrant.Identity.GetUserId();
            //var identity = Thread.CurrentPrincipal.Identity;
            //var id = HttpContext.Current.User.Identity.GetUserId();
            var CurrentUser = HttpContext.Current.User;
            if (CurrentUser.IsInRole("Admin"))
            {
                return true;
            }
            return ((CurrentUser != null && !CurrentUser.IsInRole(Roles)) || CurrentUser == null) ? false : true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var CurrentUser = HttpContext.Current.User;
            RedirectToRouteResult routeData = null;


            if (CurrentUser.Identity.IsAuthenticated)
            {
                routeData = new RedirectToRouteResult
                (new RouteValueDictionary
                 (new
                 {
                     controller = "Error",
                     action = "AccessDenied"
                 }
                 ));
            } else
            {
                routeData = new RedirectToRouteResult
                    (new RouteValueDictionary
                    (new
                    {
                        controller = "Login",
                        action = "Login",
                    }
                    ));
            }

            //if (CurrentUser == null)
            //{
            //    routeData = new RedirectToRouteResult
            //        (new RouteValueDictionary
            //        (new
            //        {
            //            controller = "Login",
            //            action = "Login",
            //        }
            //        ));
            //}
            //else
            //{
            //    routeData = new RedirectToRouteResult
            //    (new RouteValueDictionary
            //     (new
            //     {
            //         controller = "Error",
            //         action = "AccessDenied"
            //     }
            //     ));
            //}

            filterContext.Result = routeData;
        }
    }
}