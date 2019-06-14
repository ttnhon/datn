using CommonProject;
using COURSE_CODING.Common;
using COURSE_CODING.Models;
using DAO.DAO;
using DAO.EF;
using Newtonsoft.Json;
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
                model.Questions = (new QuestionDAO().GetAllByCompeteID(id));
                return View(model);
            }
            return View();
        }

        public ActionResult RenderQuestionView(int id)
        {
            var questionDAO = new QuestionDAO();
            List<QUESTION> questionList = new List<QUESTION>();
            questionList = questionDAO.GetAllByCompeteID(id);

            return PartialView("_QuestionList", questionList);
        }

        public ActionResult RenderParticipantView(int id)
        {
            var competeDAO = new CompeteDAO();
            var userDAO = new UserDAO();
            ParticipantList model = new ParticipantList();
            model.ContestID = id;
            var participantList = competeDAO.GetParticipantList(id);
            for(int i=0;i<participantList.Count;i++)
            {
                var info = userDAO.GetUserById(participantList[i]);

                Participant p = new Participant();
                p.ID = info.ID;
                p.Name = info.UserName;
                p.Email = info.Email;
                model.Participants.Add(p);
            }

            return PartialView("_ParticipantList", model);
        }

        [HttpPost]
        public ActionResult SendInvitation(int contestID,string email)
        {
            if(email!=null)
            {
                var user = (new UserDAO().GetUserByEmail(email));
                var contest = (new CompeteDAO().GetOne(contestID));
                string emailHeader = String.Format("{0} invited you to participate in the {1} contest.", user.UserName, contest.Title);
                string emailContent = String.Format("@{0} has invited you to participant in the {1}contest. You can accept or deline following the link below:/nhttp://localhost:49512/Compete/{2}Invitation", user.UserName, contest.Title, contest.ID);
                CommonProject.Helper.Email_Helper emailHelper = new CommonProject.Helper.Email_Helper();
                emailHelper.SendMail(email, emailHeader, emailContent);
                COMPETE_PARTICIPANTS model = new COMPETE_PARTICIPANTS();
                model.CompeteID = contestID;
                model.UserID = user.ID;
                model.TimeJoined = DateTime.Now;
                if(new CompeteDAO().CheckParticipantExist(user.ID))
                {
                    return Json("This email is exist in this contest! Please enter another email");
                }
                var result = (new CompeteDAO().InsertParticipant(model));
                if (result)
                {
                    Participant p = new Participant();
                    p.ID = user.ID;
                    p.Name = user.UserName;
                    p.Email = user.Email;
                    return Json(p, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failt to send invitation");
                }
            }
            return Json("Fail to send invitation");
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
            var list = DAO.GetAllByCompeteID(id);
            for(int i = 0; i < list.Count; i++)
            {
                Question ques = new Question();
                ques.ID = list[i].ID;
                ques.Description = list[i].Title;
                ques.Score = (int)list[i].Score;
                ques.Type = list[i].Type;

                //ques.list
                string[] val = JsonConvert.DeserializeObject<string[]>(list[i].Choise);
                int[] res = JsonConvert.DeserializeObject<int[]>(list[i].Result);
                List<Answer> answers = new List<Answer>();
                for (int index = 0; index < val.Length; index++)
                {
                    Answer temp = new Answer();
                    temp.Value = val[index];
                    if (res.Contains(index))
                    {
                        temp.Result = true;
                    }
                    else
                    {
                        temp.Result = false;
                    }
                    answers.Add(temp);
                }
                ques.List = answers.ToArray();
                model.Questions.Add(ques);
            }
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
                ques.Type = question[i].Type;
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
            return Json(new { result = true, msg = "Create question succeed." });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditQuestion(Question[] question, int competeID)
        {
            QuestionDAO DAO = new QuestionDAO();
            for (int i = 0; i < question.Length; i++)
            {
                QUESTION ques = new QUESTION();
                ques.ID = question[i].ID;
                ques.Title = question[i].Description;
                ques.Score = question[i].Score;
                ques.Type = question[i].Type;
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

                //update ques to table QUESTION by id
                bool res = false;
                if(ques.ID == 0)
                {
                    //if question not exist insert to table QUESTION
                    ques.CompeteID = competeID;
                    res = DAO.Insert(ques);
                }
                else
                {
                    //update
                    res = DAO.Update(ques);
                }
                if (!res)
                {
                    return Json(new { result = false, msg = "Fail to update question " + i });
                }
            }
            return Json(new { result = true, msg = "Update questions succeed." });
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