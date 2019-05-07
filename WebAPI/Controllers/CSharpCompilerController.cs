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
            string result_string = string.Empty;
            string error_string = p.StandardError.ReadToEnd();
            result_string = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            //return result
            string status = Constant.STATUS_SUCCESS;
            string result_message = result_string;
            if (error_string != "")
            {
                status = Constant.STATUS_FAIL;
                result_message = error_string;
            }
            Dictionary<string, string> result = new Dictionary<string, string>();
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
            p.WaitForExit();

            //return result
            string status = "success";
            string result_message = result_string;
            if (error_string != "")
            {
                status = "fail";
                result_message = error_string;
            }
            Dictionary<string, string> result = new Dictionary<string, string>();
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
        public IHttpActionResult Compiler(Source source)
        {
            try
            {
                StringBuilder resultCompiler = new StringBuilder();
                String fileName = "Outputcshap"+Guid.NewGuid().ToString();
                String pathFolder = System.AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Code_File\Csharp\";
                String outputCompiler = pathFolder+fileName;
                Dictionary<string, string> resultAPI = new Dictionary<string, string>();
                using (StreamWriter w = new StreamWriter(pathFolder + fileName+".cs" , true))
                {
                    w.WriteLine(source.stringSource); 
                }

                CSharpCodeProvider ccp = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters(new[] { Constant.MSCOR_LIB,
                Constant.CORE_LIB }, outputCompiler, true);
                CompilerResults result = ccp.CompileAssemblyFromSource(parameters, source.stringSource);
                //return result
                string status = Constant.STATUS_SUCCESS;
                string result_message = string.Empty;
                   
                if (result.Errors.HasErrors)
                {
                    var listErrors = result.Errors.Cast<CompilerError>().ToList();
                    foreach (var error in listErrors)
                    {
                        resultCompiler.Append(error.ErrorText);
                    }
                    status = Constant.STATUS_FAIL;                  
                }
                else
                {
                    Dictionary<string, string> result_compiler = this.ExecuteCSC(pathFolder, fileName+".cs");
                    if (result_compiler["status"] == Constant.STATUS_FAIL)            //return if run javac fail
                        return Ok(result_compiler);

                    /*run java E:\\MyClass*/
                    resultAPI = this.ExecuteCMD(pathFolder, fileName);

                    this.DeleteFile(outputCompiler);
                }
                return Ok(resultAPI);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}
