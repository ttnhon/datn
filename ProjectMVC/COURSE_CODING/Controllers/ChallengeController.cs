﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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
        [Route("Challenge/{id}/Problem")]
        public ActionResult Problem( int id)
        {
            
            //Prepare model
            ChallengeModel model = new ChallengeModel();

            //Fill data
            model.challenge = (new ChallengeDAO()).GetOne(id);
            model.OwnerName = (new UserDAO()).GetNameByID(model.challenge.OwnerID);
            model.languages = (new LanguageDAO()).GetByChallengeID(id);

            var cs = (new ChallengeDAO()).GetCodeStubs(id);

            foreach (var item in cs)
            {
                if (item.LanguageID == 1)
                {
                    model.CodeStubs_Cpp = item.CodeStub;
                }
                else if (item.LanguageID == 2)
                {
                    model.CodeStubs_CSharp = item.CodeStub;
                }
                else if (item.LanguageID == 3)
                {
                    model.CodeStubs_Java = item.CodeStub;
                }
            }
            return View(model);
        }

        [Route("Challenge/{id}/Question")]
        public ActionResult Question(int id)
        {

            ////Prepare model
            //ChallengeModel model = new ChallengeModel();

            ////Fill data
            //model.challenge = (new ChallengeDAO()).GetOne(id);
            //model.OwnerName = (new UserDAO()).GetNameByID(model.challenge.OwnerID);
            //model.languages = (new LanguageDAO()).GetByChallengeID(id);

            //var cs = (new ChallengeDAO()).GetCodeStubs(id);

            //foreach (var item in cs)
            //{
            //    if (item.LanguageID == 1)
            //    {
            //        model.CodeStubs_Cpp = item.CodeStub;
            //    }
            //    else if (item.LanguageID == 2)
            //    {
            //        model.CodeStubs_CSharp = item.CodeStub;
            //    }
            //    else if (item.LanguageID == 3)
            //    {
            //        model.CodeStubs_Java = item.CodeStub;
            //    }
            //}
            return View("Question");
        }

        public int GetLoginID()
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

        [Route("Challenge/{id}/forum")]
        public ActionResult Discussion(int id)
        {
            var models = new CommentListModel();


            models.Info = (new UserDAO().GetUserById(GetLoginID()));

            models.challenge = (new ChallengeDAO().GetOne(id));

            models.like_status = (new LikeStatusDAO().GetAllByUser(GetLoginID()));

            var commentList = (new CommentDAO().GetAllByChallenge(id,2));
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
        public ActionResult SortComment(int id,int sort)
        {
            return Redirect(String.Format("/Challenge/Discussion/{0}/{1}", id, sort));
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
                    c = commentDAO.GetNewest();
                    return Json(c.ID);
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Fail to add comment");
                }
            }
            return View("Discussion");
        }

        [HttpPost]
        public ActionResult AddReply(int userID, int commentID, string commentInput)
        {
            if (ModelState.IsValid)
            {
                var replyDAO = new ReplyDAO();
                REPLY c = new REPLY();
                c.Text = commentInput;
                c.Likes = 0;
                c.OwnerID = userID;
                c.CommentID = commentID;
                c.CreateDate = DateTime.Now;
                Boolean result = replyDAO.Insert(c);
                if (result)
                {
                    c = replyDAO.GetNewest();
                    return Json(c.ID);
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Fail to add comment");
                }
            }
            return View("Discussion");
        }

        [HttpPost]
        public ActionResult UpdateLikes(int likes,int commentId, int userId, string type)
        {
            if(ModelState.IsValid)
            {
                if(type.Equals("comment"))
                {
                    var commentDAO = new CommentDAO();
                    var statusDAO = new LikeStatusDAO();
                    COMMENT c = commentDAO.GetOne(commentId);

                    LIKE_STATUS s = new LIKE_STATUS();
                    s.OwnerID = userId;
                    s.CommentID = c.ID;
                    if (c.Likes < likes)
                    {
                        if(statusDAO.CheckLikeStatus(s) == false)
                        {
                            statusDAO.Insert(s);
                        }
                    }
                    else
                    {
                        statusDAO.Delete(s);
                    }
                    c.Likes = likes;
                    var result = commentDAO.UpdateLikes(c);
                    if(result)
                    {
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Fail to update likes");
                    }
                }
                else
                {
                    var replyDAO = new ReplyDAO();
                    var statusDAO = new LikeStatusDAO();
                    REPLY c = replyDAO.GetOne(commentId);
                    LIKE_STATUS s = new LIKE_STATUS();
                    s.OwnerID = userId;
                    s.ReplyID = c.ID;
                    if (c.Likes < likes)
                    {
                        if (statusDAO.CheckLikeStatus(s) == false)
                        {
                            statusDAO.Insert(s);
                        }
                    }
                    else
                    {
                        statusDAO.Delete(s);
                    }
                    c.Likes = likes;
                    var result = replyDAO.UpdateLikes(c);
                    if (result)
                    {
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Fail to update likes");
                    }
                }
            }
            return View();
        }

        public ActionResult GetSortComment(int challengeID, int sort)
        {
            
            List<CommentModel> models = new List<CommentModel>();

            var commentList = (new CommentDAO().GetAllByChallenge(challengeID, sort));
            if (commentList.Count > 0)
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

                    models.Add(model);
                }
            }

            return PartialView("_CommentList", models);
        }

        // GET: Challenge/edit/:id
        public ActionResult Edit(int id)
        {
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            if(ses == null)
            {
                return Redirect("Authen/Login");
            }
            ChallengeDAO DAO = new ChallengeDAO();
            bool IsEditor = DAO.IsEditor(id, ses.ID);
            if (!IsEditor)
            {
                return Redirect("User/Dashboard");
            }
            CHALLENGE c = DAO.GetOne(id);
            if(c != null)
            {
                //get code stubs
                var cs = DAO.GetCodeStubs(id);
                bool isCpp = false, isCsharp = false, isJava = false;
                foreach (var item in cs)
                {
                    if (item.LanguageID == 1)
                    {
                        isCpp = true;
                    }
                    else if (item.LanguageID == 2)
                    {
                        isCsharp = true;
                    }
                    else if (item.LanguageID == 3)
                    {
                        isJava = true;
                    }
                }
                EditChallengeModel model = new EditChallengeModel()
                {
                    ID = id,
                    Slug = c.Slug,
                    Name = c.Title,
                    Difficulty = (Difficulty)c.ChallengeDifficulty,
                    Description = c.Description,
                    ProblemStatement = c.ProblemStatement,
                    InputFormat = c.InputFormat,
                    Constraints = c.Constraints,
                    OutputFormat = c.OutputFormat,
                    Tags = c.Tags,
                    Moderators = DAO.GetModeratorByChallengeID(id),
                    TestCases = DAO.GetTestCaseByID(id),
                    LanguageCSharp = isCsharp,
                    LanguageCpp = isCpp,
                    LanguageJava = isJava,
                    DisCompileTest = (bool)c.DisCompileTest,
                    DisCustomTestcase = (bool)c.DisCustomTestcase,
                    DisSubmissions = (bool)c.DisSubmissions,
                    PublicTestcase = (bool)c.PublicTestcase,
                    PublicSolutions = (bool)c.PublicSolutions,
                    RequiredKnowledge = c.RequiredKnowledge,
                    TimeComplexity = c.TimeComplexity,
                    Editorialist = c.Editorialist,
                    PartialEditorial = (bool)c.PartialEditorial,
                    Approach = c.Approach,
                    ProblemSetter = c.ProblemSetter,
                    SetterCode = c.SetterCode,
                    ProblemTester = c.ProblemTester,
                    TesterCode = c.TesterCode
                };
                //code stub to model
                foreach(var item in cs)
                {
                    if(item.LanguageID == 1)
                    {
                        model.CodeStubs_Cpp = item.CodeStub;
                    }else if(item.LanguageID == 2)
                    {
                        model.CodeStubs_CSharp = item.CodeStub;
                    }
                    else if (item.LanguageID == 3)
                    {
                        model.CodeStubs_Java = item.CodeStub;
                    }
                }
                //get test case content
                int pos_test_case = 0;
                Dictionary<int, Dictionary<string, string>> test_case_contents = this.ReadTestCaseContent(model.TestCases);
                foreach (var one_test_case in model.TestCases)
                {
                    //get test case content
                    Dictionary<string, string> one_test_case_content = test_case_contents[pos_test_case++];
                    TestCaseResultModel temp = new TestCaseResultModel();
                    temp.Input = one_test_case_content["Input"];
                    temp.Output = one_test_case_content["Output"];
                    model.TestCaseContent.Add(temp);
                }
                return View(model);
            }
            return View("Edit");
        }

        public Dictionary<int, Dictionary<string, string>> ReadTestCaseContent(List<TESTCASE> testCase)
        {
            int pos = 0;
            Dictionary<int, Dictionary<string, string>> test_case_files = new Dictionary<int, Dictionary<string, string>>();
            foreach (TESTCASE item in testCase)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();
                temp.Add("inputFile", item.Input);
                temp.Add("outputFile", item.Output);
                test_case_files.Add(pos++, temp);
            }

            API_Helper apiHelper = new API_Helper();
            Dictionary<int, Dictionary<string, string>> result = apiHelper.ReadTestCase(test_case_files);

            return result;
        }

        [HttpPost]
        public ActionResult Edit(EditChallengeModel model)
        {
            return null;
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateDetails(EditChallengeModel model)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null || !DAO.IsEditor(model.ID, ses.ID))
            {
                return Json(new { result = false });
            }
            
            CHALLENGE c = new CHALLENGE()
            {
                ID = model.ID,
                Title = model.Name,
                Slug = CommonProject.Helper.SlugGenerator.GenerateSlug(model.Name),
                Description = model.Description,
                ProblemStatement = model.ProblemStatement,
                InputFormat = model.InputFormat,
                OutputFormat = model.OutputFormat,
                ChallengeDifficulty = (short)model.Difficulty,
                Constraints = model.Constraints,
                Tags = model.Tags
            };
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
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null || !DAO.IsEditor(challengeID, ses.ID))
            {
                return Json(new { result = false });
            }
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

        [HttpPost]
        public JsonResult AddTestCase(string input, string output, int challengeID)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null || !DAO.IsEditor(challengeID, ses.ID))
            {
                return Json(new { result = false });
            }

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
                return Json(new { result = false, msg = resInput });
            }
            t.Input = fileInput.FileName + ".txt";
            API_Helper apiHelperOutput = new API_Helper();
            var resOutput = apiHelperOutput.RequestUploadAPI(fileOutput, CommonConstant.TYPE_UPLOAD_FILE_API);
            if (!resOutput.Equals("success"))
            {
                return Json(new { result = false, msg = resOutput });
            }
            t.Output = fileOutput.FileName + ".txt";
            
            //call DAO to add test case
            bool res = DAO.AddTestCase(t);
            if (res)
            {
                return Json(new { result = true, data = t, count = testcaseID, content = new { input, output } });
            }
            return Json(new { result = false });
        }

        [HttpPost]
        public JsonResult DeleteTestCase(int id, int challengeID)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null || !DAO.IsEditor(challengeID, ses.ID))
            {
                return Json(new { result = false });
            }

            var testcase = DAO.GetOneTestCase(id);
            if (testcase != null)
            {
                FileManager fileInput = new FileManager();
                fileInput.FileName = testcase.Input;
                FileManager fileOutput = new FileManager();
                fileOutput.FileName = testcase.Output;

                //call api delete file
                API_Helper apiHelperInput = new API_Helper();
                var resInput = apiHelperInput.RequestUploadAPI(fileInput, CommonConstant.TYPE_DELETE_FILE_API);
                if (!resInput.Equals("success"))
                {
                    return Json(new { result = false, msg = resInput });
                }

                API_Helper apiHelperOutput = new API_Helper();
                var resOutput = apiHelperOutput.RequestUploadAPI(fileOutput, CommonConstant.TYPE_DELETE_FILE_API);
                if (!resOutput.Equals("success"))
                {
                    return Json(new { result = false, msg = resOutput });
                }

                bool res = DAO.DeleteTestCase(id);
                if (res)
                {
                    return Json(new { result = true, data = id });
                }
            }
            return Json(new { result = false });
        }

        [HttpPost]
        public JsonResult UpdateTestCase(int testcaseId, string input, string output, int id)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null || !DAO.IsEditor(id, ses.ID))
            {
                return Json(new { result = false });
            }

            var testcase = DAO.GetOneTestCase(testcaseId);
            if (testcase != null)
            {
                FileManager fileInput = new FileManager();
                fileInput.FileName = testcase.Input;
                fileInput.Content = input;
                FileManager fileOutput = new FileManager();
                fileOutput.FileName = testcase.Output;
                fileOutput.Content = output;

                //call api delete file
                API_Helper apiHelperInput = new API_Helper();
                var resInput = apiHelperInput.RequestUploadAPI(fileInput, CommonConstant.TYPE_UPDATE_FILE_API);
                if (!resInput.Equals("success"))
                {
                    return Json(new { result = false, msg = resInput });
                }

                API_Helper apiHelperOutput = new API_Helper();
                var resOutput = apiHelperOutput.RequestUploadAPI(fileOutput, CommonConstant.TYPE_UPDATE_FILE_API);
                if (!resOutput.Equals("success"))
                {
                    return Json(new { result = false, msg = resOutput });
                }

                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateCodeStubs(int challengeID, int language, string Code)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null || !DAO.IsEditor(challengeID, ses.ID))
            {
                return Json(new { result = false });
            }
            bool result = false;
            result = DAO.UpdateCodestub(challengeID, language, Code);
            return Json(new { result });
        }

        [HttpPost]
        public JsonResult UpdateLanguages(EditChallengeModel model)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null || !DAO.IsEditor(model.ID, ses.ID))
            {
                return Json(new { result = false });
            }
            CHALLENGE c = new CHALLENGE()
            {
                ID = model.ID,
                LanguageCSharp = model.LanguageCSharp,
                LanguageCpp = model.LanguageCpp,
                LanguageJava = model.LanguageJava
            };
            bool res = DAO.UpdateLanguage(c);
            if (res)
            {
                return Json(new { result = true, data = c });
            }
            return Json(new { result = false });
        }

        [HttpPost]
        public JsonResult UpdateSettings(EditChallengeModel model)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null || !DAO.IsEditor(model.ID, ses.ID))
            {
                return Json(new { result = false });
            }
            CHALLENGE c = new CHALLENGE()
            {
                ID = model.ID,
                DisCompileTest = model.DisCompileTest,
                DisCustomTestcase = model.DisCustomTestcase,
                DisSubmissions = model.DisSubmissions,
                PublicTestcase = model.PublicTestcase,
                PublicSolutions = model.PublicSolutions
            };
            bool res = DAO.UpdateSetting(c);
            if (res)
            {
                return Json(new { result = true, data = c });
            }
            return Json(new { result = false });
        }

        [HttpPost]
        public JsonResult UpdateEditorial(EditChallengeModel model)
        {
            ChallengeDAO DAO = new ChallengeDAO();
            //login session
            var ses = Session[CommonConstant.SESSION_INFO_LOGIN] as InfoLogIn;
            //check is login and is editor
            if (ses == null || !DAO.IsEditor(model.ID, ses.ID))
            {
                return Json(new { result = false });
            }
            CHALLENGE c = new CHALLENGE()
            {
                ID = model.ID,
                RequiredKnowledge = model.RequiredKnowledge,
                TimeComplexity = model.TimeComplexity,
                Editorialist = model.Editorialist,
                PartialEditorial = model.PartialEditorial,
                Approach = model.Approach,
                ProblemSetter = model.ProblemSetter,
                SetterCode = model.SetterCode,
                ProblemTester = model.ProblemTester,
                TesterCode = model.SetterCode
            };
            bool res = DAO.UpdateEditorial(c);
            if (res)
            {
                return Json(new { result = true, data = c });
            }
            return Json(new { result = false });
        }
    }
}