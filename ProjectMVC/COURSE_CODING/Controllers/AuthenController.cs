﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BotDetect.Web.Mvc;
using CommonProject;
using COURSE_CODING.Common;
using COURSE_CODING.Models;
using DAO.DAO;
using DAO.EF;
using Facebook;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace COURSE_CODING.Controllers
{
    public class AuthenController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Incorrect CAPTCHA code!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var DAO = new UserDAO();
                if (DAO.CheckUserNameExist(model.UserName))
                {
                    ModelState.AddModelError(String.Empty, "This user name is existed");
                }
                else if (DAO.CheckUserNameExist(model.Email))
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
                    Boolean result = DAO.Insert(u);
                    if (result)
                    {
                        var sessionLogin = new InfoLogIn();
                        sessionLogin.ID = u.ID;
                        sessionLogin.Name = u.UserName;
                        Session.Add(CommonConstant.SESSION_INFO_LOGIN, sessionLogin);
                        ViewBag.Success = "Register sussesfull";
                        model = new RegisterModel();
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
                    if (user.RoleUser.Equals(CommonConstant.ROLE_ADMIN))
                    {
                        return Redirect("/Admin/User/Index");
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
            if (type == "Google")
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new
                    AuthenticationProperties
                { RedirectUri = "Authen/GoogleLoginCallback" }, "Google");
            }
        }

        [AllowAnonymous]
        public ActionResult GoogleLoginCallback()
        {
            var claimsPrincipal = HttpContext.User.Identity as ClaimsIdentity;
            var loginInfo = GoogleLoginViewModel.GetLoginInfo(claimsPrincipal);
            if (loginInfo == null)
            {
                return RedirectToAction("/");
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
                userSession.Role = CommonConstant.ROLE_MEMBER;
                Session.Add(CommonConstant.SESSION_INFO_LOGIN, userSession);
                // Set the auth cookie
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return Redirect("/User/Dashboard");
            }
            return Redirect("~/");
        }

        public ActionResult LoginFace()
        {
            var fa = new FacebookClient();
            var loginFace = fa.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
              //  redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            }
                );
            return Redirect(loginFace.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fa = new FacebookClient();
            var DAO = new UserDAO();
            dynamic result = fa.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
               // redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fa.AccessToken = accessToken;
                dynamic me = fa.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string id = me.id;
                string firstName = me.first_name;
                string middleName = me.middle_name;
                string lastName = me.last_name;
                string userName = firstName + " " + middleName + " " + lastName;
                var user = DAO.GetByName(userName);
                Boolean canLogin = false;
                if (user == null)
                {
                    user = new USER_INFO();
                    string ID = Guid.NewGuid().ToString();
                    user.FirstName = firstName;
                    user.LastName = lastName;
                    if (email == null)
                    {
                        email = ID + "gmail.com";
                    }
                    user.Email = email;
                    user.RoleUser = CommonConstant.ROLE_MEMBER;
                    user.StatusUser = CommonConstant.STATUS_RIGHT_ACCOUNT;
                    user.UserName = firstName + " " + middleName + " " + lastName;
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
                    userSession.Role = CommonConstant.ROLE_MEMBER;
                    Session.Add(CommonConstant.SESSION_INFO_LOGIN, userSession);
                    // Set the auth cookie
                    FormsAuthentication.SetAuthCookie(email, false);
                    return Redirect("/User/Dashboard");
                }
            }
            return Redirect("/");
        }
        /// <summary>
        /// object help catch data call back 
        /// </summary>
        //private Uri RedirectUri
        //{
        //    get
        //    {
        //        var uriBuilder = new UriBuilder(Request.Url);
        //        uriBuilder.Query = null;
        //        uriBuilder.Fragment = null;
        //        uriBuilder.Path = Url.Action("FacebookCallback");
        //        return uriBuilder.Uri;
        //    }
        //}
    }
}
