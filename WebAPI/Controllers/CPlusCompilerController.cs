using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using CommonProject;
using Microsoft.CSharp;
using WebAPI.Common;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// class compile for c++
    /// </summary>
    public class CPlusCompilerController : ApiController
    {
        //private string text;
        protected Dictionary<string, string> ExecuteCMD(string directory_path = "D:\\", string file_code = "MyClass")
        {
            /*Run cmd command*/
            //set up
            string ProjDir = System.AppDomain.CurrentDomain.BaseDirectory;
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";  //run cmd
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = directory_path;        //Link to directory of file need to execute
            //p.StartInfo.Arguments = "/C " + directory_path_code + file_code + ".exe";          //=> "MyClass"
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;

            //run
            p.Start();
            p.StandardInput.WriteLine("set PATH=%PATH%;" + ProjDir + @"Compilers/MinGW/bin");
            p.StandardInput.WriteLine(file_code + ".exe");
            p.StandardInput.Flush();
            p.StandardInput.Close();
            string result_string = p.StandardOutput.ReadToEnd();
            string error_string = p.StandardError.ReadToEnd();
            bool WaitResult = p.WaitForExit(30000);

            Dictionary<string, string> result = new Dictionary<string, string>();
            if (!WaitResult)
            {
                Helpers.ProcessManager.KillProcessAndChildren(p.Id);
                result.Add("status", "fail");
                result.Add("message", "Error: Process timeout");
                return result;
            }
            string[] str = result_string.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            StringBuilder S_builder = new StringBuilder();

            for(int i = 6; i < str.Length - 1; i++)
            {
                S_builder.Append(str[i]);
            }
            //return result
            string status = "success";
            string result_message = S_builder.ToString();
            if (error_string != "")
            {
                status = "fail";
                result_message = error_string;
            }
            result.Add("status", status);
            result.Add("message", result_message);
            return result;

        }
        
        //create a.exe (defaut) file
        protected Dictionary<string, string> ExecuteGPP(string directory_path_code = "D:\\", string file_code = "MyClass.exe MyClass.cpp")
        {
            string ProjDir = System.AppDomain.CurrentDomain.BaseDirectory;
            ProjDir = ProjDir.Replace("\\", "/");
            file_code = file_code.Replace("\\", "/");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
           // p.StartInfo.FileName = ProjDir + @"Compilers/MinGW/bin/g++.exe";  //Link to g++.exe  => "g++"
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            //p.StartInfo.WorkingDirectory = ProjDir + @"Compilers/MinGW/bin";                //Link to directory of file need to execute
            p.StartInfo.WorkingDirectory = directory_path_code;
            //p.StartInfo.Arguments = "/C g++ -o " + file_code;             // =>    "g++ -o MyClass.exe MyClass.cpp"
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;

            //run
            p.Start();
            p.StandardInput.WriteLine("set PATH=%PATH%;"+ ProjDir + @"Compilers/MinGW/bin");
            p.StandardInput.WriteLine("g++ -o " + file_code);
            p.StandardInput.Flush();
            p.StandardInput.Close();
            string result_string = p.StandardOutput.ReadToEnd();
            string error_string = p.StandardError.ReadToEnd();
            bool WaitResult = p.WaitForExit(30000);

            Dictionary<string, string> result = new Dictionary<string, string>();
            if (!WaitResult)
            {
                Helpers.ProcessManager.KillProcessAndChildren(p.Id);
                result.Add("status", "fail");
                result.Add("message", "Error: Process timeout");
                return result;
            }
            //return result
            string status = "success";
            string result_message = result_string;
            if (error_string != "")
            {
                status = "fail";
                result_message = error_string;
            }
            
            result.Add("status", status);
            result.Add("message", result_message);
            return result;
        }
        
        /// <summary>
        /// get output when compile string source
        /// </summary>
        /// <param name="source"> string of source</param>
        /// <returns></returns>
        [HttpPost]
        [System.Web.Mvc.ValidateInput(false)]
        public IHttpActionResult Compiler(Source source)
        {
            try
            {
                string code = source.stringSource;
                string app_path = AppDomain.CurrentDomain.BaseDirectory;        //get app_path
                string directory_path = app_path + Constant.CODE_DIR;    //get directory execute
                string filename_code = "MyClass" + source.userKey;              //file to execute
                string class_name = filename_code;                              //class name in file execute   ( = filename)
                string input_file = "";
                if (source.Data.Count > 0)
                {
                    input_file = source.Data["inputFile"] ?? "";                   //input file name to read when execute
                }

                //Change code
                code = this.ChangeCode(code, class_name, input_file);
                string full_path = directory_path + filename_code;
                //make sure this file not exist
                DeleteFile(full_path);

                /*write code to file.cpp*/
                using (StreamWriter w = new StreamWriter(full_path + ".cpp", true))
                {
                    w.WriteLine(code); // Write the text
                }

                /*run g++ dir\\MyClass{userKey}.cpp*/
                Dictionary<string, string> result_gpp = this.ExecuteGPP(directory_path, filename_code + ".exe " + filename_code + ".cpp");
                if (result_gpp["status"] == "fail")            //return if run g++ fail
                {
                    //delete MyClass.cpp
                    if (System.IO.File.Exists(full_path + ".cpp"))
                    {
                        System.IO.File.Delete(full_path + ".cpp");
                    }
                    return Ok(result_gpp);
                }

                /*run D:\\MyClass*/
                Dictionary<string, string> result_cpp = this.ExecuteCMD(directory_path, filename_code);

                DeleteFile(full_path);

                //return g++ execute
                //if (result_gpp["status"] == "fail")
                //    return BadRequest(result_cpp["message"]);
                return Ok(result_cpp);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        //Change file input name to full path
        protected string ChangeCode(string Code, string className, string inputFileName)
        {
            string app_path = AppDomain.CurrentDomain.BaseDirectory;
            string input_file_path = app_path + Constant.TESTCASE_DIR + inputFileName;
            input_file_path = input_file_path.Replace("\\", "/");
            //Replace class name
            //Code = Code.Replace("class MyClass", "class " + className);
            //Code = Regex.Replace(Code, @"(?<=class )(.*?)(?={)", className);       // class Abcdfegh   => class Classname

            //Replace Input and Output file name
            Code = Code.Replace("INPUT_FILE_NAME", input_file_path);

            return Code;
        }

        /// <summary>
        /// Delete File
        /// </summary>
        /// <param name="full_path">Full path of the file</param>
        /// <returns></returns>
        protected void DeleteFile(string full_path)
        {
            //delete file MyClass{userKey}.exe and MyClass{userKey}.cpp
            if (System.IO.File.Exists(full_path + ".exe"))
            {
                System.IO.File.Delete(full_path + ".exe");
            }
            if (System.IO.File.Exists(full_path + ".cpp"))
            {
                System.IO.File.Delete(full_path + ".cpp");
            }
        }

        /// <summary>
        /// get result when submit code challenge
        /// </summary>
        /// <param name="source">data contain string code and testcase</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RunCodeChallenge(Source source)
        {
            try
            {
                string code = source.stringSource;
                string app_path = AppDomain.CurrentDomain.BaseDirectory;        //get app_path
                string directory_path = app_path + Constant.CODE_DIR;    //get directory execute
                string filename_code = "MyClass" + source.userKey;              //file to execute
                string class_name = filename_code;                              //class name in file execute   ( = filename)

                TestCaseFileManagerController testcase_controller = new TestCaseFileManagerController();
                List<TestCaseResultModel> list_result_run_code = new List<TestCaseResultModel>();
                List<TestCaseFile> testcases = source.TestCase;

                foreach (TestCaseFile testCase in testcases)
                {
                    string code_run = this.ChangeCode(code, class_name, testCase.inputFile);
                    Dictionary<string, string> run_code_output = this.Run(code_run, filename_code, directory_path);     //run code and catch output
                    Dictionary<string, string> testcase_content = testcase_controller.ReadTestCaseContent(testCase);    //read testcase content
                    list_result_run_code.Add(this.MatchTestCase(run_code_output, testcase_content));
                }
                return Ok(list_result_run_code);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        protected Dictionary<string, string> Run(string code, string filename_code, string directory_path)
        {
            string class_name = filename_code;                              //class name in file execute   ( = filename)
            string path_file_execute = directory_path + "\\" + filename_code;

            this.DeleteFile(path_file_execute);             // make sure file not exit to create file

            /*write code to file.java*/
            using (StreamWriter w = new StreamWriter(path_file_execute + ".cpp", true))
            {
                w.WriteLine(code); // Write the text
            }

            /*run javac E:\\MyClass.java*/
            Dictionary<string, string> result_gpp = this.ExecuteGPP(directory_path, filename_code + ".exe " + filename_code + ".cpp");
            if (result_gpp["status"] == Constant.STATUS_FAIL)            //return if run javac fail
                return result_gpp;

            /*run java E:\\MyClass*/
            Dictionary<string, string> result = this.ExecuteCMD(directory_path, filename_code);

            this.DeleteFile(path_file_execute);
            return result;
        }

        protected TestCaseResultModel MatchTestCase(Dictionary<string, string> run_code_output, Dictionary<string, string> testcase)
        {
            if (run_code_output.Count <= 0)
            {
                return (new TestCaseResultModel()
                {
                    Status = "fail",
                    Input = testcase["Input"],
                    Output = "Server call api compiler fail",
                    OutputExpect = testcase["Output"],
                });
            }
            char[] charsToTrim = { '\r', '\n' };
            run_code_output["message"] = run_code_output["message"].TrimEnd(charsToTrim);

            if (run_code_output["status"] == "success")
            {
                //Check result with output testcase
                if (run_code_output["message"] == testcase["Output"])
                {
                    return (new TestCaseResultModel()
                    {
                        Status = "success",
                        Input = testcase["Input"],
                        Output = run_code_output["message"],
                        OutputExpect = testcase["Output"],
                    });
                }
                else
                {
                    return (new TestCaseResultModel()
                    {
                        Status = "fail",
                        Input = testcase["Input"],
                        Output = run_code_output["message"],
                        OutputExpect = testcase["Output"],
                    });
                }
            }
            else
            {
                return (new TestCaseResultModel()
                {
                    Status = "fail",
                    Input = testcase["Input"],
                    Output = run_code_output["message"],
                    OutputExpect = testcase["Output"],
                });
            }
        }
    }
}
