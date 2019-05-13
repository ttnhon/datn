using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Common;

namespace WebAPI.Controllers
{
    /// <summary>
    /// class compile for c#
    /// </summary>
    public class TestCaseFileManagerController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetTestCase(Dictionary<int, Dictionary<string, string>> TestCase)
        {
            int key = 0;
            Dictionary<int, Dictionary<string, string>> result = new Dictionary<int, Dictionary<string, string>>();
            foreach (var oneItem in TestCase)
            {
                Dictionary<string, string> one_test_case = oneItem.Value;
                result.Add(key++, this.ReadTestCaseContent(one_test_case));
            }

            return Ok(result);
        }



        protected Dictionary<string, string> ReadTestCaseContent(Dictionary<string,string> oneTestCase)
        {
            string app_path = AppDomain.CurrentDomain.BaseDirectory;
            string input_file = app_path + Constant.TESTCASE_DIR + oneTestCase["inputFile"];
            string output_file = app_path + Constant.TESTCASE_DIR + oneTestCase["outputFile"];

            Dictionary<string, string> test_case_content = new Dictionary<string, string>();

            if (System.IO.File.Exists(input_file))
            {
                using (StreamReader reader = new StreamReader(input_file))
                {
                    test_case_content.Add("Input", reader.ReadToEnd());
                }
            }
            else
            {
                test_case_content.Add("Input", "NO INPUT");
            }
            if (System.IO.File.Exists(output_file))
            {
                using (StreamReader reader = new StreamReader(output_file))
                {
                    test_case_content.Add("Output", reader.ReadToEnd());
                }
            }
            else
            {
                test_case_content.Add("Output", "NO OUTPUT");
            }
            return test_case_content;
        }
    }
}