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


namespace COURSE_CODING.Controllers
{
    public class CompeteController : Controller
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

                model.languages = languageDao.GetList();
                model.competes = competeDao.GetAll();
                return View(model);
            }
            return View();
        }
    }
}