using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COURSE_CODING.Models;
using DAO.DAO;

namespace COURSE_CODING.Controllers
{
    public class ChallengeController : BaseController
    {
        // GET: Challenge
        public ActionResult Problem( int id)
        {
            
            //Prepare model
            ChallengeModel model = new ChallengeModel();

            //Fill data
            model.challenge = (new ChallengeDAO()).GetOne(id);
            model.OwnerName = (new UserDAO()).GetNameByID(model.challenge.OwnerID);
            model.languages = (new LanguageDAO()).GetByChallengeID(id);

            return View(model);
        }

        // GET: Challenge/edit/:id
        public ActionResult Edit(int id)
        {
            EditChallengeModel model = new EditChallengeModel()
            {
                Slug = "test-1",
                Name = "test 1"
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditChallengeModel model)
        {
            return null;
        }

        [HttpPost]
        public JsonResult UpdateDetails(EditChallengeModel model)
        {
            return Json(new { model });
        }
    }
}