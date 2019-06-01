using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session[CommonConstant.SESSION_INFO_LOGIN] = null;
            return Redirect("/");
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
                        else
                        {
                            continue;
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
        public ActionResult Edit(USER_INFO user)
        {
            return View();
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

       

        // GET: User/Profile/5
        [HttpGet]
        public new ActionResult Profile(int id)
        {
            if(ModelState.IsValid)
            {
                UserProfileModel model = new UserProfileModel();
                model.Info = (new UserDAO().GetUserById(id));
                model.FirstName = model.Info.FirstName;
                model.LastName = model.Info.LastName;
                model.Country = model.Info.Country;  
                model.Competes = (new CompeteDAO().GetTen(id));
                model.School = (new SchoolDAO().GetSchoolByID(model.Info.SchoolID));
                SetSchoolViewBag(model.Info.SchoolID);
                SetYearViewBag(model.Info.YearGraduation);

                var ChallengeDones = (new AnswerDAO().GetChallengesDone(id));
                for(int i = 0; i < ChallengeDones.Count; i++)
                {
                    var ChallengeDone = new DoneChallenge();
                    ChallengeDone.challenge = ChallengeDones[i];
                    ChallengeDone.timeDone = (new AnswerDAO().GetTimeDoneByChallenge(ChallengeDones[i].ID));

                    model.Challenges.Add(ChallengeDone);
                }
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditIntro(UserProfileModel user)
        {
            if (ModelState.IsValid)
            {
                var userDAO = new UserDAO();
                user.Info.FirstName = user.FirstName;
                user.Info.LastName = user.LastName;
                user.Info.Country = user.Country;
                var result = userDAO.UpdateIntro(user.Info);
                if (result)
                {
                    return Redirect(String.Format("/User/Profile/{0}", user.Info.ID));
                }
                else
                {
                    ModelState.AddModelError("", "Update Success!");
                }
            }
            return View("Profile",user);
        }

        [HttpPost]
        public ActionResult EditAbout(UserProfileModel user)
        {
            if (ModelState.IsValid)
            {
                var userDAO = new UserDAO();
                var result = userDAO.UpdateAbout(user.Info);
                if (result)
                {
                    return Redirect(String.Format("/User/Profile/{0}", user.Info.ID));
                }
                else
                {
                    ModelState.AddModelError("", "Update Success!");
                }
            }
            return View("Profile",user.Info.ID);
        }

        [HttpPost]
        public ActionResult Upload(UserProfileModel user, HttpPostedFileBase file)
        {
            var userDAO = new UserDAO();

            if (file.ContentLength > 0)
            {
                string _fileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/Assets/Public/images/profile_photos"), _fileName);
                file.SaveAs(_path);

                var result = userDAO.UpdatePhoto(user.Info, _fileName);
                if (result)
                {
                    return Redirect(String.Format("/User/Profile/{0}", user.Info.ID));
                }
                else
                {
                    ModelState.AddModelError("", "Update Success!");
                }
            }
            return View();
        }

        public void SetSchoolViewBag(int? selectedID = null)
        {
            ViewBag.School = new SelectList(new SchoolDAO().GetList(), "ID", "Name", selectedID);
        }

        public void SetYearViewBag(int? selectedID = null)
        {
            List<SelectListItem> yearList = new List<SelectListItem>();
            for (int i = DateTime.Now.Year; i < (DateTime.Now.Year + 6); i++)
            {
                SelectListItem year = new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                };
                yearList.Add(year);
            }
            yearList.Add(new SelectListItem { Value="0",Text="I am still in HighSchool"});

            ViewBag.Year = new SelectList(yearList, "Value", "Text", selectedID);
        }
    }

}
