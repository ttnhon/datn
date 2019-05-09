using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using CommonProject;
using COURSE_CODING.Common;
using COURSE_CODING.Models;
using DAO.DAO;
using DAO.EF;
using Facebook;

namespace COURSE_CODING.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            //1.GET DATA 
                //get data archieve

                //get data skill

                //get data challenge list

                //get language

                //get add data(*)
            //2.PREPARE DATA TO VIEW

            //3.LOAD VIEW


            if (ModelState.IsValid)
            {
                var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
                UserDashboardModel model = new UserDashboardModel();
                LanguageDAO DAO = new LanguageDAO();
                model.Languages = DAO.GetList();
                model.Data.UserCompetes = DAO.GetCompeteCount();
                model.Data.AvailableLanguages = model.Languages.Count;
                if(ses != null)
                {
                    model.Data.SuccessChellenges = DAO.GetNumberSuccessChallengeByID(ses.ID);
                    
                    foreach (var item in model.Languages)
                    {
                        //get skill list
                        int count = DAO.GetAnswerCountByID(ses.ID, item.Name);
                        if (count > 0)
                        {
                            Skill skill = new Skill();
                            skill.Language = item;
                            skill.Solved = count;
                            skill.Count = DAO.GetChallengeCount(item.Name);
                            model.Skills.Add(skill);
                        }
                        //get next challenge list
                        CHALLENGE c = DAO.GetNextChallengeByID(ses.ID, item.Name);
                        if(c != null)
                        {
                            model.Challenges.Add(c);
                        }
                    }
                }
                return View(model);
            }
            return View("Dashboard");
        }

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
                }else if (DAO.CheckUserNameExist(model.Email))
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
                    Boolean result = DAO.Insert(u);
                    if (result)
                    {
                        var sessionLogin = new InfoLogIn();
                        sessionLogin.ID = u.ID;
                        sessionLogin.Name = u.UserName;
                        Session.Add(CommonConstant.SESSION_INFO_LOGIN, sessionLogin);
                        ViewBag.Success = "Register sussesfull";
                        model = new RegisterModel();
                        return Redirect("/");
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
            return View();
        }

       
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var DAO = new UserDAO();
                var statusLogin = DAO.Login(model.UserName,HashMD5.HashStringMD5(model.Password));
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
                        //return Redirect("databoardAdmin");
                    }
                    else if(user.RoleUser.Equals(CommonConstant.ROLE_MEMBER)) {
                        return Redirect("/User/Dashboard");
                    }
                    else if(user.RoleUser.Equals(CommonConstant.ROLE_TEACHER))
                    {
                       // return Redirect("databoardTeacher");
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

        public ActionResult LoginFace()
        {
            var fa = new FacebookClient();
            var loginFace = fa.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            }
                );
            return Redirect(loginFace.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fa = new FacebookClient();
            dynamic result = fa.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fa.AccessToken = accessToken;    
                dynamic me = fa.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                var user = new USER_INFO();
                user.Email = email;
                user.StatusUser = CommonConstant.STATUS_RIGHT_ACCOUNT;
                user.UserName = firstname + " " + middlename + " " + lastname;
               // user.CreateDate= DateTime.Now;
                Boolean canLogin = new UserDAO().Insert(user);
                if(canLogin.Equals(true))
                {
                    var userSession = new InfoLogIn();
                    userSession.Name = user.UserName;
                    userSession.ID = user.ID;
                    Session.Add(CommonConstant.SESSION_INFO_LOGIN, userSession);
                }
            }
            return Redirect("/");
        }

        public ActionResult Logout()
        {
            Session[CommonConstant.SESSION_INFO_LOGIN] = null;
            return Redirect("/");
        }
        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// object help catch data call back 
        /// </summary>
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult Profile(int id)
        {
            UserProfileModel model = new UserProfileModel();
            model.Challenges = (new ChallengeDAO().GetAll(id));
            model.Competes = (new CompeteDAO().GetAll(id));
            return View(model);
        }
    }

}
