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
            model.lsCompetes = (new CompeteDAO().GetAll(userId));

            return View(model);
        }

        [Route("Moderator/CreateChallenge")]
        [Route("Moderator/CreateChallenge/{competeID}")]

        public ActionResult CreateChallenge(int? competeID)
        {
            CreateChallengeModel model = new CreateChallengeModel();
            if (competeID != null)
                model.competeID = (int)competeID;
            return View(model);
        }

        public ActionResult EditCompete(int id)
        {
            if(ModelState.IsValid)
            {
                CompeteDetailModel model = new CompeteDetailModel();
                var c = (new CompeteDAO().GetOne(id));
                //check compete exist
                if(c == null)
                {
                    return Redirect("/Error/PageNotFound");
                }
                //check is owner
                var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
                if (ses.ID != c.OwnerID)
                {
                    ViewBag.CanAccess = false;
                    return View("EditCompete");
                }
                ViewBag.CanAccess = true;
                model.ID = c.ID;
                model.OwnerID = c.OwnerID;
                model.Title = c.Title;
                model.Description = c.Description;
                model.TimeEnd = (DateTime)c.TimeEnd;
                model.isPublic = c.IsPublic;
                model.Questions = (new QuestionDAO().GetAllByCompeteID(id));
                return View(model);
            }
            return View();
        }

        public ActionResult RenderChallengeView(int id)
        {
            var challengeDAO = new ChallengeDAO();
            List<CHALLENGE> challengeList = new List<CHALLENGE>();
            challengeList = challengeDAO.GetAllByCompeteID(id);
            ViewBag.CompeteID = id;

            return PartialView("_ChallengeList", challengeList);
        }

        public ActionResult RenderQuestionView(int id)
        {
            var questionDAO = new QuestionDAO();
            List<QUESTION> questionList = new List<QUESTION>();
            questionList = questionDAO.GetAllByCompeteID(id);
            ViewBag.CompeteID = id;
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
                var info = userDAO.GetUserById(participantList[i].UserID);

                Participant p = new Participant();
                p.ID = info.ID;
                p.Name = info.UserName;
                p.Email = info.Email;
                if (participantList[i].TimeJoined != null)
                {
                    p.TimeJoined = (DateTime)participantList[i].TimeJoined;
                }
                model.Participants.Add(p);
            }

            return PartialView("_ParticipantList", model);
        }

        public ActionResult RenderScoreView(int id)
        {
            //var session = (COURSE_CODING.Common.InfoLogIn)Session[CommonProject.CommonConstant.SESSION_INFO_LOGIN];
            //int userID = session.ID;
            CompeteDAO DAO = new CompeteDAO();
            //check compete exist
            var compete = DAO.GetOne(id);
            if (compete == null)
            {
                return Redirect("/Error/PageNotFound");
            }
            ////check is owner
            //if (compete.OwnerID != userID)
            //{
            //    ViewBag.CanAccess = false;
            //    return View("Score");
            //}
            ViewBag.CanAccess = true;
            ViewBag.competeID = id;
            ViewBag.Name = compete.Title;
            USER_INFO u = new UserDAO().GetUserById(compete.OwnerID);
            ViewBag.Author = u.FirstName + " " + u.LastName;
            ViewBag.TimeEnd = compete.TimeEnd.ToString();
            List<UserScore> Model = new List<UserScore>();
            //get list participants
            var paticipants = DAO.GetParticipants(id);
            foreach (var item in paticipants)
            {
                UserScore temp = new UserScore();
                temp.Name = item.FirstName + " " + item.LastName;
                temp.PhotoUrl = item.PhotoURL;

                temp.TotalScore = 0;
                //get score question
                temp.QuestionDone = 0;
                temp.ScoreQuestion = 0;

                dynamic questions = (new QuestionDAO()).GetAllWithAnswerByCompeteID(id, item.ID);
                temp.QuestionNumber = 0;

                foreach (var one_question in questions)
                {
                    temp.QuestionNumber++;
                    var question = one_question.GetType().GetProperty("Question").GetValue(one_question, null);
                    var chosen = one_question.GetType().GetProperty("Chosen").GetValue(one_question, null);
                    temp.TotalScore += question.Score;
                    if (chosen != null)
                    {
                        if (chosen.TimeDone <= compete.TimeEnd)
                        {
                            if (chosen.Result == 1)
                            {
                                temp.QuestionDone++;
                                temp.ScoreQuestion += question.Score;
                            }
                        }
                    }
                }
                //get score challenge
                temp.ChallengeDone = 0;
                temp.ScoreChallenge = 0;

                //Get list Challenge and check user is solved challenge
                dynamic challenges = new ChallengeDAO().GetAllWithAnswerByCompeteID(id, item.ID);
                temp.ChallengeNumber = 0;
                foreach (var challenge in challenges)         //Parse data
                {
                    temp.ChallengeNumber++;
                    bool isSolved = challenge.GetType().GetProperty("isSolved").GetValue(challenge, null);
                    int score = challenge.GetType().GetProperty("Score").GetValue(challenge, null);
                    DateTime timeDone = challenge.GetType().GetProperty("TimeDone").GetValue(challenge, null);
                    temp.TotalScore += score;

                    if (isSolved)
                    {
                        if (timeDone <= compete.TimeEnd)
                        {
                            temp.ChallengeDone++;
                            temp.ScoreChallenge += score;
                        }
                    }

                }

                //add to model
                Model.Add(temp);
            }
            return PartialView("_CompeteScore", Model);
        }

        [HttpPost]
        public ActionResult SendInvitation(int contestID,string email)
        {
            if(email!=null)
            {
                var userDAO = new UserDAO();
                var competeDAO = new CompeteDAO();

                var user = userDAO.GetUserByEmail(email);
                var compete = competeDAO.GetOne(contestID);

                if(!userDAO.CheckEmailExist(email))
                {
                    return Json(new { result=false, msg = "This email is not registered yet! Please enter another email" });
                }

                string emailHeader = String.Format("{0} invited you to participate in the {1} contest.", user.UserName, compete.Title);
                string emailContent = String.Format("@{0} has invited you to participant in the {1} contest. You can accept or deline following the link:\n http://localhost:49512/Compete/{2}/Invitation", user.UserName, compete.Title, compete.ID);
                CommonProject.Helper.Email_Helper emailHelper = new CommonProject.Helper.Email_Helper();
                emailHelper.SendMail(email, emailHeader, emailContent);
                COMPETE_PARTICIPANTS model = new COMPETE_PARTICIPANTS();
                model.CompeteID = contestID;
                model.UserID = user.ID;
                if(new CompeteDAO().CheckParticipantExist(user.ID))
                {
                    return Json(new { result = false, msg = "This email is exist in this contest! Please enter another email" });
                }
                var result = (new CompeteDAO().InsertParticipant(model));
                if (result)
                {
                    Participant p = new Participant();
                    p.ID = user.ID;
                    p.Name = user.UserName;
                    p.Email = user.Email;
                    return Json(new { data = p, result = true, msg = "Send invitation succeed!"}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, msg = "Fail to send invitation!" });
                }
            }
            return Json(new { result = false, msg = "Fail to send invitation!" });
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
            var c = new CompeteDAO().GetOne(id);
            //check compete exist
            if (c == null)
            {
                return Redirect("/Error/PageNotFound");
            }
            //check is owner
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            if (ses.ID != c.OwnerID)
            {
                ViewBag.CanAccess = false;
                return View("CreateQuestion");
            }
            ViewBag.CanAccess = true;
            //CreateChallengeModel model = new CreateChallengeModel();
            return View("CreateQuestion", id);
        }

        //GET: /Moderator/EditQuestion/{id}
        [HttpGet]
        public ActionResult EditQuestion(int id)
        {
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is compete exist, owner
            bool? isOwner = new CompeteDAO().IsOwner(id, ses.ID);
            if(isOwner == null)
            {
                return Redirect("/Error/PageNotFound");
            }
            if(isOwner == false)
            {
                ViewBag.CanAccess = false;
                return View("EditQuestion");
            }
            ViewBag.CanAccess = true;
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
            ViewBag.CompeteTitle = new CompeteDAO().GetOne(id).Title;
            return View("EditQuestion", model);
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
            List<int> InsertList = new List<int>();
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
                    InsertList.Add(ques.ID);
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
            return Json(new { result = true, msg = "Update questions succeed.", insertList = InsertList.ToArray() });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNewChallenge(CreateChallengeModel model)
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
            if(model.competeID != 0)
            {
                DAO.AddChallengetoCompete(c.ID, model.competeID);
            }
            //add to table CHALLENGE_EDITOR
            CHALLENGE_EDITOR editor = new CHALLENGE_EDITOR();
            editor.ChallegenID = c.ID;
            editor.EditorID = ses.ID;
            res = DAO.AddModerator(editor);
            if (res)
            {
                return Json(new { result = true, competeID = model.competeID, challengeID = c.ID });
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
                OwnerID = ses.ID,
                Title = model.Title,
                Slug = CommonProject.Helper.SlugGenerator.GenerateSlug(model.Title),
                Description = model.Description,
                Rules = model.Rules,
                TimeEnd = model.TimeEnd,
                IsPublic = model.isPublic
            };
            //add to table CHALLENGE
            bool res = DAO.Insert(c);
            //add to table CHALLENGE_EDITOR
            if(res)
            {
                return Json(new { result = true, competeID = c.ID });
            }
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

                return Json(result);
            }
            return View(model);

        }

        [HttpPost]
        public ActionResult DeleteParticipant(int contestID,int userID)
        {
            var competeDAO = new CompeteDAO();
            COMPETE_PARTICIPANTS c = new COMPETE_PARTICIPANTS();
            c.CompeteID = contestID;
            c.UserID = userID;
            var result = competeDAO.DeleteParticipant(c);
            if(result)
            {
                return Json("Delete succeed!");
            }
            else
            {
                return Json("Delete fail!");
            }
        }
        [HttpPost]
        public ActionResult DeleteChallenge(int contestID, int challengeID)
        {
            var challengeDAO = new ChallengeDAO();
            CHALLENGE_COMPETE c = new CHALLENGE_COMPETE();
            c.CompeteID = contestID;
            c.ChallengeID = challengeID;
            var result = challengeDAO.DeleteChallenge(c);
            if (result)
            {
                return Json("Delete succeed!");
            }
            else
            {
                return Json("Delete fail!");
            }
        }
        [HttpPost]
        public ActionResult DeleteQuestion(int contestID, int questionID)
        {
            var questionDAO = new QuestionDAO();
            QUESTION q = new QUESTION();
            q.CompeteID = contestID;
            q.ID = questionID;
            var result = questionDAO.Delete(q);
            if (result)
            {
                return Json("Delete succeed!");
            }
            else
            {
                return Json("Delete fail!");
            }
        }

        public ActionResult GetUserEmail()
        {
            var userDAO = new UserDAO();
            return Json(userDAO.GetAllUserEmailExcept(GetLoginID()),JsonRequestBehavior.AllowGet);
        }
    }
}