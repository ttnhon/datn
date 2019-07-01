using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using CommonProject;
using Microsoft.CSharp;
using WebAPI.Common;
using WebAPI.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Controllers
{
    /// <summary>
    /// class compile for c#
    /// </summary>
    public class CSharpCompilerController : ApiController
    {
        protected Dictionary<string, string> ExecuteCMD(string directory_file_code , string file_code)
        {
            /*Run cmd command*/
            //set up
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";  //run cmd
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = directory_file_code;        //Link to directory of file need to execute
            p.StartInfo.Arguments = "/C "+ file_code;        
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;

            //run
            p.Start();
            bool WaitExecute = p.WaitForExit(10000);
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (!WaitExecute)
            {
                Helpers.ProcessManager.KillProcessAndChildren(p.Id);
                result.Add("status", "fail");
                result.Add("message", "Error: Process timeout");
                return result;
            }
            string result_string = string.Empty;
            string error_string = p.StandardError.ReadToEnd();
            result_string = p.StandardOutput.ReadToEnd();
            p.WaitForExit(30000);

            
            //return result
            string status = Constant.STATUS_SUCCESS;
            string result_message = result_string;
            if (error_string != "")
            {
                status = Constant.STATUS_FAIL;
                result_message = error_string;
            }
            result.Add("status", status);
            result.Add("message", result_message);
            return result;
          
        }

        //create a.exe (defaut) file
        protected Dictionary<string, string> ExecuteCSC(string directory_file_code , string file_code)
        {
            string ProjDir = System.AppDomain.CurrentDomain.BaseDirectory;
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = ProjDir + @"\Compilers\v4.0.30319\csc.exe";  
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = directory_file_code;            
            p.StartInfo.Arguments =  file_code;            
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;

            //run
            p.Start();
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
        protected void DeleteFile(string full_path)
        {
            if (System.IO.File.Exists(full_path))           
            {
                System.IO.File.Delete(full_path);
            }
            if (System.IO.File.Exists(full_path + ".cs"))
            {
                System.IO.File.Delete(full_path + ".cs");
            }
            if (System.IO.File.Exists(full_path + ".exe"))
            {
                System.IO.File.Delete(full_path + ".exe");
            }
            if (System.IO.File.Exists(full_path + ".pdb"))
            {
                System.IO.File.Delete(full_path + ".pdb");
            }
        }
        /// <summary>
        /// get output when compile string source
        /// </summary>
        /// <param name="stringSource"> string of source</param>
        /// <returns></returns>
        [HttpPost]
        [System.Web.Mvc.ValidateInput(false)]
        public IHttpActionResult Compiler(Source source)
        {
            try
            {
                string code = source.stringSource;
                StringBuilder resultCompiler = new StringBuilder();
                String fileName = "Outputcshap" + Guid.NewGuid().ToString();
                string input_file = "";
                if (source.Data.Count > 0)
                {
                    input_file = source.Data["inputFile"] ?? "";                   //input file name to read when execute
                }
                //Change code
                code = this.ChangeCode(code, "", input_file);
                String pathFolder = System.AppDomain.CurrentDomain.BaseDirectory + Common.Constant.CODE_DIR;
                String outputCompiler = pathFolder + fileName;


                Dictionary<string, string> resultAPI = new Dictionary<string, string>();
                using (StreamWriter w = new StreamWriter(pathFolder + fileName + ".cs", true))
                {
                    w.WriteLine(code); 
                }
                
                CSharpCodeProvider ccp = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters(new[] { Constant.MSCOR_LIB,
                Constant.CORE_LIB }, outputCompiler, true);
                CompilerResults result = ccp.CompileAssemblyFromSource(parameters, source.stringSource);
                //return result
                string status = Constant.STATUS_SUCCESS;
                //string result_message = string.Empty;
                   
                if (result.Errors.HasErrors)
                {
                    var listErrors = result.Errors.Cast<CompilerError>().ToList();
                    foreach (var error in listErrors)
                    {
                        resultCompiler.Append(error.ErrorText + "\r\n");
                    }
                    status = Constant.STATUS_FAIL;
                    resultAPI.Add("status", status);
                    resultAPI.Add("message", resultCompiler.ToString());
                }
                else
                {
                    Dictionary<string, string> result_compiler = this.ExecuteCSC(pathFolder, fileName+".cs");
                    if (result_compiler["status"] == Constant.STATUS_FAIL)            //return if run javac fail
                        return Ok(result_compiler);

                    /*run java E:\\MyClass*/
                    resultAPI = this.ExecuteCMD(pathFolder, fileName);
                }
                this.DeleteFile(outputCompiler);
                return Ok(resultAPI);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        // POST api/values
        [HttpPost]
        public IHttpActionResult RunCodeChallenge(Source source)
        {
            try
            {
                string code = source.stringSource;
                StringBuilder resultCompiler = new StringBuilder();
                String fileName = "Outputcshap" + Guid.NewGuid().ToString();

                String pathFolder = System.AppDomain.CurrentDomain.BaseDirectory + Common.Constant.CODE_DIR;
                String outputCompiler = pathFolder + fileName;

                TestCaseFileManagerController testcase_controller = new TestCaseFileManagerController();
                List<TestCaseResultModel> list_result_run_code = new List<TestCaseResultModel>();
                List<TestCaseFile> testcases = source.TestCase;

                foreach (TestCaseFile testCase in testcases)
                {
                    string code_run = this.ChangeCode(code, "", testCase.inputFile);
                    Dictionary<string, string> run_code_output = this.Run(code_run, fileName, pathFolder);     //run code and catch output
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
            StringBuilder resultCompiler = new StringBuilder();
            String outputCompiler = directory_path + filename_code;
            this.DeleteFile(outputCompiler);

            Dictionary<string, string> resultAPI = new Dictionary<string, string>();
            using (StreamWriter w = new StreamWriter(directory_path + filename_code + ".cs", true))
            {
                w.WriteLine(code);
            }

            CSharpCodeProvider ccp = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters(new[] { Constant.MSCOR_LIB,
                Constant.CORE_LIB }, outputCompiler, true);
            CompilerResults result = ccp.CompileAssemblyFromSource(parameters, code);
            //return result
            string status = Constant.STATUS_SUCCESS;

            if (result.Errors.HasErrors)
            {
                var listErrors = result.Errors.Cast<CompilerError>().ToList();
                foreach (var error in listErrors)
                {
                    resultCompiler.Append(error.ErrorText + "\r\n");
                }
                status = Constant.STATUS_FAIL;
                resultAPI.Add("status", status);
                resultAPI.Add("message", resultCompiler.ToString());
            }
            else
            {
                Dictionary<string, string> result_compiler = this.ExecuteCSC(directory_path, filename_code + ".cs");
                if (result_compiler["status"] == Constant.STATUS_FAIL)            //return if run javac fail
                    return result_compiler;
                
                resultAPI = this.ExecuteCMD(directory_path, filename_code);
            }
            this.DeleteFile(outputCompiler);
            return resultAPI;
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


        //Change file input name to full path
        protected string ChangeCode(string Code, string className, string inputFileName)
        {
            string app_path = AppDomain.CurrentDomain.BaseDirectory;
            string input_file_path = app_path + Constant.TESTCASE_DIR + inputFileName;
            input_file_path = input_file_path.Replace("\\", "/");
            Code = Code.Replace("INPUT_FILE_NAME", input_file_path);
            return Code;
        }
    }
}
