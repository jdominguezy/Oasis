using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Owin;
using System;
using System.Threading.Tasks;
using Oasis.Models.Login;
using Oasis.App_Start;

[assembly: OwinStartup(typeof(Oasis.Startup))]

namespace Oasis
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //var authOptions = new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Login/Login"),
            //    LogoutPath = new PathString("/Login/Logout"),
            //    ExpireTimeSpan = TimeSpan.FromDays(7),
            //};

            //app.UseCookieAuthentication(authOptions);

            //app.MapSignalR();
            // Para obtener más información sobre cómo configurar la aplicación, visite https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
