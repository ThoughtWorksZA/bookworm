using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BirdBrain;
using BookWorm.Services.Account;
using BookWorm.Services.Email;
using Microsoft.Web.WebPages.OAuth;
using Raven.Client;
using WebMatrix.WebData;
using BookWorm.Models;
using Roles = BookWorm.Models.Roles;

namespace BookWorm.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private IAccountService _accountService;
        private IEmailService _emailService;

        public IEmailService EmailService
        {
            get
            {
                return _emailService ?? (_emailService = new EmailService());
            }
            set
            {
                _emailService = value;
            }
        }

        public IAccountService AccountService
        {
            get
            {
                return _accountService ?? (_accountService = new AccountService());
            }
            set
            {
                _accountService = value;
            }
        }

        public AccountController()
        {

        }

        public AccountController(Repository repository) : base(repository)
        {
        }

        public AccountController(IDocumentSession documentSession)
        {
            DocumentSession = documentSession;
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
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && AccountService.Login(model.Email, model.Password, model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AccountService.Logout();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (GetUsersCount() > 0)
                return new HttpStatusCodeResult(403);
            return View("Register");
        }

        protected virtual int GetUsersCount()
        {
            return Membership.GetAllUsers().Count;
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                AccountService.CreateUserAndAccount(model.Email, model.Password);
                AccountService.AddUserToRole(model.Email, model.Role);
                AccountService.Login(model.Email, model.Password, false);
                return RedirectToAction("Index", "Home");
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }

            return View(model);
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        protected virtual bool IsLocalUrl(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl);
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        [HttpGet]
        public ViewResult List()
        {
            var users = DocumentSession.Query<User>().OrderByDescending(u=>u.Created).ToList();
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public ViewResult Create()
        {
            ViewBag.Title = "Add a User";
            return View(new UserInformation(new RegisterModel()
                {
                    ConfirmPassword = RegisterModel.DefaultPassword,
                    Password = RegisterModel.DefaultPassword
                }));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser =  Membership.GetUser(model.Email);

                if (existingUser != null)
                {
                    ViewBag.Title = "Add a User";
                    TempData["flashError"] = "A user with this email already exists";
                    return View(new UserInformation(model));
                }
                try
                {
                    var securityToken = WebSecurity.CreateUserAndAccount(model.Email, model.Password, new {Email = model.Email}, true);
                    EmailService.SendConfirmation("donotreply@puku.co.za", model.Email, securityToken, WebSecurity.GetUserId(model.Email));
                    System.Web.Security.Roles.AddUsersToRole(new string[] { model.Email }, model.Role);
                    return RedirectToAction("List", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            return View(new UserInformation(model));
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult RegisterConfirmation(string secureToken, int userId)
        {
            ViewBag.Title = "Confirm user account";
            return View(new LocalPasswordModel()
                {
                    SecurityToken = secureToken,
                    UserId = userId,
                    OldPassword = RegisterModel.DefaultPassword
                });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterConfirmation(LocalPasswordModel localPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = DocumentSession.Load<User>(localPasswordModel.UserId);
                WebSecurity.ConfirmAccount(user.Username, localPasswordModel.SecurityToken);
                WebSecurity.ChangePassword(user.Username, RegisterModel.DefaultPassword,
                                           localPasswordModel.NewPassword);

                WaitForTheEndOfWritingPassword(user.Username);

                AccountService = AccountService ?? new AccountService();
                if (AccountService.Login(user.Username, localPasswordModel.NewPassword, false))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(localPasswordModel);
        }

        private void WaitForTheEndOfWritingPassword(string username)
        {
            DocumentSession.Query<User>()
                    .Customize(u => u.WaitForNonStaleResultsAsOfLastWrite())
                    .First(u => u.Username == username);
        }
    }
}
