using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            //set up
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "C:\\Users\\Admin\\Downloads\\Document\\jdk1.8.0_151\\bin\\java.exe";  //Link to java.exe  => "javac"
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

        protected Dictionary<string, string> ExecuteJavac(string directory_file_code, string file_code = "MyClass.java")
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "C:\\Users\\Admin\\Downloads\\Document\\jdk1.8.0_151\\bin\\javac.exe";  //Link to javac.exe  => "javac"
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
        
        // POST api/values
        [HttpPost]
        public IHttpActionResult Compiler(Source source)
        {
            string code = source.stringSource;
            string directory_file = "D:\\TestCompiler";
            string filename_code = "MyClass";
            string full_path = directory_file + "\\" + filename_code;
            this.DeleteFile(full_path);
            /*write code to file.java*/
            using (StreamWriter w = new StreamWriter(full_path + ".java", true))
            {
                w.WriteLine(code); // Write the text
            }

            /*run javac E:\\MyClass.java*/
            Dictionary<string, string> result_javac = this.ExecuteJavac(directory_file, filename_code + ".java");
            if (result_javac["status"] == "fail")            //return if run javac fail
                return BadRequest(result_javac["message"]);

            /*run java E:\\MyClass*/
            Dictionary<string, string> result_java = this.ExecuteJava(directory_file, filename_code);

            this.DeleteFile(full_path);
            //return java execute
            if (result_javac["status"] == "fail")
                return BadRequest(result_java["message"]);
            return Ok(result_java["message"]);
        }
        
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
