using COURSE_CODING.Models;
using DAO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COURSE_CODING.Controllers
{
    public class ModeratorController : BaseModeratorController
    {
        [HttpGet]
        // GET: Moderator
        public ActionResult ManageChallenge()
        {
            var session = (COURSE_CODING.Common.InfoLogIn)Session[CommonProject.CommonConstant.SESSION_INFO_LOGIN];
            int userId = session.ID;
            //Prepare model
            ManageChallengesModel model = new ManageChallengesModel();

            model.userId = userId;
            model.lsChallenges = (new ChallengeDAO()).GetAllByEditorId(userId);

            return View(model);
        }
    }
}