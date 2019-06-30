using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using WebAPI.Common;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// class compile for c#
    /// </summary>
    public class JavaCompilerController : ApiController
    {
        //private string text;
        protected Dictionary<string, string> ExecuteJava(string directory_file_code, string file_code = "MyClass")
        {
            /*Run java command*/
            string app_path = AppDomain.CurrentDomain.BaseDirectory;
            //set up
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = app_path + Constant.JAVA_EXECUTE_LINK + "java.exe";  //Link to java.exe  => "javac"
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = directory_file_code;        //Link to directory of file need to execute
            p.StartInfo.Arguments = file_code;          //=> "javac E:\MyClass"
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;

            //run
            p.Start();
            string result_string = p.StandardOutput.ReadToEnd(); ;
            string error_string = p.StandardError.ReadToEnd();
            p.WaitForExit(30000);

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

        protected Dictionary<string, string> ExecuteJavac(string directory_file_code, string file_code = "MyClass.java")
        {
            /*Run javac command*/
            string app_path = AppDomain.CurrentDomain.BaseDirectory;
            //set up
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = app_path + Constant.JAVA_EXECUTE_LINK + "javac.exe";  //Link to javac.exe  => "javac"
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = directory_file_code;                //Link to directory of file need to execute
            p.StartInfo.Arguments = file_code;             // =>    "javac E:\MyClass.java"
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;

            //run
            p.Start();
            string result_string = p.StandardOutput.ReadToEnd(); ;
            string error_string = p.StandardError.ReadToEnd();
            p.WaitForExit(30000);

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
        
        // POST api/values
        [HttpPost]
        public IHttpActionResult Compiler(Source source)
        {
            try
            {
                string code = source.stringSource;
                string app_path = AppDomain.CurrentDomain.BaseDirectory;        //get app_path
                string directory_file = app_path + Constant.FOLDER_CODE_DIR;    //get directory execute
                string filename_code = "MyClass" + source.userKey;              //file to execute
                string class_name = filename_code;                              //class name in file execute   ( = filename)
                string input_file = "";
                if (source.Data.Count > 0)
                {
                    input_file = source.Data["inputFile"] ?? "";                   //input file name to read when execute
                }
                
                //Change code
                code = this.ChangeCode(code, class_name, input_file);

                string path_file_execute = directory_file + "\\" + filename_code;
                this.DeleteFile(path_file_execute);             // make sure file not exit to create file

                /*write code to file.java*/
                using (StreamWriter w = new StreamWriter(path_file_execute + ".java", true))
                {
                    w.WriteLine(code); // Write the text
                }

                /*run javac E:\\MyClass.java*/
                Dictionary<string, string> result_javac = this.ExecuteJavac(directory_file, filename_code + ".java");
                if (result_javac["status"] == Constant.STATUS_FAIL)            //return if run javac fail
                    return Ok(result_javac);

                /*run java E:\\MyClass*/
                Dictionary<string, string> result_java = this.ExecuteJava(directory_file, filename_code);

                this.DeleteFile(path_file_execute);

                //return java execute
                //if (result_javac["status"] == Constant.STATUS_FAIL)
                //    return BadRequest(result_java["message"]);
                //return Ok(result_java["message"]);

                return Ok(result_java);
            }catch(Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        //Change code to specific with every user
        protected string ChangeCode(string Code, string className, string inputFileName)
        {
            string app_path = AppDomain.CurrentDomain.BaseDirectory;
            string input_file_path = app_path + Constant.TESTCASE_DIR + inputFileName;
            input_file_path = input_file_path.Replace("\\", "/");
            //Replace class name
            //Code = Code.Replace("class MyClass", "class " + className);
            Code = Regex.Replace(Code, @"(?<=class )(.*?)(?={)", className);       // class Abcdfegh   => class Classname

            //Replace Input and Output file name
            Code = Code.Replace("INPUT_FILE_NAME", input_file_path);

            return Code;
        }

        // Xoa 3 file java, class, exe
        protected void DeleteFile(string full_path)
        {
            if (System.IO.File.Exists(full_path))           //delete file MyClass and MyClass.java
            {
                System.IO.File.Delete(full_path);
            }
            if (System.IO.File.Exists(full_path + ".java"))
            {
                System.IO.File.Delete(full_path + ".java");
            }
            if (System.IO.File.Exists(full_path + ".class"))
            {
                System.IO.File.Delete(full_path + ".class");
            }
        }
    }
}
