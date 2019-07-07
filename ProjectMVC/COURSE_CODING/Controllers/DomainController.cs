using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COURSE_CODING.Models;
using DAO.DAO;

namespace COURSE_CODING.Controllers
{
    public class DomainController : BaseController
    {
        // GET: Domain
        // Domain/index/:language
        public ActionResult Index(string id)
        {
            if (ModelState.IsValid)
            {
                var session = (COURSE_CODING.Common.InfoLogIn)Session[CommonProject.CommonConstant.SESSION_INFO_LOGIN];
                DomainModel model = new DomainModel();
                LanguageDAO DAO = new LanguageDAO();
                model.Language = DAO.GetLanguageByName(id);
                model.ListSolved = DAO.GetSolvedAndPublic(model.Language.Name, session.ID);
                model.ListUnsolved = DAO.GetUnsolvedAndPublic(model.Language.Name, session.ID);
                return View(model);
            }
            return View();
        }

        // GET: Domain
        // Domain/index/:language?status=all&diff=all
        //public ActionResult Index(string language, string status = "all", string diff = "all")
        //{
        //    //get data language
        //    //get data challenge list
        //    //prepare data
        //    //load view
        //    if (ModelState.IsValid)
        //    {
        //        DomainModel model = new DomainModel();
        //        LanguageDAO DAO = new LanguageDAO();
        //        model.status = status;
        //        model.difficulty = diff;
        //        model.Language = DAO.GetLanguageByName(language);
        //        model.List = DAO.GetChallenge(model.Language.Name);
        //        return View(model);
        //    }
        //    return View();
        //}
        
    }
}