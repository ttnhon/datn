using CommonProject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Common;
using WebAPI.Models;

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
        
        public Dictionary<string, string> ReadTestCaseContent(Dictionary<string,string> oneTestCase)
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

        public Dictionary<string, string> ReadTestCaseContent(TestCaseFile testcase)
        {
            string app_path = AppDomain.CurrentDomain.BaseDirectory;
            string input_file = app_path + Constant.TESTCASE_DIR + testcase.inputFile;
            string output_file = app_path + Constant.TESTCASE_DIR + testcase.outputFile;

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

        /// <summary>
        /// upload string to create file txt test case in server
        /// </summary>
        /// <param name="file"> content of file</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Upload(FileManager file)
        {
            try
            {
                //return result
                string res = "success";
                string content = file.Content;
                string app_path = AppDomain.CurrentDomain.BaseDirectory;
                string directory_file = app_path + Constant.TESTCASE_DIR;
                //challenge_{id_challenge}_input/output_{id_testcase}.txt
                string filename_code = file.FileName;
                string full_path = directory_file + filename_code;
                /*write code to file.txt*/
                if (!System.IO.File.Exists(full_path + ".txt"))
                {
                    using (StreamWriter w = new StreamWriter(full_path + ".txt", false))
                    {
                        w.Write(content); // Write the text
                    }

                    return Ok(res);
                }
                else
                {
                    return BadRequest("Error: file already exists.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// update content file txt test case in server by name
        /// </summary>
        /// <param name="file"> content of file</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateFile(FileManager file)
        {
            try
            {
                //return result
                string res = "success";
                string content = file.Content;
                string app_path = AppDomain.CurrentDomain.BaseDirectory;
                string directory_file = app_path + Constant.TESTCASE_DIR;
                //challenge_{id_challenge}_input/output_{id_testcase}
                string filename_code = file.FileName;
                string full_path = directory_file + filename_code;
                /*write code to file.txt*/
                if (System.IO.File.Exists(full_path))
                {
                    System.IO.File.Delete(full_path);
                    using (StreamWriter w = new StreamWriter(full_path, false))
                    {
                        w.Write(content); // Write the text
                    }
                }
                else
                {
                    return BadRequest("Error: file is not exists.");
                }
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// delete file txt test case in server by name
        /// </summary>
        /// <param name="file"> content of file</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteFile(FileManager file)
        {
            try
            {
                //return result
                string res = "success";
                //string content = file.Content;
                string app_path = AppDomain.CurrentDomain.BaseDirectory;
                string directory_file = app_path + Constant.TESTCASE_DIR;
                //challenge_{id_challenge}_input/output_{id_testcase}
                string filename_code = file.FileName;
                string full_path = directory_file + filename_code;
                /*write code to file.txt*/
                if (System.IO.File.Exists(full_path))
                {
                    System.IO.File.Delete(full_path);
                }
                else
                {
                    return BadRequest("Error: file is not exists.");
                }
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}