using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using CommonProject;
using COURSE_CODING.Common;
using COURSE_CODING.Models;
using DAO.DAO;
using DAO.EF;

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

        public ActionResult Discussion(int id)
        {
            var models = new CommentListModel();

            //Lay user login info
            models.Info = (new UserDAO().GetUserById(1));
            models.challenge = (new ChallengeDAO().GetOne(id));

            var commentList = (new CommentDAO().GetAllByChallenge(id));
            if(commentList.Count > 0)
            {
                for (int i = 0; i < commentList.Count; i++)
                {
                    var model = new CommentModel();
                    model.comment = commentList[i];
                    model.owner = (new UserDAO().GetUserById(model.comment.OwnerID));

                    var replyList = (new ReplyDAO().GetAllByComment(model.comment.ID));
                    if (replyList.Count > 0)
                    {
                        for (int j = 0; j < replyList.Count; j++)
                        {
                            var reply = new ReplyModel();
                            reply.reply = replyList[j];
                            reply.owner = (new UserDAO().GetUserById(reply.reply.OwnerID));

                            model.replies.Add(reply);
                        }
                    }

                    models.comments.Add(model);
                }
            }

            return View(models);
        }

        [HttpPost]
        public ActionResult AddComment(int userID,int challengeID,string commentInput)
        {
            if(ModelState.IsValid)
            {
                var commentDAO = new CommentDAO();
                COMMENT c = new COMMENT();
                c.Text = commentInput;
                c.Likes = 0;
                c.OwnerID = userID;
                c.ChallengeID = challengeID;
                c.CreateDate = DateTime.Now;
                Boolean result = commentDAO.Insert(c);
                if(result)
                {
                    return Json(result);
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Fail to add comment");
                }
            }
            return View("Discussion");
        }

        [HttpPost]
        public ActionResult AddReply(int userID, int commentID, string input)
        {
            if (ModelState.IsValid)
            {
                var replyDAO = new ReplyDAO();
                REPLY c = new REPLY();
                c.Text = input;
                c.Likes = 0;
                c.OwnerID = userID;
                c.CommentID = commentID;
                c.CreateDate = DateTime.Now;
                Boolean result = replyDAO.Insert(c);
                if (result)
                {
                    return Redirect(String.Format("/Problem/{0}/Discussion", c.COMMENT.ChallengeID));
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Fail to add comment");
                }
            }
            return View("Discussion");
        }

        // GET: Challenge/edit/:id
        public ActionResult Edit(int id)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            CHALLENGE c = DAO.GetOne(id);
            if(c != null)
            {
                EditChallengeModel model = new EditChallengeModel()
                {
                    ID = id,
                    Slug = c.Slug,
                    Name = c.Title,
                    Difficulty = (Difficulty)c.ChallengeDifficulty,
                    Description = c.Description,
                    ProblemStatement = "",
                    InputFormat = c.InputFormat,
                    Constraints = c.Constraints,
                    OutputFormat = c.OutputFormat,
                    Tags = c.Tags,
                    Moderators = DAO.GetModeratorByChallengeID(id),
                    TestCases = DAO.GetTestCaseByID(id),
                    CodeStubs_CSharp = "CSharp",
                    CodeStubs_Cpp = "Cpp",
                    CodeStubs_Java = "Java"
                };
                return View(model);
            }
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Edit(EditChallengeModel model)
        {
            return null;
        }

        [HttpPost]
        public JsonResult UpdateDetails(EditChallengeModel model)
        {
            CHALLENGE c = new CHALLENGE()
            {
                ID = model.ID,
                Title = model.Name,
                Slug = CommonProject.Helper.SlugGenerator.GenerateSlug(model.Name),
                Description = model.Description,
                InputFormat = model.InputFormat,
                OutputFormat = model.OutputFormat,
                ChallengeDifficulty = (short)model.Difficulty,
                Constraints = model.Constraints,
                Tags = model.Tags
            };
            ChallengeDAO DAO = new ChallengeDAO();

            bool res = DAO.Update(c);
            if (res)
            {
                return Json(new { result = true, data = c });
            }
            return Json(new { result = false });
        }

        [HttpPost]
        public JsonResult AddModerator(string moderator, int challengeID)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            USER_INFO person = DAO.GetUserByName(moderator);
            if(person != null)
            {
                CHALLENGE_EDITOR entity = new CHALLENGE_EDITOR();
                entity.ChallegenID = challengeID;
                entity.EditorID = person.ID;
                //entity.USER_INFO = person;
                //entity.CHALLENGE = DAO.GetOne(challengeID);
                bool res = DAO.AddModerator(entity);
                if (res)
                {
                    return Json(new { result = true, data = new { person.UserName, person.PhotoURL, person.FirstName, person.LastName } });
                }
                else
                {
                    return Json(new { result = false });
                }
            }
            else
            {
                return Json(new { result = false });
            }
        }

        public JsonResult AddTestCase(string input, string output, int challengeID)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            int testcaseID = DAO.GetTestCaseNextID(challengeID);
            TESTCASE t = new TESTCASE();
            FileManager fileInput = new FileManager();
            fileInput.Content = input;
            //filename = challenge_{id_challenge}_input_{id_testcase}
            fileInput.FileName = "challenge_" + challengeID + "_input_" + testcaseID;
            //fileInput.userKey = 1;
            FileManager fileOutput = new FileManager();
            fileOutput.Content = output;
            //filename = challenge_{id_challenge}_output_{id_testcase}
            fileOutput.FileName = "challenge_" + challengeID + "_output_" + testcaseID;

            //call api upload file
            API_Helper apiHelperInput = new API_Helper();
            t.ChallengeID = challengeID;
            var resInput = apiHelperInput.RequestUploadAPI(fileInput, CommonConstant.TYPE_UPLOAD_FILE_API);
            if (!resInput.Equals("success"))
            {
                return Json(new { result = false });
            }
            t.Input = fileInput.FileName + ".txt";
            API_Helper apiHelperOutput = new API_Helper();
            var resOutput = apiHelperOutput.RequestUploadAPI(fileOutput, CommonConstant.TYPE_UPLOAD_FILE_API);
            if (!resOutput.Equals("success"))
            {
                return Json(new { result = false });
            }
            t.Output = fileOutput.FileName + ".txt";
            
            //call DAO to add test case
            bool res = DAO.AddTestCase(t);
            if (res)
            {
                return Json(new { result = true, data = t, count = testcaseID });
            }
            return Json(new { result = false });
        }

        public JsonResult UpdateCodeStubs(int challengeID, CodeStubs Code)
        {
            return Json(new { Code });
        }

        public JsonResult UpdateLanguages(EditChallengeModel model)
        {
            return Json(new { model });
        }

        public JsonResult UpdateSettings(EditChallengeModel model)
        {
            return Json(new { model });
        }

        public JsonResult UpdateEditorial(EditChallengeModel model)
        {
            return Json(new { model });
        }
    }
}