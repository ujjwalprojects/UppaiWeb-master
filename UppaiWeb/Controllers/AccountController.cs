using CaptchaMvc.HtmlHelpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UppaiWeb.Helpers;
using UppaiWeb.Models;
using UppaiWeb.ViewModels;

namespace UppaiWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        ApiConnection objAPI = new ApiConnection("");
        private string LoginUrl = ConfigurationManager.AppSettings["LoginURL"];
        private string APIUrl = ConfigurationManager.AppSettings["ApiURL"];
        ApiConnection objAPINav = new ApiConnection("general");
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }


        #region Other Operation
        [AllowAnonymous]
        public ActionResult New(string code, string userId)
        {
            if (code == null || userId == null)
                return View("Error");
            ActivateModel model = new ActivateModel()
            {
                Code = code,
                UserID = userId
            };
            string jsonStr = JsonConvert.SerializeObject(model);
            string result = objAPI.PostRecordtoApIWithoutCredentials("account", "new", jsonStr);
            if (!result.Contains("Error"))
            {
                TempData["ErrMsg"] = result;
                return RedirectToAction("setpassword", new { userId = userId, code = result });
            }
            TempData["ErrMsg"] = result;
            return RedirectToAction("login");
        }
        [AllowAnonymous]
        public ActionResult SetPassword(string userId, string code)
        {
            try
            {
                if (code == null || userId == null)
                    return View("Error");
                SetPasswordManageModel model = new SetPasswordManageModel();
                model.UserProfile = objAPI.GetObjectByKeyWithoutCredentials<UserProfileModel>("account", "userprofilebyid", userId, "Id");
                model.Code = code;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrMsg"] = "Error: Operation Failed, " + ex.Message;
                return RedirectToAction("login");
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetPassword(SetPasswordManageModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.SetPassword.Code = model.Code;
                    string jsonStr = JsonConvert.SerializeObject(model.SetPassword);
                    string result = objAPI.PostRecordtoApIWithoutCredentials("Account", "SetPassword", jsonStr);
                    if (result.Equals("Success"))
                    {
                        TempData["ErrMsg"] = "Password set for your account, please use the credentials to access your account";
                        return RedirectToAction("login");
                    }
                    TempData["ErrMsg"] = result;
                }
                model.UserProfile = objAPI.GetObjectByKeyWithoutCredentials<UserProfileModel>("account", "userprofilebyid", model.SetPassword.UserID, "Id");
                return View(model);
            }
            catch (Exception)
            {
                TempData["ErrMsg"] = "Error: Operation Failed, you can reset password using Forgot Password option.";
                return RedirectToAction("login");
            }
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                ApiConnection objApi = new ApiConnection("");
                ChangePasswordModel change_model = new ChangePasswordModel()
                {
                    Email = User.Identity.Name,
                    OldPassword = model.OldPassword,
                    NewPassword = model.NewPassword
                };
                String Jasonstring = JsonConvert.SerializeObject(change_model);
                string result = objApi.PostRecordtoApI("account", "changepassword", Jasonstring);
                if (result.Contains("Success"))
                {
                    TempData["ErrMsg"] = "Password successfully changed.";
                    return RedirectToAction("login", "account", new { Area = "" });
                }
                ModelState.AddModelError("", result);
                return View(model);
            }
            catch (AuthorizationException)
            {
                TempData["ErrMsg"] = "Your Login Session has expired. Please Login Again";
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
            catch (Exception ex)
            {
                TempData["ErrMsg"] = "Operation failed. Try Again. \nError Details: " + ex.Message;
                return View(model);
            }
        }
        #endregion
        #region genaral Login/Register
        //[AllowAnonymous]
        //public ActionResult GeneralLogin()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> GeneralLogin(LoginModel model, string returnUrl)
        //{
        //    string key = "8080808080808080";
        //    string iv = "8080808080808080";
        //    if (model.Password != null)
        //    {
        //        model.Password = AES.Decrypt(model.Password, key, iv);
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        HttpContent content = new FormUrlEncodedContent(new[]
        //        {
        //            new KeyValuePair<string, string>("grant_type", "password"),
        //            new KeyValuePair<string, string>("username", model.UserName),
        //            new KeyValuePair<string, string>("password", model.Password)
        //        });
        //        HttpResponseMessage result = await httpClient.PostAsync(LoginUrl, content);
        //        if (result.IsSuccessStatusCode)
        //        {
        //            string resultContent = result.Content.ReadAsStringAsync().Result;
        //            var token = JsonConvert.DeserializeObject<TokenModel>(resultContent);
        //            SessionModel session_model = new SessionModel()
        //            {
        //                AccessToken = string.Format("Bearer {0}", token.access_token),
        //                UserID = token.userId,
        //                Role = token.role,
        //                UserImage = token.userImage,
        //                UserName = token.userName,
        //                Email = token.email,
        //                ProfileName = token.profileName
        //            };
        //            Session["SessionVar"] = session_model;
        //            AuthenticationProperties options = new AuthenticationProperties();
        //            //options.a = true;
        //            options.IsPersistent = true;
        //            //options.ExpiresUtc = DateTime.UtcNow.AddMinutes(15);
        //            var claims = new[]
        //                {
        //                    //new Claim(ClaimTypes.Name, token.userId),
        //                    new Claim(ClaimTypes.Name, token.userName),
        //                    new Claim(ClaimTypes.Role, token.role)
        //                };
        //            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
        //            Request.GetOwinContext().Authentication.SignIn(options, identity);
        //            return RedirectToAction("AddToShoppingCart", "account", new { UrlVal = returnUrl });
        //            //if (returnUrl != "" && returnUrl != null)
        //            //    return RedirectToLocal(returnUrl);
        //            //else
        //            //{
        //            //    return RedirectToAction("index", "home");
        //            //}
        //        }
        //        else
        //        {
        //            string resultContent = result.Content.ReadAsStringAsync().Result;
        //            var login_error = JsonConvert.DeserializeObject<LoginErrorModel>(resultContent);
        //            ModelState.AddModelError("ErrorMsg", login_error.error_description);
        //            TempData["ErrMsg"] = login_error.error_description;
        //            return View(model);

        //        }
        //    }
        //}
        // GET: /Account/Register from Customer
        [AllowAnonymous]
        public ActionResult Register(string ReqFrom = "")
        {
            string RoleName = "";
            if (ReqFrom == "")
                RoleName = "Customer";
            else
                RoleName = ReqFrom;
            UserRegisterModel model = new UserRegisterModel();
            model.RoleName = RoleName;
            return View(model);
        }
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegisterModel model,bool agree)
        {
            //GenWebApiConnection objGenApi = new GenWebApiConnection();
            if (!agree)
            {
                ModelState.AddModelError("", "Please Check on I agree to Uppai's Terms & Conditions");
                TempData["ErrMsg"] = "Please Check on I agree to Uppai's Terms & Conditions";
                return View(model);
            }
            if (!this.IsCaptchaValid("Invalid Captcha Text"))
            {
                ModelState.AddModelError("", "Invalid Captcha Text");
                TempData["ErrMsg"] = "Invalid Captcha Text";
                return View(model);
            }
            if (ModelState.IsValid)
            {
                string jsonStr = JsonConvert.SerializeObject(model);
                string result = objAPI.PostRecordtoApIWithoutCredentials("Account", "UserRegister", jsonStr);
                if (result.Contains("Success"))
                {
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("Login", "account");
                }
                TempData["ErrMsg"] = "Error:" + result;
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AddToShoppingCart(string UrlVal = "")
        {
            if (Session["CheckOutItems"] != null)
            {
                List<utblShoppingCart> addcart = new List<utblShoppingCart>();
                List<CheckOutItem> cartItems = Session["CheckOutItems"] as List<CheckOutItem>;
                for (int i = 0; i < cartItems.Count; i++)
                {
                    utblShoppingCart eachcart = new utblShoppingCart();
                    {
                        eachcart.MenuID = cartItems[i].MenuID;
                        eachcart.Qty = cartItems[i].Qty;
                        eachcart.AddedBy = User.Identity.Name;
                    };
                    addcart.Add(eachcart);
                }
                string jsonStr = JsonConvert.SerializeObject(addcart);
                string result = objAPINav.PostRecordtoApI("Customer", "AddToCart", jsonStr);
            }
            List<CheckOutItem> GetItems = objAPINav.GetRecordsByQueryString<CheckOutItem>("Customer", "GetCheckOutItems", "Id=" + User.Identity.Name);//Get Check Out Items By UserName
            Session["CheckOutItems"] = GetItems;
            if (UrlVal != "" && UrlVal != null)
                return RedirectToLocal(UrlVal);
            else
            {
                return RedirectToAction("index", "home");
            }

            //string RoleName = "";
            //if (ReqFrom == "")
            //    RoleName = "Customer";
            //else
            //    RoleName = ReqFrom;
            //UserRegister model = new UserRegister();
            //model.RoleName = RoleName;
        }

        #endregion
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            string key = "8080808080808080";
            string iv = "8080808080808080";
            if (model.Password != null)
            {
                model.Password = AES.Decrypt(model.Password, key, iv);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", model.UserName),
                    new KeyValuePair<string, string>("password", model.Password)
                });
                HttpResponseMessage result = await httpClient.PostAsync(LoginUrl, content);
                if (result.IsSuccessStatusCode)
                {
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                    var token = JsonConvert.DeserializeObject<TokenModel>(resultContent);
                    SessionModel session_model = new SessionModel()
                    {
                        AccessToken = string.Format("Bearer {0}", token.access_token),
                        UserID = token.userId,
                        Role = token.role,
                        UserImage = token.userImage,
                        UserName = token.userName,
                        Email = token.email,
                        ProfileName = token.profileName
                    };
                    Session["SessionVar"] = session_model;
                    AuthenticationProperties options = new AuthenticationProperties();
                    //options.a = true;
                    options.IsPersistent = true;
                    //options.ExpiresUtc = DateTime.UtcNow.AddMinutes(15);
                    var claims = new[]
                        {
                            //new Claim(ClaimTypes.Name, token.userId),
                            new Claim(ClaimTypes.Name, token.userName),
                            new Claim(ClaimTypes.Role, token.role)
                        };
                    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                    Request.GetOwinContext().Authentication.SignIn(options, identity);
                    if (returnUrl != "" && returnUrl != null && token.role != "Customer")
                        return RedirectToLocal(returnUrl);
                    else
                    {
                        return RedirectToAction("dashboard", "user", new { Area = "", returnnav = returnUrl });
                    }
                }
                else
                {
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                    var login_error = JsonConvert.DeserializeObject<LoginErrorModel>(resultContent);
                    ModelState.AddModelError("ErrorMsg", login_error.error_description);
                    TempData["ErrMsg"] = login_error.error_description;
                    return View(model);
                    //if (login_error.error.Equals("not_activated"))
                    //{
                    //    ModelState.AddModelError("ErrorMsg", login_error.error_description);
                    //    return View(model);
                    //}
                    //if (login_error.error.Equals("disabled"))
                    //{
                    //    ModelState.AddModelError("ErrorMsg", login_error.error_description);
                    //    return View(model);
                    //}
                    //ModelState.AddModelError("ErrorMsg", login_error.error_description);
                    //return View(model);
                }
            }
        }


        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!this.IsCaptchaValid("Invalid Captcha Text"))
            {
                ModelState.AddModelError("", "Invalid Captcha Text");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //ApiConnection objApi = new ApiConnection("");
                    //String Jasonstring = JsonConvert.SerializeObject(model);
                    //string result = objApi.PostRecordtoApI("account", "forgotpassword", Jasonstring);
                    //if (result.Contains("Success"))
                    //{
                    //    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                    //}
                    //ModelState.AddModelError("", result);
                    //return View(model);

                    String Url = APIUrl + "admin/employee/ForgotPassword?Email=" + model.Email;
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage result = await httpClient.PostAsync(Url, null);

                        if (result.IsSuccessStatusCode)
                        {
                            string resultContent = result.Content.ReadAsStringAsync().Result;

                            var pass_result = JsonConvert.DeserializeObject<string>(resultContent);

                            if (pass_result.Equals("Success"))
                            {
                                return RedirectToAction("ForgotPasswordConfirmation", "Account");
                            }
                            ModelState.AddModelError("", pass_result);
                            return View(model);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Network Error");
                            return View(model);
                        }
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Network Error");
                    return View(model);
                }
            }

            return View(model);
        }
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                ApiConnection objApi = new ApiConnection("Admin");
                String Jasonstring = JsonConvert.SerializeObject(model);
                string result = objApi.PostRecordtoApI("employee", "resetpassword", Jasonstring);
                if (result.Contains("Success"))
                {
                    TempData["ErrMsg"] = "Password reset successful. Please Login again to continue.";
                    return RedirectToAction("Login", "Account", new { Area = "" });
                }
                ModelState.AddModelError("", result);
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Network Error");
                return View(model);
            }
        }

        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["CheckOutItems"] = null;
            Session["SessionVar"] = null;
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
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

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
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
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}