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
    public class CodeController : Controller
    {
        // GET: Code
        public ActionResult Index()
        {
            return View();
        }

        // GET: Code
        [HttpPost]
        public ActionResult SubmitCode(int challengeID, string Code, string Language)
        {
            int userID = this.GetUserID();

            List<TestCaseResultModel> result = new List<TestCaseResultModel>();
            int count_success = 0;
            //get test case
            List<TESTCASE> test_cases = (new TestCaseDAO()).GetAllByChallengeID(challengeID);
            foreach (var one_test_case in test_cases)
            {
                string app_path = AppDomain.CurrentDomain.BaseDirectory;
                string input_file = one_test_case.Input != "" ? app_path + CommonConstant.DIR_TESTCASE + one_test_case.Input : "";
                string output_file = one_test_case.Output != "" ? app_path + CommonConstant.DIR_TESTCASE + one_test_case.Output : "";

                //get test case
                Dictionary<string, string> test_case_content = this.ReadTestCaseContent(input_file, output_file);
                string input_expect = test_case_content["input"];
                string output_expect = test_case_content["output"];

                //change code to suitable with test case (read write input output)
                string file_name = "MyClass" + userID.ToString();
                string code_run = this.ChangeCode(Code, file_name, input_file, Language);
                //validate code

                TestCaseResultModel one_test_case_result = RunOneTestCase(input_expect, output_expect, file_name, code_run, Language, userID);
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
                    TimeDone = BitConverter.GetBytes((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds)       //timestamp now
                };

                bool insert_status = (new AnswerDAO()).Insert(answer);
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult RunCode(int challengeID, string Code, string Language)
        {
            int userID = this.GetUserID();

            List<TestCaseResultModel> result = new List<TestCaseResultModel>();
            string app_path = AppDomain.CurrentDomain.BaseDirectory;
            //get test case
            TESTCASE one_test_case = (new TestCaseDAO()).GetOneByChallengeID(challengeID);
            string input_file = one_test_case.Input != "" ? app_path + CommonConstant.DIR_TESTCASE + one_test_case.Input : "";
            string output_file = one_test_case.Output != "" ? app_path + CommonConstant.DIR_TESTCASE + one_test_case.Output : "";

            //read test case
            Dictionary<string, string> test_case_content = this.ReadTestCaseContent(input_file, output_file);
            string input_expect = test_case_content["input"];
            string output_expect = test_case_content["output"];

            //change code to suitable with test case (read write input output)
            string file_name = "MyClass" + userID.ToString();
            string code_run = this.ChangeCode(Code, file_name, input_file, Language);
            //@TODO validate code 

            TestCaseResultModel one_test_case_result =  RunOneTestCase(input_expect, output_expect, file_name, code_run, Language, userID);
            result.Add(one_test_case_result);

            return Json(result);
        }

        //@TODO get user id (from session)
        protected int GetUserID()
        {
            return 1;
        }

        //@TODO do this later 
        //API for user live code
        [HttpPost]
        public ActionResult LiveCode(string Code, int language)
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

        protected TestCaseResultModel RunOneTestCase( string input_expect, string output_expect, string file_name, string code_run, string language, int userID)
        {
            //call api to run
            Dictionary<string, string> result_api = this.CallAPI(code_run, language, userID);

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

        //Change code to specific with every user
        protected string ChangeCode(string Code, string className, string inputFileName, string Language)
        {
            Language = Language.ToUpper();
            if (Language.Equals(CommonConstant.TYPE_JAVA_COMPILER))
            {
                //Replace class name
                Code = Code.Replace("class MyClass", "class " + className);

                //Replace Input and Output file name
                Code = Code.Replace("INPUT_FILE_NAME", inputFileName);
            }
            //@TODO cac ngon ngu khac thi tiep tuc ifelse

            return Code;
        }

        protected Dictionary<string, string> CallAPI(string code, string language, int userID)
        {
            API_Helper apiHelper = new API_Helper();
            Source src = new Source();
            src.stringSource = code;
            src.versionFramework = "2.3";
            src.userKey = userID.ToString();
            var result = apiHelper.RequestAPI(CommonConstant.TYPE_JAVA_COMPILER, src);

            //var dictionary = new Dictionary<string, string>();
            //dictionary.Add("status", "success");
            //dictionary.Add("message", "Code you run is successfully");
            //return dictionary;
            return result;
        }

        protected Dictionary<string, string> ReadTestCaseContent(string input_file, string output_file)
        {
            Dictionary<string, string> test_case_content = new Dictionary<string, string>();

            if (System.IO.File.Exists(input_file))
            {
                using (StreamReader reader = new StreamReader(input_file))
                {
                    test_case_content.Add("input", reader.ReadToEnd());
                }
            }
            else
            {
                test_case_content.Add("input", "NO INPUT");
            }
            if (System.IO.File.Exists(output_file))
            {
                using (StreamReader reader = new StreamReader(output_file))
                {
                    test_case_content.Add("output", reader.ReadToEnd());
                }
            }
            else
            {
                test_case_content.Add("output", "NO OUTPUT");
            }
            return test_case_content;
        }
    }
}