using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using AQRWebApp.Filters;
using AQRWebApp.Models;
using AQRWebApp.Model.Entity;
using AQRWebApp.DAL;
using AQRWebApp.BL;

namespace AQRWebApp.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        private UnitOfWork unitOfWork = new UnitOfWork();


        [Authorize(Roles = "Admin")]
        public JsonResult ApplyLoginSystemToHumanResourceSystem()
        {
            string result = string.Empty;

            try
            {
                var ctx = new UsersContext();

                foreach (UserProfile user in ctx.UserProfiles)
                {
                    string username = user.UserName;
                    int userId = user.UserId;

                    var queryPersonAccount = unitOfWork.PersonAccountRepository.Get(m => m.Person.Email.Equals(username));
                    if (queryPersonAccount.FirstOrDefault() != null)
                        queryPersonAccount.FirstOrDefault().UserId = userId;
                    else
                    {
                        Person queryPerson = unitOfWork.PersonRepository.Get(m => m.Email.Equals(username)).FirstOrDefault();
                        if (queryPerson != null)
                        {
                            unitOfWork.PersonAccountRepository.Insert(new PersonAccount()
                            {
                                UserId = userId,
                                Person = unitOfWork.PersonRepository.Get(m => m.Email.Equals(username)).FirstOrDefault()
                            });
                        }
                    }
                }

                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RoleCreate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleCreate(string RoleName)
        {

            Roles.CreateRole(Request.Form["RoleName"]);
            ViewBag.ResultMessage = "Role created successfully !";

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RoleIndex()
        {
            var roles = Roles.GetAllRoles();
            return View(roles);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RoleDelete(string RoleName)
        {

            Roles.DeleteRole(RoleName);
            // ViewBag.ResultMessage = "Role deleted succesfully !";

            return RedirectToAction("RoleIndex", "Account");
        }

        /// <summary>
        /// Create a new role to the user
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult RoleAddToUser()
        {
            SelectList list = new SelectList(Roles.GetAllRoles());
            ViewBag.Roles = list;

            return View();
        }

        /// <summary>
        /// Add role to the user
        /// </summary>
        /// <param name="RoleName"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string RoleName, string UserName)
        {

            if (Roles.IsUserInRole(UserName, RoleName))
            {
                ViewBag.ResultMessage = "User đã có quyền này rồi!";
            }
            else
            {
                Roles.AddUserToRole(UserName, RoleName);
                ViewBag.ResultMessage = "Thêm quyền thành công !";
            }

            SelectList list = new SelectList(Roles.GetAllRoles());
            ViewBag.Roles = list;
            return View();
        }

        /// <summary>
        /// Get all the roles for a particular user
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ViewBag.RolesForThisUser = Roles.GetRolesForUser(UserName);
                SelectList list = new SelectList(Roles.GetAllRoles());
                ViewBag.Roles = list;
            }
            return View("RoleAddToUser");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRoleForUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {

            if (Roles.IsUserInRole(UserName, RoleName))
            {
                Roles.RemoveUserFromRole(UserName, RoleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }
            ViewBag.RolesForThisUser = Roles.GetRolesForUser(UserName);
            SelectList list = new SelectList(Roles.GetAllRoles());
            ViewBag.Roles = list;


            return View("RoleAddToUser");
        }

        //
        // GET: /Account/Index
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            RegisterModel model = new RegisterModel();
            PersonService pservice = new PersonService();
            DepartmentService departmentService = new DepartmentService();

            //model.ListOfPersonName = pservice.GetListOnWorkingOfPersonName(System.DateTime.Now, null, null).ToList();
            model.Departments = departmentService.GetAll().ToList();
            model.ListOfPersonName = pservice.GetListOnWorkingOfPersonDto(System.DateTime.Now, null, null).ToList();

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            string adminuserName = System.Web.Configuration.WebConfigurationManager.AppSettings["AdministratorUsername"];
            string adminpassword = System.Web.Configuration.WebConfigurationManager.AppSettings["AdministratorPassword"];


            if (!Roles.RoleExists(RoleNames.adminRole))
            {
                Roles.CreateRole(RoleNames.adminRole);
            }

            if (!Roles.RoleExists(RoleNames.view1Role))
            {
                Roles.CreateRole(RoleNames.view1Role);
            }

            if (!Roles.RoleExists(RoleNames.view2Role))
            {
                Roles.CreateRole(RoleNames.view2Role);
            }

            if (!Roles.RoleExists(RoleNames.view3Role))
            {
                Roles.CreateRole(RoleNames.view3Role);
            }

            if (!Roles.RoleExists(RoleNames.editRole))
            {
                Roles.CreateRole(RoleNames.editRole);
            }

            if (!Roles.RoleExists(RoleNames.deleteRole))
            {
                Roles.CreateRole(RoleNames.deleteRole);
            }

            if (!WebSecurity.UserExists(adminuserName))
            {
                WebSecurity.CreateUserAndAccount(adminuserName, adminpassword);

                Roles.AddUserToRole(System.Web.Configuration.WebConfigurationManager.AppSettings["AdministratorUsername"], RoleNames.adminRole);
            }

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
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ChangUserName

        [Authorize(Roles = "Admin")]
        public ActionResult ChangeUserName()
        {
            ChangeUserNameModel model = new ChangeUserNameModel();
            model.UserId = WebSecurity.CurrentUserId;

            return View(model);
        }

        //
        // POST: /Account/ChangUserName

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ChangeUserName(ChangeUserNameModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (!WebSecurity.UserExists(model.UserName))
        //            {
        //                var ctx = new UsersContext();
        //                var queryUser = ctx.UserProfiles.FirstOrDefault(m => m.UserId == model.UserId);

        //                if (queryUser != null)
        //                {
        //                    queryUser.UserName = model.UserName;
        //                    ctx.SaveChanges();
        //                }
        //                ViewBag.StatusMessage = "Đổi email truy cập hệ thống thành công.";
        //            }
        //            else
        //            {
        //                ViewBag.StatusMessage = "Email này đã được sử dụng.";
        //            }

        //        }
        //        catch (Exception e)
        //        {
        //            ModelState.AddModelError("", e.Message);
        //        }
        //    }

        //    return View(model);
        //}

        [Authorize]
        [HttpPost]
        public JsonResult ChangeUserName(int userId, string userName)
        {
            string result = string.Empty;
            try
            {
                if (!WebSecurity.UserExists(userName))
                {
                    var ctx = new UsersContext();
                    var queryUser = ctx.UserProfiles.FirstOrDefault(m => m.UserId == userId);

                    if (queryUser != null)
                    {
                        queryUser.UserName = userName;
                        ctx.SaveChanges();
                    }
                    result = "Đổi email truy cập hệ thống thành công.";
                }
                else
                {
                    result = "Email này đã được sử dụng.";
                }

            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return Json(result);
        }

        //
        // GET: /Account/Register

        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            RegisterModel model = new RegisterModel();
            PersonService pservice = new PersonService();

            //model.ListOfPersonName = pservice.GetListOnWorkingOfPersonName(System.DateTime.Now, null, null).ToList();

            return View(model);
        }

        //
        // POST: /Account/Register

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    if (!WebSecurity.UserExists(model.UserName))
                    {
                        AddNewAccount(model);

                        ViewBag.StatusMessage = "Tạo tài khoản thành công.";
                    }
                    else
                        ViewBag.StatusMessage = "Email này đã được sử dụng bởi nhân viên khác.";

                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            PersonService pservice = new PersonService();

            model.ListOfPersonName = pservice.GetListOnWorkingOfPersonDto(System.DateTime.Now, null, null).ToList();

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: Create a new account
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult CreateNewAccount(string username, string password, bool view1Role, bool view2Role, bool view3Role, bool editRole, bool deleteRole, bool adminRole, int personId)
        {
            string result = string.Empty;

            try
            {
                if (username != null && !username.Trim().Equals(string.Empty) && !WebSecurity.UserExists(username))
                {
                    RegisterModel model = new RegisterModel()
                    {
                        UserName = username,
                        Password = password,
                        View1Role = view1Role,
                        View2Role = view2Role,
                        View3Role = view3Role,
                        EditRole = editRole,
                        DeleteRole = deleteRole,
                        AdminRole = adminRole,
                        PersonId = personId
                    };

                    AddNewAccount(model);
                }
                else
                    result = "Username này đã được sử dụng bởi nhân viên khác.";
            }
            catch (Exception ex)
            {
                result = "Lỗi: " + ex.Message;
            }

            return Json(result);
        }

        //
        // POST: Create account automatic

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult CreateAccountAutomatic(string password)
        {
            string result = string.Empty;

            PersonService pservice = new PersonService();

            try
            {
                foreach (Person p in pservice.GetAll())
                {
                    if (p.Email != null && !p.Email.Trim().Equals(string.Empty) && !WebSecurity.UserExists(p.Email.Trim()))
                    {
                        RegisterModel model = new RegisterModel()
                        {
                            UserName = p.Email,
                            Password = password,
                            View1Role = true,
                            View2Role = false,
                            View3Role = false,
                            EditRole = false,
                            DeleteRole = false,
                            AdminRole = false,
                            PersonId = p.Id
                        };

                        AddNewAccount(model);
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Lỗi: " + ex.Message;
            }

            return Json(result);
        }

        //
        // POST: Delete All account

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult DeleteAllAccount()
        {
            string result = string.Empty;

            PersonService pservice = new PersonService();

            foreach (PersonAccount pa in pservice.GetAllPersonAccount())
            {
                result = DeleteByUserId(pa.UserId);

                if (result != string.Empty)
                    break;
            }

            return Json(result);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public string ResetPassword(string username, string newpassword)
        {
            string result = string.Empty;
            var token = WebSecurity.GeneratePasswordResetToken(username);

            bool changePasswordSucceeded = WebSecurity.ResetPassword(token, newpassword);

            return result;
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangeUserNameSuccess ? "Đổi tên truy cập hệ thống thành công."
                : message == ManageMessageId.ChangePasswordSuccess ? "Đổi mật khẩu thành công."
                : message == ManageMessageId.SetPasswordSuccess ? "Thiết lập mật khẩu thành công."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ChangePassword(string oldPassword, string newPassword)
        {
            string result = string.Empty;

            try
            {
                if (!WebSecurity.ChangePassword(User.Identity.Name, oldPassword, newPassword))
                    result = "Mật khẩu hiện tại hoặc mật khẩu mới không hợp lệ.";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result);
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
                        ViewBag.StatusMessage = "Sai mật khẩu hiện tại.";
                        ModelState.AddModelError("", "Mật khẩu hiện tại hoặc mật khẩu mới không hợp lệ.");
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

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region "Ajax data provider"

        [Authorize(Roles = "Admin")]
        public ActionResult GetList(JQueryDataTableParamModel param)
        {
            var ctx = new UsersContext();
            var allUsers = ctx.UserProfiles;

            IEnumerable<UserProfile> filteredUsers;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var usernameFilter = Convert.ToString(Request["sSearch_1"]);
                var rolenameFilter = Convert.ToString(Request["sSearch_2"]);
                var personnameFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all 
                var isUsernameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isRolenameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isPersonnameSearchable = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredUsers = allUsers
                   .Where(c => isUsernameSearchable && c.UserName != null && c.UserName.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredUsers = allUsers;
            }

            var isUsernameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isRolenameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isPersonnameSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<UserProfile, string> orderingFunction = (c => sortColumnIndex == 1 && isUsernameSortable ? c.UserName : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredUsers = filteredUsers.OrderBy(orderingFunction);
            else
                filteredUsers = filteredUsers.OrderByDescending(orderingFunction);

            IEnumerable<UserProfile> displayedUsers;

            if (param.iDisplayLength != -1)
                displayedUsers = filteredUsers.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            else
                displayedUsers = filteredUsers;

            var result = from c in displayedUsers
                         select new string[]
                              {
                                  c.UserId.ToString()
                                  ,c.UserName
                                  ,unitOfWork.PersonAccountRepository.Get(m=>m.UserId==c.UserId).FirstOrDefault() !=null ?  unitOfWork.PersonAccountRepository.Get(m=>m.UserId==c.UserId).FirstOrDefault().Person !=null ? unitOfWork.PersonAccountRepository.Get(m=>m.UserId==c.UserId).FirstOrDefault().Person.Lastname + " " + unitOfWork.PersonAccountRepository.Get(m=>m.UserId==c.UserId).FirstOrDefault().Person.Firstname : string.Empty: string.Empty
                                  ,Roles.FindUsersInRole(RoleNames.view1Role,c.UserName).FirstOrDefault()!=null ? "1" : "0"
                                  ,Roles.FindUsersInRole(RoleNames.view2Role,c.UserName).FirstOrDefault()!=null ? "1" : "0"
                                  ,Roles.FindUsersInRole(RoleNames.view3Role,c.UserName).FirstOrDefault()!=null ? "1" : "0"
                                  ,Roles.FindUsersInRole(RoleNames.editRole,ctx.UserProfiles.FirstOrDefault(m=>m.UserId==c.UserId).UserName).FirstOrDefault()!=null ? "1" : "0"
                                  ,Roles.FindUsersInRole(RoleNames.deleteRole,c.UserName).FirstOrDefault()!=null ? "1" : "0"
                                  ,Roles.FindUsersInRole(RoleNames.adminRole,c.UserName).FirstOrDefault()!=null ? "1" : "0"
                              };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allUsers.Count(),
                iTotalDisplayRecords = filteredUsers.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region "Create Update Delete operations"

        [HttpPost]
        [Authorize(Roles = "Admin")]
        private string AddNewAccount(RegisterModel model)
        {
            string result = string.Empty;

            if (!WebSecurity.UserExists(model.UserName))
            {
                WebSecurity.CreateUserAndAccount(model.UserName, model.Password);

                if (model.View1Role)
                {
                    RoleAddToUser(RoleNames.view1Role, model.UserName);
                }

                if (model.View2Role)
                {
                    RoleAddToUser(RoleNames.view2Role, model.UserName);
                }

                if (model.View3Role)
                {
                    RoleAddToUser(RoleNames.view3Role, model.UserName);
                }

                if (model.EditRole)
                {
                    RoleAddToUser(RoleNames.editRole, model.UserName);
                }

                if (model.DeleteRole)
                {
                    RoleAddToUser(RoleNames.deleteRole, model.UserName);
                }

                if (model.AdminRole)
                {
                    RoleAddToUser(RoleNames.adminRole, model.UserName);
                }

                PersonAccount pa = new PersonAccount()
                {
                    UserId = WebSecurity.GetUserId(model.UserName),
                    Person = unitOfWork.PersonRepository.GetByID(model.PersonId)
                };

                unitOfWork.PersonAccountRepository.Insert(pa);
                unitOfWork.Save();
            }
            else
                result = "User này đã có trong cơ sở dữ liệu rồi.";

            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public string Update(int id, string value, int? rowId, int? columnPosition, int? columnId, string columnName)
        {
            switch (columnPosition)
            {
                case 0:
                    var ctx = new UsersContext();

                    var queryUser = ctx.UserProfiles.Where(m => m.UserName.Equals(value));
                    if (queryUser.FirstOrDefault() == null)
                    {
                        queryUser = ctx.UserProfiles.Where(m => m.UserId == id);
                        queryUser.FirstOrDefault().UserName = value;
                        ctx.SaveChanges();
                    }

                    break;

                case 1:
                    PersonAccount querypa = unitOfWork.PersonAccountRepository.Get(m => m.UserId == id).FirstOrDefault();
                    if (querypa != null)
                    {
                        querypa.Person = unitOfWork.PersonRepository.GetByID(Convert.ToInt32(value.Trim()));
                        querypa.ModifiedDate = System.DateTime.Now;
                        unitOfWork.Save();
                    }
                    else 
                    {
                        PersonAccount personAccount = new PersonAccount()
                        {
                            UserId = id,
                            Person = unitOfWork.PersonRepository.GetByID(Convert.ToInt32(value.Trim()))
                        };

                        unitOfWork.PersonAccountRepository.Insert(personAccount);
                        unitOfWork.Save();
                    }

                    break;

                case 2:
                    ctx = new UsersContext();

                    if (value == "true")
                    {
                        if (Roles.FindUsersInRole(RoleNames.view1Role, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() == null)
                        {
                            Roles.AddUserToRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.view1Role);
                        }
                    }

                    if (value == "false")
                    {
                        if (Roles.FindUsersInRole(RoleNames.view1Role, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() != null)
                        {
                            Roles.RemoveUserFromRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.view1Role);
                        }
                    }

                    break;

                case 3:
                    ctx = new UsersContext();

                    if (value == "true")
                    {
                        if (Roles.FindUsersInRole(RoleNames.view2Role, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() == null)
                        {
                            Roles.AddUserToRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.view2Role);
                        }
                    }

                    if (value == "false")
                    {
                        if (Roles.FindUsersInRole(RoleNames.view2Role, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() != null)
                        {
                            Roles.RemoveUserFromRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.view2Role);
                        }
                    }

                    break;

                case 4:
                    ctx = new UsersContext();

                    if (value == "true")
                    {
                        if (Roles.FindUsersInRole(RoleNames.view3Role, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() == null)
                        {
                            Roles.AddUserToRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.view3Role);
                        }
                    }

                    if (value == "false")
                    {
                        if (Roles.FindUsersInRole(RoleNames.view3Role, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() != null)
                        {
                            Roles.RemoveUserFromRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.view3Role);
                        }
                    }

                    break;

                case 5:
                    ctx = new UsersContext();

                    if (value == "true")
                    {
                        if (Roles.FindUsersInRole(RoleNames.editRole, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() == null)
                        {
                            Roles.AddUserToRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.editRole);
                        }
                    }

                    if (value == "false")
                    {
                        if (Roles.FindUsersInRole(RoleNames.editRole, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() != null)
                        {
                            Roles.RemoveUserFromRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.editRole);
                        }
                    }

                    break;

                case 6:
                    ctx = new UsersContext();
                    if (value == "true")
                    {
                        if (Roles.FindUsersInRole(RoleNames.deleteRole, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() == null)
                        {
                            Roles.AddUserToRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.deleteRole);
                        }
                    }

                    if (value == "false")
                    {
                        if (Roles.FindUsersInRole(RoleNames.deleteRole, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() != null)
                        {
                            Roles.RemoveUserFromRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.deleteRole);
                        }
                    }

                    break;

                case 7:
                    ctx = new UsersContext();
                    if (value == "true")
                    {
                        if (Roles.FindUsersInRole(RoleNames.adminRole, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() == null)
                        {
                            Roles.AddUserToRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.adminRole);
                        }
                    }

                    if (value == "false")
                    {
                        if (Roles.FindUsersInRole(RoleNames.adminRole, ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName).FirstOrDefault() != null)
                        {
                            Roles.RemoveUserFromRole(ctx.UserProfiles.Where(m => m.UserId == id).FirstOrDefault().UserName, RoleNames.adminRole);
                        }
                    }

                    break;

                default:
                    break;
            }

            return value;
        }

        [Authorize(Roles = "Admin")]
        public string DeleteByUserId(int id)
        {
            string result = string.Empty;
            try
            {
                var ctx = new UsersContext();

                var queryUser = ctx.UserProfiles.FirstOrDefault(m => m.UserId == id);
                if (queryUser != null)
                {
                    string userName = queryUser.UserName;
                    if (!Roles.IsUserInRole(userName, RoleNames.adminRole))
                    {
                        if (Roles.GetRolesForUser(userName).Count() > 0)
                        {
                            Roles.RemoveUserFromRoles(userName, Roles.GetRolesForUser(userName));
                        }
                        ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(userName); // deletes record from webpages_Membership table
                        ((SimpleMembershipProvider)Membership.Provider).DeleteUser(userName, true); // deletes record from UserProfile table

                        unitOfWork.DataContext.PersonAccounts.RemoveRange(unitOfWork.PersonAccountRepository.Get(m => m.UserId == id));
                        unitOfWork.Save();
                    }
                }
                else
                {
                    unitOfWork.DataContext.PersonAccounts.RemoveRange(unitOfWork.PersonAccountRepository.Get(m => m.UserId == id));
                    unitOfWork.Save();
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        #endregion

        [Authorize]
        [HttpPost]
        public string GetFirstnameByUserName(string username)
        {
            string result = string.Empty;
            var ctx = new UsersContext();
            var queryUserId = ctx.UserProfiles.Where(m => m.UserName.Equals(username));
            if (queryUserId.FirstOrDefault() != null)
            {
                int userId = queryUserId.FirstOrDefault().UserId;
                var queryUser = unitOfWork.PersonAccountRepository.Get(m => m.UserId == userId);
                if (queryUser.FirstOrDefault() != null)
                {
                    var p = queryUser.FirstOrDefault().Person;
                    if (p != null)
                        result = p.Firstname;
                }
            }

            return result;
        }

        #region Helpers
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

        public enum ManageMessageId
        {
            ChangeUserNameSuccess,
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
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
    }
}
