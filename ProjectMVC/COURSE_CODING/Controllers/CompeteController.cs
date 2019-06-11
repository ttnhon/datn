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
        // GET: Compete
        public ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                CompeteModel model = new CompeteModel();    //Model
                //Prepare data
                CompeteDAO competeDao = new CompeteDAO();
                LanguageDAO languageDao = new LanguageDAO();

                //model.languages = languageDao.GetList();
                model.competes = competeDao.GetAll();
                return View(model);
            }
            return View();
        }

        // GET: Compete/Detail/id
        public ActionResult Detail(int id)
        {
            if (ModelState.IsValid)
            {
                CompeteChallengesModel model = new CompeteChallengesModel();    //Model
                //Prepare data
                CompeteDAO competeDao = new CompeteDAO();
                ChallengeDAO challenge = new ChallengeDAO();

                model.compete = competeDao.GetOne(id);
                model.challenges = challenge.GetAllByCompeteID(id);
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