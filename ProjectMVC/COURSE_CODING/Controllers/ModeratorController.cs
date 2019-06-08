using CommonProject;
using COURSE_CODING.Common;
using COURSE_CODING.Models;
using DAO.DAO;
using DAO.EF;
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
            if (session == null)
            {
                return Redirect("Authen/Login");
            }
            int userId = session.ID;
            //Prepare model
            ManageChallengesModel model = new ManageChallengesModel();

            model.userId = userId;
            model.lsChallenges = (new ChallengeDAO()).GetAllByEditorId(userId);

            return View(model);
        }

        public ActionResult CreateChallenge()
        {
            CreateChallengeModel model = new CreateChallengeModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateQuestion()
        {
            //CreateChallengeModel model = new CreateChallengeModel();
            return View("CreateQuestion");
        }

        //GET: /Moderator/EditQuestion/{id}
        [HttpGet]
        public ActionResult EditQuestion(int id)
        {
            EditQuestionModel model = new EditQuestionModel()
            {
                ID = id
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateQuestion(Question[] question)
        {
            return Json(new { result = false });
        }

        [HttpPost]
        public ActionResult EditQuestion(Question[] question, int competeID)
        {
            return Json(new { result = false });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateChallenge(CreateChallengeModel model)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null)
            {
                return Json(new { result = false });
            }

            CHALLENGE c = new CHALLENGE()
            {
                OwnerID = ses.ID,
                Title = model.Name,
                Slug = CommonProject.Helper.SlugGenerator.GenerateSlug(model.Name),
                Score = model.Score,
                Description = model.Description,
                ProblemStatement = model.ProblemStatement,
                InputFormat = model.InputFormat,
                OutputFormat = model.OutputFormat,
                ChallengeDifficulty = (short)model.Difficulty,
                Constraints = model.Constraints,
                Tags = model.Tags
            };
            //add to table CHALLENGE
            bool res = DAO.Insert(c);
            //add to table CHALLENGE_EDITOR
            CHALLENGE_EDITOR editor = new CHALLENGE_EDITOR();
            editor.ChallegenID = c.ID;
            editor.EditorID = ses.ID;
            res = DAO.AddModerator(editor);
            if (res)
            {
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }
    }
}