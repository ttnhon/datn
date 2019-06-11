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
                    return View();
                }

                CompeteChallengesModel model = new CompeteChallengesModel();    //Model

                //Prepare data
                model.compete = competeDao.GetOne(id);
                model.challenges = new ChallengeDAO().GetAllByCompeteID(id);

                dynamic questions = (new QuestionDAO()).GetAllWithAnswerByCompeteID(id, this.GetLoginID());
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
                ViewBag.Title = model.compete.Title;
                ViewBag.CompeteID = id;
                return View(model);
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