using COURSE_CODING.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COURSE_CODING.Controllers
{
    public class ManageChallengeController : BaseController
    {
        // GET: ManageChallenge
        public ActionResult ManageChallenge()
        {
            //Prepare model
            ManageChallengesModel  model = new ManageChallengesModel();

            return View(model);
        }
    }
}