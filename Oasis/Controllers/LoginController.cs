using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Oasis.App_Start;
using Oasis.Models;
using Oasis.Models.Login; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace Oasis.Controllers 
{
    [AllowAnonymous]
    [Authorize]
    public class LoginController : Controller
    {
        



        //// GET: Account
        //public void Index()
        //{
        //    RedirectToAction("Home", "Index", new { area = "" });
        //    //return View();
        //}       

        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {

            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpGet]
        public ActionResult CambiarPassword(string ReturnUrl = "")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Login");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (_userManager != null)
        //        {
        //            _userManager.Dispose();
        //            _userManager = null;
        //        }

        //        if (_signInManager != null)
        //        {
        //            _signInManager.Dispose();
        //            _signInManager = null;
        //        }
        //    }

        //    base.Dispose(disposing);
        //}


        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(usuarioOasis model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Default UserStore constructor uses the default connection string named: DefaultConnection
        //        var userStore = new UserStore<IdentityUser>();
        //        var manager = new UserManager<IdentityUser>(userStore);

        //        var user = new IdentityUser() { UserName = UserName.Text };
        //        IdentityResult result = manager.Create(user, Password.Text);

        //        var user = new ApplicationUser() { UserName = model.UserName };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            AddErrors(result);
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        [HttpPost]
        public ActionResult CambiarPassword(string password1, string password2, string ReturnUrl = "")
        {
            if (password1 == password2)
            {
                var usuarioActual = this.User;
                using(var db = new as2oasis()){
                    var pwd = new LoginRequest();
                    pwd.Password = password1;
                    var usuario = db.usuario.Find((User as CustomPrincipal).id_usuario);
                    usuario.password = pwd.PasswordEncriptada();
                    db.SaveChanges();
                }
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            } else
            {
                ModelState.AddModelError("", "Algo salió mal : Los campos no coinciden ");
                return View();
            }
             
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  ActionResult Login(LoginView loginView, string ReturnUrl = "")
        {
            var pwd = new LoginRequest() { Password = loginView.password }.PasswordEncriptada();

            if (!ModelState.IsValid)
            {
                return View(loginView);
            }

            if (Membership.ValidateUser(loginView.username, pwd))
            {
                var usuario = (CustomMemberShipUser)Membership.GetUser(loginView.username,false);
                if (usuario != null)
                {
                    CustomSerializeModel userModel = new CustomSerializeModel()
                    {
                        id_usuario = usuario.id_usuario,
                        nombre = usuario.nombre,
                        apellido = usuario.apellido,
                        roles = usuario.roles.Select(x => x.nombre).ToList()
                    };
                    var userData = JsonConvert.SerializeObject(userModel);
                    var authTicket = new FormsAuthenticationTicket
                           (
                           1, loginView.username, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                           );
                    string enTicket = FormsAuthentication.Encrypt(authTicket);
                    var faCookie = new HttpCookie("Cookie1", enTicket);
                    Response.Cookies.Add(faCookie);
                }
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Credenciales incorrectas");
            return View(loginView);
            
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationView registrationView)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;

            if (ModelState.IsValid)
            {
                // Email Verification
                string userName = Membership.GetUserNameByEmail(registrationView.Email);
                if (!string.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError("Warning Email", "Sorry: Email already Exists");
                    return View(registrationView);
                }

                //Save User Data
                using (as2oasis dbContext = new as2oasis())
                {
                    var user = new usuarioOasis()
                    {
                        username = registrationView.Username,
                        nombre = registrationView.FirstName,
                        apellido = registrationView.LastName,
                        email = registrationView.Email,
                        password = registrationView.Password,
                        //ActivationCode = Guid.NewGuid(),
                    };

                    dbContext.usuario.Add(user);
                    dbContext.SaveChanges();
                }

                //Verification Email
                VerificationEmail(registrationView.Email, registrationView.ActivationCode.ToString());
                messageRegistration = "Tu cuenta ha sido creada correctamente.";
                statusRegistration = true;
            }
            else
            {
                messageRegistration = "Ups, algo salió mal!";
            }
            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;

            return View(registrationView);
        }

        [HttpGet]
        public ActionResult ActivationAccount(string id)
        {
            bool statusAccount = false;
            using (as2oasis dbContext = new as2oasis())
            {
                var userAccount = dbContext.usuario.Where(u => u.id_usuario.ToString().Equals(id)).FirstOrDefault();

                if (userAccount != null)
                {
                    userAccount.indicador_vendedor = true;
                    dbContext.SaveChanges();
                    statusAccount = true;
                }
                else
                {
                    ViewBag.Message = "Something Wrong !!";
                }

            }
            ViewBag.Status = statusAccount;
            return View();
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login", null);
        }

        [NonAction]
        public void VerificationEmail(string email, string activationCode)
        {
            var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

            var fromEmail = new MailAddress("oasis@grupolabovida.com", "Activación de cuenta");
            var toEmail = new MailAddress(email);

            var fromEmailPassword = "2d{uC(_v4jr=";
            string subject = "Activation Account !";

            string body = "<br/> Please click on the following link in order to activate your account" + "<br/><a href='" + link + "'> Activation Account ! </a>";

            var smtp = new SmtpClient
            {
                Host = "mail.grupolabovida.com",
                Port = 587,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword), 
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })

                smtp.Send(message);

        }

        //#region Helpers
        //// Used for XSRF protection when adding external logins
        //private const string XsrfKey = "XsrfId";

        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}


        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}

        //private bool HasPassword()
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        return user.PasswordHash != null;
        //    }
        //    return false;
        //}

        //public enum ManageMessageId
        //{
        //    ChangePasswordSuccess,
        //    SetPasswordSuccess,
        //    RemoveLoginSuccess,
        //    Error
        //}

        //private ActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}

        //private class ChallengeResult : HttpUnauthorizedResult
        //{
        //    public ChallengeResult(string provider, string redirectUri)
        //        : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        LoginProvider = provider;
        //        RedirectUri = redirectUri;
        //        UserId = userId;
        //    }

        //    public string LoginProvider { get; set; }
        //    public string RedirectUri { get; set; }
        //    public string UserId { get; set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
        //        if (UserId != null)
        //        {
        //            properties.Dictionary[XsrfKey] = UserId;
        //        }
        //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //    }
        //}
        //#endregion
    }
}
