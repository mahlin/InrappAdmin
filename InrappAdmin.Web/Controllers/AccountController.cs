﻿using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using InrappAdmin.ApplicationService;
using InrappAdmin.ApplicationService.Interface;
using InrappAdmin.DataAccess;
using InrappAdmin.DomainModel;
using InrappAdmin.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using InrappAdmin.Web.Models;

namespace InrappAdmin.Web.Controllers
{
   [Authorize]
   public class AccountController : Controller
   {
        private ApplicationUserManager _userManager;
        private CustomIdentityResultErrorDescriber _errorDecsriber;
        private readonly IPortalAdminService _portalAdminService;


        public AccountController()
        {
          _portalAdminService =
              new PortalAdminService(new PortalAdminRepository(new InrappAdminDbContext(), new InrappAdminIdentityDbContext()));
        }

      public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
      {
          _errorDecsriber = new CustomIdentityResultErrorDescriber();
            UserManager = userManager;
         SignInManager = signInManager;
      }

      public ApplicationUserManager UserManager
      {
         get
         {
            return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
         }
         private set
         {
            _userManager = value;
         }
      }


      private ApplicationSignInManager _signInManager;

      public ApplicationSignInManager SignInManager
      {
         get
         {
            return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
         }
         private set { _signInManager = value; }
      }


       //
       // GET: /Account/Login
       [AllowAnonymous]
       public ActionResult Login(string returnUrl)
       {
           ViewBag.ReturnUrl = returnUrl;
           return View();
       }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
           if (!ModelState.IsValid)
           {
              return View(model);
           }
            try
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, change to shouldLockout: true
                    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                        shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            return RedirectToLocal(returnUrl);
                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("AccountController", "Login", e.ToString(), e.HResult, model.Email);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid inloggningen",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
           if (ModelState.IsValid)
           {
               try
               {
                    var user = new AppUserAdmin { UserName = model.Email, Email = model.Email };
                    user.SkapadAv = model.Email;
                    user.SkapadDatum = DateTime.Now;
                    user.AndradAv = model.Email;
                    user.AndradDatum = DateTime.Now;
                    var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //  Comment the following line to prevent log in until the user is confirmed.
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    //string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");

                    //ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                    //             + "before you can log in.";

                    // For local debug only
                    //ViewBag.Link = callbackUrl;

                     //return View("Info");
                     return RedirectToAction("Index", "Home");
                  }
                  AddErrors(result);
               }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("AccountController", "Register", e.ToString(), e.HResult, model.Email);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade vid registreringen.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Återställ pinkod", "Vänligen återställ ditt lösenord genom att klicka <a href=\"" + callbackUrl + "\">här</a>");

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            ViewBag.Link = TempData["ViewBagLink"];
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                user.AndradAv = user.Email;
                user.AndradDatum = DateTime.Now;
                _portalAdminService.UppdateraAnvandarInfo(user);
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

  
             
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

       protected override void Dispose(bool disposing)
       {
           if (disposing)
           {
               if (_userManager != null)
               {
                   _userManager.Dispose();
                   _userManager = null;
               }

               if (_signInManager != null)
               {
                   _signInManager.Dispose();
                   _signInManager = null;
               }
           }

           base.Dispose(disposing);
       }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

              private IAuthenticationManager AuthenticationManager
              {
                 get
                 {
                    return HttpContext.GetOwinContext().Authentication;
                 }
              }

              private void AddErrors(IdentityResult result)
              {
                 foreach (var error in result.Errors)
                 {
                    ModelState.AddModelError("", error);
                 }
              }

              private ActionResult RedirectToLocal(string returnUrl)
              {
                 if (Url.IsLocalUrl(returnUrl))
                 {
                    return Redirect(returnUrl);
                 }
                 return RedirectToAction("Index", "Home");
              }

              internal class ChallengeResult : HttpUnauthorizedResult
              {
                 public ChallengeResult(string provider, string redirectUri)
                    : this(provider, redirectUri, null)
                 {
                 }

                 public ChallengeResult(string provider, string redirectUri, string userId)
                 {
                    LoginProvider = provider;
                    RedirectUri = redirectUri;
                    UserId = userId;
                 }

                 public string LoginProvider { get; set; }
                 public string RedirectUri { get; set; }
                 public string UserId { get; set; }

                 public override void ExecuteResult(ControllerContext context)
                 {
                    var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                    if (UserId != null)
                    {
                       properties.Dictionary[XsrfKey] = UserId;
                    }
                    context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
                 }
              }

        //private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        //{
        //   string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
        //   var callbackUrl = Url.Action("ConfirmEmail", "Account",
        //      new { userId = userID, code = code }, protocol: Request.Url.Scheme);
        //   await UserManager.SendEmailAsync(userID, subject,
        //      "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

        //   return callbackUrl;
        //}
              #endregion
           }
        }