using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COURSE_CODING.Models;
using CommonProject;
using DAO.DAO;
using DAO.EF;
using CommonProject.Models;

namespace COURSE_CODING.Controllers
{
    public class CodeController : BaseController
    {
        // GET: Code
        public ActionResult Index()
        {
            return View();
        }

        /* Submit code to anser the challenge
        ** Input    : challengeID, Code, Language: string
        ** Output   : List{   Status : "success" | "fail",
                              Input  : string,
                              Output : string,
                              OutputExpect : string,
                      }
        */
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitCode(int challengeID, string Code, string Language)
        {
            int userID = this.GetUserID();
            string code = Code;
            int count_success = 0, pos_test_case = 0;
            //get test case content
            List<TESTCASE> test_cases = (new TestCaseDAO()).GetAllByChallengeID(challengeID);
            //Dictionary<int,Dictionary<string, string>> test_case_contents = this.ReadTestCaseContent(test_cases);
            //foreach (var one_test_case in test_cases)
            //{
            //    //get test case content
            //    Dictionary<string, string> one_test_case_content = test_case_contents[pos_test_case++];
            //    string input_expect = one_test_case_content["Input"];
            //    string output_expect = one_test_case_content["Output"];

            //    TestCaseResultModel one_test_case_result = RunOneTestCase(code, Language, input_expect, output_expect, userID, one_test_case.Input);
            //    result.Add(one_test_case_result);
            //    if (one_test_case_result.Status == "success")
            //    {
            //        count_success++;
            //    }
            //}
            List<TestCaseFile> list_test_case = new List<TestCaseFile>();
            foreach (var one_test_case in test_cases)
            {
                list_test_case.Add(new TestCaseFile
                {
                    inputFile = one_test_case.Input,
                    outputFile = one_test_case.Output,
                });
            }

            //Call API (code, language, userID, TESTCASE);
            List<TestCaseResultModel> result = this.CallAPIRunCode(code, Language, userID, list_test_case);

            //save submit to DB
            if (count_success == test_cases.Count())
            {
                ANSWER answer = new ANSWER()
                {
                    ChallengeID = challengeID,
                    UserId = userID,
                    Content = Code,
                    Result = true,
                    TimeDone = DateTime.Now       //timestamp now
                };

                bool insert_status = (new AnswerDAO()).InsertOrUpdate(answer);
            }

            return Json(result);
        }

        /* Run code to test the challenge
        ** Input    : challengeID, Code, Language: string
        ** Output   : Status : "success" | "fail",
                      Input  : string,
                      Output : string,
                      OutputExpect : string,
        */
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult RunCode(int challengeID, string Code, string Language)
        {
            int userID = this.GetUserID();
            string code = Code;

            //get test case
            TESTCASE one_test_case = (new TestCaseDAO()).GetOneByChallengeID(challengeID);
            List<TestCaseFile> list_test_case = new List<TestCaseFile>();
            if (one_test_case != null)
            {
                list_test_case.Add(new TestCaseFile {
                    inputFile = one_test_case.Input,
                    outputFile = one_test_case.Output,
                });
            }

            //Call API (code, language, userID, TESTCASE);
            List<TestCaseResultModel> result = this.CallAPIRunCode(code, Language, userID, list_test_case);

            return Json(result);
        }

        //@TODO get user id (from session)
        protected int GetUserID()
        {
            var session = (COURSE_CODING.Common.InfoLogIn)Session[CommonProject.CommonConstant.SESSION_INFO_LOGIN];
            if (session != null)
            {
                return session.ID;
            }
            return (new Random()).Next(1,100);
        }

        //@TODO do this later 
        //API for user live code
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LiveCode(string Code, string Language)
        {
            int userID = this.GetUserID();

            Dictionary<string, string> result_api = CallAPI(Code, Language, userID, null);
            return Json(result_api);
        }

        //protected TestCaseResultModel RunOneTestCase(string code_run, string language, string input_expect, string output_expect, int user_id, string input_file_name_change_in_code)
        //{
        //    //call api to run
        //    Dictionary<string, string> result_api = this.CallAPI(code_run, language, user_id, null);

        //    if(result_api.Count <= 0)
        //    {
        //        return (new TestCaseResultModel()
        //        {
        //            Status = "fail",
        //            Input = input_expect,
        //            Output = "Server call api compiler fail",
        //            OutputExpect = output_expect,
        //        });
        //    }
        //    char[] charsToTrim = {'\r','\n'};
        //    result_api["message"] = result_api["message"].TrimEnd(charsToTrim);

        //    if (result_api["status"] == "success")
        //    {
        //        //Check result with output testcase
        //        if (result_api["message"] == output_expect)
        //        {
        //            return (new TestCaseResultModel()
        //            {
        //                Status = "success",
        //                Input = input_expect,
        //                Output = result_api["message"],
        //                OutputExpect = output_expect,
        //            });
        //        }
        //        else
        //        {
        //            return (new TestCaseResultModel()
        //            {
        //                Status = "fail",
        //                Input = input_expect,
        //                Output = result_api["message"],
        //                OutputExpect = output_expect,
        //            });
        //        }
        //    }
        //    else
        //    {
        //        return (new TestCaseResultModel()
        //        {
        //            Status = "fail",
        //            Input = input_expect,
        //            Output = result_api["message"],
        //            OutputExpect = output_expect,
        //        });
        //    }
        //}
        //TODO
        //[HttpPost]
        //public ActionResult BaseRun(string Code, int language)
        //{
        //    switch (language)
        //    {
        //        case 1:
        //            //call api C++
        //            break;
        //        case 2:
        //            //call api C#
        //            break;
        //        case 3:
        //            //call api Java
        //            break;
        //    }

        //    return View();
        //}

        protected Dictionary<string, string> CallAPI(string code, string language, int userID, List<TestCaseFile> testCase = null)
        {
            API_Helper apiHelper = new API_Helper();
            Source src = new Source();
            src.stringSource = code;
            src.versionFramework = "2.3";
            src.userKey = userID.ToString();
            language = language.ToUpper();
            if (testCase != null)
            {
                src.TestCase = testCase;
            }

            var result = apiHelper.RequestAPI(language, src);
            return result;
        }


        protected List<TestCaseResultModel> CallAPIRunCode(string code, string language, int userID, List<TestCaseFile> testCase = null)
        {
            API_Helper apiHelper = new API_Helper();
            Source src = new Source();
            src.stringSource = code;
            src.versionFramework = "2.3";
            src.userKey = userID.ToString();
            language = language.ToUpper();
            if (testCase != null)
            {
                src.TestCase = testCase;
            }

            List<TestCaseResultModel> result = apiHelper.RequestAPIRunCode(language, src);
            return result;
        }

        //protected Dictionary<int, Dictionary<string, string>> ReadTestCaseContent( List<TESTCASE> testCase)
        //{
        //    int pos = 0;
        //    Dictionary<int, Dictionary<string, string>> test_case_files = new Dictionary<int, Dictionary<string, string>>();
        //    foreach (TESTCASE item in testCase)
        //    {
        //        Dictionary<string, string> temp = new Dictionary<string, string>();
        //        temp.Add("inputFile", item.Input);
        //        temp.Add("outputFile", item.Output);
        //        test_case_files.Add(pos++, temp);
        //    }

        //    API_Helper apiHelper = new API_Helper();
        //    Dictionary<int, Dictionary<string, string>> result = apiHelper.ReadTestCase(test_case_files);

        //    return result;
        //}
    }
}