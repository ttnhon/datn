using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //private string text;
        protected Dictionary<string,string> ExecuteJava(string directory_file_code = "E:", string file_code = "MyClass")
        {
            /*Run java command*/
            //set up
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "C:\\Program Files\\Java\\jdk1.8.0_171\\bin\\java.exe";  //Link to java.exe  => "javac"
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
            if(error_string != "")
            {
                status = "fail";
                result_message = error_string;
            }
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("status", status);
            result.Add("message", result_message);
            return result;

        }

        protected Dictionary<string,string> ExecuteJavac(string directory_file_code = "E:", string file_code = "MyClass.java")
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "C:\\Program Files\\Java\\jdk1.8.0_171\\bin\\javac.exe";  //Link to javac.exe  => "javac"
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



        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {"ahuhu"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value "+id;
        }

        // POST api/values
        [HttpPost]
        public ActionResult<IEnumerable<string>> Post([FromBody] string code)
        {
            string directory_file = "E:";
            string filename_code = "MyClass";
            string full_path = directory_file + "\\" + filename_code;
            /*write code to file.java*/
            using (StreamWriter w = new StreamWriter(full_path + ".java", true))
            {
                w.WriteLine(code); // Write the text
            }

            /*run javac E:\\MyClass.java*/
            Dictionary<string, string> result_javac = this.ExecuteJavac(directory_file, filename_code + ".java");
            if (result_javac["status"] == "fail")            //run javac fail
                return new string[] { result_javac["status"], result_javac["message"] };

            /*run java E:\\MyClass*/
            Dictionary<string, string> result_java = this.ExecuteJava(directory_file, filename_code);

            if (System.IO.File.Exists(full_path))           //delete file MyClass and MyClass.java and MyClass.class
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


            return new string[] { result_java["status"], result_java["message"] };
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
