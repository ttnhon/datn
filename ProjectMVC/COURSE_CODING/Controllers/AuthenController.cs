using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BotDetect.Web.Mvc;
using CommonProject;
using CommonProject.Helper;
using COURSE_CODING.Common;
using COURSE_CODING.Models;
using DAO.DAO;
using DAO.EF;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace COURSE_CODING.Controllers
{
    public class AuthenController : Controller
    {
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        private string stringOTP = string.Empty;
        private DateTime timeBegin;

        [HttpGet]
        public ActionResult Home()
        {
            var session = (COURSE_CODING.Common.InfoLogIn)Session[CommonProject.CommonConstant.SESSION_INFO_LOGIN];
            if (session != null)
            {
                if (session.Role.Equals(CommonConstant.ROLE_ADMIN))
                {
                    return Redirect("/Admin/User/index");
                }
                else
                {
                    if (session.Role.Equals(CommonConstant.ROLE_MEMBER))
                    {
                        return Redirect("/User/Dashboard");
                    }
                }
                if (session.Role.Equals(CommonConstant.ROLE_TEACHER))
                {
                    return Redirect("/Moderator/ManageChallenge");
                }
            }
            return View("Login");
        }
        [HttpGet]
        public ActionResult Register()
        {
            var session = (COURSE_CODING.Common.InfoLogIn)Session[CommonProject.CommonConstant.SESSION_INFO_LOGIN];
            if (session != null)
            {
                if (session.Role.Equals(CommonConstant.ROLE_ADMIN))
                {
                    return Redirect("/Admin/User/index");
                }
                else
                {
                    if (session.Role.Equals(CommonConstant.ROLE_MEMBER))
                    {
                        return Redirect("/User/Dashboard");
                    }
                }
                if (session.Role.Equals(CommonConstant.ROLE_TEACHER))
                {
                    return Redirect("/Moderator/ManageChallenge");
                }

            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Incorrect CAPTCHA code!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                string callback_uri = string.Empty;
                if (Request.UrlReferrer != null)
                {
                    var paramss = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
                    callback_uri = paramss["callback"] ?? "";
                }
                var DAO = new UserDAO();
                if (DAO.CheckUserNameExist(model.UserName))
                {
                    ModelState.AddModelError(String.Empty, "This user name is existed");
                }
                else if (DAO.CheckEmailExist(model.Email))
                {
                    ModelState.AddModelError(String.Empty, "Your email is registerd");
                }
                else
                {
                    USER_INFO u = new USER_INFO();
                    u.FirstName = model.FirstName;
                    u.LastName = model.LastName;
                    u.UserName = model.UserName;
                    u.PasswordUser = HashMD5.HashStringMD5(model.PasswordUser);
                    u.RoleUser = CommonConstant.ROLE_MEMBER;
                    u.Email = model.Email;
                    u.SchoolID = 1;
                    u.CreateDate = DateTime.Now;
                    Boolean result = DAO.Insert(u);
                    if (result)
                    {
                        var sessionLogin = new InfoLogIn();
                        sessionLogin.ID = u.ID;
                        sessionLogin.Name = u.UserName;
                        Session.Add(CommonConstant.SESSION_INFO_LOGIN, sessionLogin);
                        ViewBag.Success = "Register sussesfull";
                        model = new RegisterModel();
                        if (!String.IsNullOrEmpty(callback_uri))
                        {
                            return Redirect(callback_uri);
                        }
                        return Redirect("/User/Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Fail register your account");
                    }
                }
            }
            return View("Register");
        }

        [OutputCache(Duration = 3600, VaryByParam = "none")]
        [HttpGet]
        public ActionResult Login()
        {
            var session = (COURSE_CODING.Common.InfoLogIn)Session[CommonProject.CommonConstant.SESSION_INFO_LOGIN];
            if (session != null)
            {
                if (session.Role.Equals(CommonConstant.ROLE_ADMIN))
                {
                    return Redirect("/Admin/User/index");
                }
                else
                {
                    if (session.Role.Equals(CommonConstant.ROLE_MEMBER))
                    {
                        return Redirect("/User/Dashboard");
                    }
                }
                if (session.Role.Equals(CommonConstant.ROLE_TEACHER))
                {
                    return Redirect("/Moderator/ManageChallenge");
                }

            }

            return View();
        }

        public ActionResult Logout()
        {
            Session[CommonConstant.SESSION_INFO_LOGIN] = null;
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string callback_uri = string.Empty;
                if (Request.UrlReferrer != null)
                {
                    var paramss = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
                    callback_uri = paramss["callback"] ?? "";
                }
                var DAO = new UserDAO();
                var statusLogin = DAO.Login(model.UserName, HashMD5.HashStringMD5(model.Password));
                if (statusLogin.Equals(CommonConstant.STATUS_RIGHT_ACCOUNT))
                {
                    var user = DAO.GetByName(model.UserName);
                    var sessionLogin = new InfoLogIn();
                    sessionLogin.ID = user.ID;
                    sessionLogin.Name = user.UserName;
                    sessionLogin.Role = (int)user.RoleUser;
                    Session.Add(CommonConstant.SESSION_INFO_LOGIN, sessionLogin);

                    if (!String.IsNullOrEmpty(callback_uri))
                    {
                        return Redirect(callback_uri);
                    }
                    if (user.RoleUser.Equals(CommonConstant.ROLE_ADMIN))
                    {
                        return Redirect("/Admin/Home/Index");
                    }
                    else if (user.RoleUser.Equals(CommonConstant.ROLE_MEMBER))
                    {
                        return Redirect("/User/Dashboard");
                    }
                    else if (user.RoleUser.Equals(CommonConstant.ROLE_TEACHER))
                    {
                        return Redirect("/Moderator/ManageChallenge");
                    }
                    return Redirect("/");
                }
                else if (statusLogin.Equals(CommonConstant.STATUS_NOT_EXIST_ACCOUNT))
                {
                    ModelState.AddModelError(String.Empty, "This account is not exist");
                }
                else if (statusLogin.Equals(CommonConstant.STATUS_LOCK_ACCOUNT))
                {
                    ModelState.AddModelError(String.Empty, "This account is locked");
                }
                else if (statusLogin.Equals(CommonConstant.STATUS_WRONG_PASS_ACCOUNT))
                {
                    ModelState.AddModelError(String.Empty, "Your password is wrong");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Fail login !");
                }
            }
            else
            {
                MvcCaptcha.ResetCaptcha("registerCaptcha");
            }
            return View();
        }
        public void SignIn(string ReturnUrl = "/", string type = "")
        {
            try
            {
                if (type == "Google")
                {
                    HttpContext.GetOwinContext().Authentication.Challenge(new
                        AuthenticationProperties
                    { RedirectUri = "Authen/GoogleLoginCallback" }, "Google");
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        [AllowAnonymous]
        public ActionResult GoogleLoginCallback()
        {
            try
            {
                var claimsPrincipal = HttpContext.User.Identity as ClaimsIdentity;
                var loginInfo = GoogleLoginViewModel.GetLoginInfo(claimsPrincipal);
                if (loginInfo == null)
                {
                    return RedirectToAction("/");
                }
                string callback_uri = string.Empty;
                if (Request.UrlReferrer != null)
                {
                    var paramss = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
                    callback_uri = paramss["callback"] ?? "";
                }
                var DAO = new UserDAO();
                var user = DAO.GetByEmail(loginInfo.emailaddress);
                Boolean canLogin = false;
                if (user == null)
                {
                    user = new USER_INFO();
                    string ID = Guid.NewGuid().ToString();
                    user.FirstName = loginInfo.givenname;
                    user.LastName = loginInfo.surname;
                    user.Email = loginInfo.emailaddress;
                    user.RoleUser = CommonConstant.ROLE_MEMBER;
                    user.StatusUser = CommonConstant.STATUS_RIGHT_ACCOUNT;
                    user.UserName = loginInfo.surname;
                    user.PasswordUser = "123456";
                    user.CreateDate = DateTime.Now;
                    user.SchoolID = 1;
                    canLogin = new UserDAO().Insert(user);
                }
                else
                {
                    canLogin = true;
                }
                if (canLogin.Equals(true))
                {
                    var userSession = new InfoLogIn();
                    userSession.Name = user.UserName;
                    userSession.ID = user.ID;
                    userSession.Role = user.RoleUser;
                    Session.Add(CommonConstant.SESSION_INFO_LOGIN, userSession);
                    // Set the auth cookie
                    // FormsAuthentication.SetAuthCookie(user.Email, false);
                    if (!String.IsNullOrEmpty(callback_uri))
                    {
                        return Redirect(callback_uri);
                    }
                    if (user.RoleUser.Equals(CommonConstant.ROLE_ADMIN))
                    {
                        return Redirect("/Admin/Home/Index");
                    }
                    else if (user.RoleUser.Equals(CommonConstant.ROLE_MEMBER))
                    {
                        return Redirect("/User/Dashboard");
                    }
                    else if (user.RoleUser.Equals(CommonConstant.ROLE_TEACHER))
                    {
                        return Redirect("/Moderator/ManageChallenge");
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return Redirect("~/");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(ResetModel model)
        {
            ViewBag.isValidateCode = true;
            TempData["timeBegin"]= DateTime.Now;
            TempData["email"] = model.Email;
            string baseURL = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string destinationURL = baseURL + "ChangePassword";
            var DAO = new UserDAO();
            var user = DAO.GetByEmail(model.Email);
            if (user != null)
            {
                this.stringOTP = OTP.GenerateStringCodeOTP();
                TempData["stringOTP"] = OTP.GenerateStringCodeOTP();
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Page/pages/ConfirmPassword.html"));
                content = content.Replace("{{CustomerName}}", user.UserName);
                content = content.Replace("{{Code}}", this.stringOTP);
                content = content.Replace("{{Content}}", destinationURL);
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new Email_Helper().SendMail(model.Email, "Confirm from coursecoding", content);
                
                SetAlert("Go to your mail to confirm password", "success");
                return View("ChangePassword");
            }
            else return View("Login");
        }
        [HttpPost]
        public ActionResult ChangePassword(ForgotPasswordModel model)
        {
           // dang mat time begin va string otp o method nay
            if (ModelState.IsValid)
            {
                if ((DateTime)TempData["timeBegin"] != null&& TempData["stringOTP"]!=null && TempData["email"]!=null)
                {
                    DateTime timeLimit = ((DateTime)TempData["timeBegin"]).AddHours((double)CommonConstant.TIME_OUT_HOUR_CONFRIMPASS);
                    if (DateTime.Now.CompareTo(timeLimit) < 0 
                        && model.CodeValidate.Equals(TempData["stringOTP"].ToString())
                        && TempData["email"].ToString().Equals(model.Email))
                    {
                        var DAO = new UserDAO();
                        var user = DAO.GetByEmail(model.Email);
                        user.PasswordUser = HashMD5.HashStringMD5(model.Password);
                        bool result = DAO.Update(user);
                        if (result)
                        {
                            var sessionLogin = new InfoLogIn();
                            sessionLogin.ID = user.ID;
                            sessionLogin.Name = user.UserName;
                            sessionLogin.Role = user.RoleUser;
                            Session.Add(CommonConstant.SESSION_INFO_LOGIN, sessionLogin);
                            ViewBag.Success = "Change password sussesfull!";
                            SetAlert("Change password successfull", "success");
                            TempData.Keep();
                            if (sessionLogin.Role.Equals(CommonConstant.ROLE_ADMIN))
                            {
                                return Redirect("/Admin/User/index");
                            }
                            else
                            {
                                if (sessionLogin.Role.Equals(CommonConstant.ROLE_MEMBER))
                                {
                                    return Redirect("/User/Dashboard");
                                }
                            }
                            if (sessionLogin.Role.Equals(CommonConstant.ROLE_TEACHER))
                            {
                                return Redirect("/Moderator/ManageChallenge");
                            }
                        }
                        else
                        {
                            SetAlert("Can not change  your password", "error");
                        }
                    }
                    if (DateTime.Now.CompareTo(timeLimit) > 0)
                    {
                        SetAlert("Time of OTP code is over", "error");
                    }else
                    if (!model.CodeValidate.Equals(TempData["stringOTP"].ToString()))
                    {
                        SetAlert("Code OTP not match", "error");
                    }else
                    if (!TempData["email"].ToString().Equals(model.Email))
                    {
                        SetAlert("This is not your mail", "error");
                    }
                }
            }
            TempData.Keep();
            //SetAlert("Can not change  your password", "error");
            model = new ForgotPasswordModel();
            return View("ChangePassword");
        }
        public ActionResult ChangePassword()
        {
            TempData.Keep();
            return View();
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
        
        [Route("Competition/{competeID}/Invitation")]
        public ActionResult JoinCompete(int competeID)
        {
            try
            {
                string redirectURL = "/Compete/" + competeID.ToString() + "/Invitation";
                string token = Request.QueryString["ticket"] ?? "";
                string email = CommonProject.Helper.Encrypt.DecryptString(token, CommonConstant.SECRET_KEY_TOKEN);
                if (!this.IsValidEmail(email))
                {
                    return Content("Token is invalid");
                }
                var session = (COURSE_CODING.Common.InfoLogIn)Session[CommonProject.CommonConstant.SESSION_INFO_LOGIN];
                USER_INFO user = new UserDAO().GetByEmail(email);

                if (session != null)
                {
                    return Redirect(redirectURL);
                }
                else if (user.UserName.Equals(user.Email))           //is Trial user
                {
                    var sessionLogin = new InfoLogIn();
                    sessionLogin.ID = user.ID;
                    sessionLogin.Name = user.UserName;
                    sessionLogin.Role = (int)user.RoleUser;
                    Session.Add(CommonConstant.SESSION_INFO_LOGIN, sessionLogin);
                    return Redirect(redirectURL);
                }
                else
                {
                    return Content("Token is invalid");
                }
            }
            catch (Exception e)
            {
                return Content("Token is invalid");
            }
            
        }

        protected bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
