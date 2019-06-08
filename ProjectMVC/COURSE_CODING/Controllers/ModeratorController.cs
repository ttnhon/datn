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

        public ActionResult EditCompete(int id)
        {
            if(ModelState.IsValid)
            {
                CompeteDetailModel model = new CompeteDetailModel();
                var c = (new CompeteDAO().GetOne(id));
                model.ID = c.ID;
                model.OwnerID = c.OwnerID;
                model.Title = c.Title;
                model.Description = c.Description;
                model.TimeEnd = c.TimeEnd;
                model.isPublic = c.IsPublic;
                return View(model);
            }
            return View();

        }

        public ActionResult CreateCompete()
        {
            CompeteDetailModel model = new CompeteDetailModel();
            return View(model);
        }

        //GET: /Moderator/CreateQuestion/{id}
        [HttpGet]
        public ActionResult CreateQuestion(int id)
        {
            //CreateChallengeModel model = new CreateChallengeModel();
            return View(id);
        }

        //GET: /Moderator/EditQuestion/{id}
        [HttpGet]
        public ActionResult EditQuestion(int id)
        {
            QuestionDAO DAO = new QuestionDAO();
            EditQuestionModel model = new EditQuestionModel()
            {
                ID = id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateQuestion(Question[] question, int competeID)
        {
            QuestionDAO DAO = new QuestionDAO();
            for(int i = 0; i < question.Length; i++)
            {
                QUESTION ques = new QUESTION();
                ques.CompeteID = competeID;
                ques.Title = question[i].Description;
                ques.Score = question[i].Score;
                ques.Type = question[i].Type.Equals("single") ? (short)0 : (short)1;
                List<string> value = new List<string>();
                List<int> result = new List<int>();
                for (int j = 0; j < question[i].List.Length; j++)
                {
                    value.Add('"' + question[i].List[j].Value + '"');
                    if (question[i].List[j].Result)
                    {
                        result.Add(j);
                    }
                }

                //question choise and result
                ques.Choise = "[" + string.Join(", ", value) + "]";
                ques.Result = "[" + string.Join(", ", result) + "]";

                //insert ques to table QUESTION
                bool res = DAO.Insert(ques);
                if (!res)
                {
                    return Json(new { result = false, msg = "Fail to create question " + i });
                }
            }
            return Json(new { result = true, msg = "Create Question succeed." });
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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateCompete(CompeteDetailModel model)
        {
            CompeteDAO DAO = new CompeteDAO();
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null)
            {
                return Json(new { result = false });
            }

            COMPETE c = new COMPETE()
            {
                OwnerID = model.OwnerID,
                Title = model.Title,
                Description = model.Description,
                TimeEnd = model.TimeEnd,
                IsPublic = model.isPublic
            };
            //add to table CHALLENGE
            bool res = DAO.Insert(c);
            //add to table CHALLENGE_EDITOR
            return Json(new { result = false });
        }

        [HttpPost]
        public ActionResult EditCompete(CompeteDetailModel model)
        {
            if (ModelState.IsValid)
            {
                COMPETE c = new COMPETE();
                c.ID = model.ID;
                c.OwnerID = model.OwnerID;
                c.Title = model.Title;
                c.Description = model.Description;
                c.TimeEnd = model.TimeEnd;
                c.IsPublic = model.isPublic;
                var DAO = new CompeteDAO();
                var result = DAO.Update(c);
            }
            return View();

        }
    }
}