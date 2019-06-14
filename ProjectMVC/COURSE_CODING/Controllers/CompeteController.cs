using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COURSE_CODING.Models;
using DAO.DAO;
using DAO.EF;
using Newtonsoft.Json;

namespace COURSE_CODING.Controllers
{
    public class CompeteController : BaseController
    {
        protected int GetLoginID()
        {
            var session = (COURSE_CODING.Common.InfoLogIn)Session[CommonProject.CommonConstant.SESSION_INFO_LOGIN];
            if (session != null)
            {
                return session.ID;
            }
            else
            {
                return -1;
            }
        }

        // GET: Compete
        public ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                int userID = this.GetLoginID();
                if (userID <= 0) return Redirect("Authen/Login");
                CompeteModel model = new CompeteModel();    //Model
                //Prepare data
                CompeteDAO competeDao = new CompeteDAO();
                LanguageDAO languageDao = new LanguageDAO();
                
                model.competes = competeDao.GetJoined(userID);
                model.PublicCompetes = competeDao.GetPublic(userID);
                //ViewBag.Joined = true;
                //ViewBag.Unjoined = true;
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public JsonResult EnterPublicContest(int id)
        {
            CompeteDAO DAO = new CompeteDAO();
            bool? isPublic = DAO.IsPublic(id);
            if(isPublic == null)
            {
                return Json(new { result = false, msg = "Contest doesn't exist." });
            }
            if ((bool)isPublic)
            {
                int userID = this.GetLoginID();
                bool res = DAO.EnterCompete(userID, id);
                if (res)
                {
                    return Json(new { result = true, msg = "Enter contest succeed." });
                }
            }
            return Json(new { result = false, msg = "fail to enter contest." });
        }

        // GET: Compete/Detail/id
        public ActionResult Detail(int id)
        {
            if (ModelState.IsValid)
            {
                int userID = this.GetLoginID();
                ViewBag.CanAccess = true;
                ViewBag.Title = "can't access this Compete!";
                //Check User permission access to Compete
                CompeteDAO competeDao = new CompeteDAO();
                bool can_access = competeDao.CanAccess(id, userID);
                if (!can_access)
                {
                    ViewBag.CanAccess = false;
                    return View("Detail.cshtml");
                }

                CompeteChallengesModel model = new CompeteChallengesModel();    //Model

                //Prepare data
                model.compete = competeDao.GetOne(id);

                //Get list Challenge and check user is solved challenge
                dynamic questions = (new QuestionDAO()).GetAllWithAnswerByCompeteID(id, userID);
                foreach (var one_question in questions)         //Check question is answered
                {
                    var chosen = one_question.GetType().GetProperty("Chosen").GetValue(one_question, null);
                    if (chosen == null || chosen.Content.Equals("[]"))
                    {
                        model.questions.Add(false);
                    }
                    else
                    {
                        model.questions.Add(true);
                    }
                }
                //Get list Challenge and check user is solved challenge
                dynamic challenges = new ChallengeDAO().GetAllWithAnswerByCompeteID(id, userID);
                foreach (var challenge in challenges)         //Parse data
                {
                    UserChallengeStatus one = new UserChallengeStatus();
                    one.ID = challenge.GetType().GetProperty("ID").GetValue(challenge, null);
                    one.Title = challenge.GetType().GetProperty("Title").GetValue(challenge, null);
                    one.Difficulty = challenge.GetType().GetProperty("Difficulty").GetValue(challenge, null);
                    one.Score = challenge.GetType().GetProperty("Score").GetValue(challenge, null);
                    one.isSolved = challenge.GetType().GetProperty("isSolved").GetValue(challenge, null);
                    model.challenges.Add(one);
                }

                ViewBag.Title = model.compete.Title;
                ViewBag.CompeteID = id;
                return View("Detail", model);
                //return Json(model);
            }
            return View();
        }

        public ActionResult Invitation(int id)
        {
            var loginID = GetLoginID();
            var competeDAO = new CompeteDAO();
            var userDAO = new UserDAO();
            if (competeDAO.CheckParticipantExist(loginID))
            {
                ViewBag.CanAccess = true;
                InvitationModel model = new InvitationModel();
                var compete = competeDAO.GetOne(id);
                model.contestID = id;
                model.contestName = compete.Title;
                model.contestOwner = userDAO.GetUserById(compete.OwnerID);
                ViewBag.Title = model.contestName;
                return View("Invitation", model);
            }
            else
            {
                ViewBag.Title = "can't access this Compete!";
                ViewBag.CanAccess = false;
                return View("Invitation");
            }
        }

        public ActionResult AcceptInvitation(int id)
        {
            return Redirect("/Compete/Detail/" + id);
        }

        public ActionResult DeclineInvitation(int id)
        {
            var competeDAO = new CompeteDAO();
            COMPETE_PARTICIPANTS model = new COMPETE_PARTICIPANTS();
            model.CompeteID = id;
            model.UserID = GetLoginID();
            var result = competeDAO.DeleteParticipant(model);
            if(result)
            {
                return View("Index.cshtml");
            }
            return View();
        }

        //public ActionResult VerifyEmail(string crypt)
        //{
        //    string emailDecrypt = CommonProject.Helper.Encrypt.DecryptString(crypt, "123456");
        //    string[] ID = emailDecrypt.Split('_');

        //    var CompeteParticipantDAO = new CompeteParticipantDAO();

        //    var result = CompeteParticipantDAO.Insert(ID[1], ID[2]);
        //    if(result)
        //    {
        //        return Redirect(String.Format("/Compete/{0}", ID[2]));
        //    }
        //    return View();

        //}
    }
}