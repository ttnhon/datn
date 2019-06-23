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
            List<TestCaseResultModel> result = new List<TestCaseResultModel>();
            int count_success = 0, pos_test_case = 0;
            //get test case content
            List<TESTCASE> test_cases = (new TestCaseDAO()).GetAllByChallengeID(challengeID);
            Dictionary<int,Dictionary<string, string>> test_case_contents = this.ReadTestCaseContent(test_cases);
            foreach (var one_test_case in test_cases)
            {
                //get test case content
                Dictionary<string, string> one_test_case_content = test_case_contents[pos_test_case++];
                string input_expect = one_test_case_content["Input"];
                string output_expect = one_test_case_content["Output"];

                TestCaseResultModel one_test_case_result = RunOneTestCase(code, Language, input_expect, output_expect, userID, one_test_case.Input);
                result.Add(one_test_case_result);
                if (one_test_case_result.Status == "success")
                {
                    count_success++;
                }
            }

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

                bool insert_status = (new AnswerDAO()).Insert(answer);
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
            List<TestCaseResultModel> result = new List<TestCaseResultModel>();

            //get test case
            TESTCASE one_test_case = (new TestCaseDAO()).GetOneByChallengeID(challengeID);
            List<TESTCASE> list_test_case = new List<TESTCASE>();
            if (one_test_case != null)
            {
                list_test_case.Add(one_test_case);
            }
            //read test case content
            var readTestCase = this.ReadTestCaseContent(list_test_case);
            Dictionary<string, string> test_case_content = readTestCase.Count > 0 ? readTestCase[0] : null;
            
            string input_expect = test_case_content != null ? test_case_content["Input"] : "NO INPUT";
            string output_expect = test_case_content != null ? test_case_content["Output"] : "NO INPUT";

            TestCaseResultModel one_test_case_result =  RunOneTestCase(code, Language, input_expect, output_expect, userID, one_test_case != null ? one_test_case.Input : null);
            result.Add(one_test_case_result);

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

            Dictionary<string, string> result_api = CallAPI(Code, Language, userID,"");
            return Json(result_api);
        }

        protected TestCaseResultModel RunOneTestCase(string code_run, string language, string input_expect, string output_expect, int user_id, string input_file_name_change_in_code)
        {
            //call api to run
            Dictionary<string, string> result_api = this.CallAPI(code_run, language, user_id, input_file_name_change_in_code);

            if(result_api.Count <= 0)
            {
                return (new TestCaseResultModel()
                {
                    Status = "fail",
                    Input = input_expect,
                    Output = "Server call api compiler fail",
                    OutputExpect = output_expect,
                });
            }
            char[] charsToTrim = {'\r','\n'};
            result_api["message"] = result_api["message"].TrimEnd(charsToTrim);

            if (result_api["status"] == "success")
            {
                //Check result with output testcase
                if (result_api["message"] == output_expect)
                {
                    return (new TestCaseResultModel()
                    {
                        Status = "success",
                        Input = input_expect,
                        Output = result_api["message"],
                        OutputExpect = output_expect,
                    });
                }
                else
                {
                    return (new TestCaseResultModel()
                    {
                        Status = "fail",
                        Input = input_expect,
                        Output = result_api["message"],
                        OutputExpect = output_expect,
                    });
                }
            }
            else
            {
                return (new TestCaseResultModel()
                {
                    Status = "fail",
                    Input = input_expect,
                    Output = result_api["message"],
                    OutputExpect = output_expect,
                });
            }
        }
        //TODO
        [HttpPost]
        public ActionResult BaseRun(string Code, int language)
        {
            switch (language)
            {
                case 1:
                    //call api C++
                    break;
                case 2:
                    //call api C#
                    break;
                case 3:
                    //call api Java
                    break;
            }

            return View();
        }

        protected string[] GetTestCase()
        {
            string[] result = { "1 2 3 4 5 6", "1 2 3 4 5 6" };
            return result;
        }

        ////Change code to specific with every user
        //protected string ChangeCode(string Code, string className, string inputFileName, string Language)
        //{
        //    Language = Language.ToUpper();
        //    if (Language.Equals(CommonConstant.TYPE_JAVA_COMPILER))
        //    {
        //        //Replace class name
        //        Code = Code.Replace("class MyClass", "class " + className);

        //        //Replace Input and Output file name
        //        Code = Code.Replace("INPUT_FILE_NAME", inputFileName);
        //    }
        //    //@TODO cac ngon ngu khac thi tiep tuc ifelse

        //    return Code;
        //}

        protected Dictionary<string, string> CallAPI(string code, string language, int userID, string input_file_name_change_in_code)
        {
            API_Helper apiHelper = new API_Helper();
            Source src = new Source();
            src.stringSource = code;
            src.versionFramework = "2.3";
            src.userKey = userID.ToString();
            src.Data.Add("inputFile", input_file_name_change_in_code);
            language = language.ToUpper();
            
            var result = apiHelper.RequestAPI(language, src);
            return result;
        }




        public Dictionary<int, Dictionary<string, string>> ReadTestCaseContent( List<TESTCASE> testCase)
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
    }
}