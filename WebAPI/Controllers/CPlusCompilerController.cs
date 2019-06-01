﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
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
        protected Dictionary<string, string> ExecuteCMD(string directory_file_code = "D:\\", string file_code = "MyClass")
        {
            /*Run cmd command*/
            //set up
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";  //run cmd
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = directory_file_code;        //Link to directory of file need to execute
            p.StartInfo.Arguments = "/C " + file_code;          //=> "MyClass"
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

        //create a.exe (defaut) file
        protected Dictionary<string, string> ExecuteGPP(string directory_file_code = "D:\\", string file_code = "MyClass.exe MyClass.cpp")
        {
            string ProjDir = System.AppDomain.CurrentDomain.BaseDirectory;
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = ProjDir + @"\Compilers\MinGW\bin\g++.exe";  //Link to g++.exe  => "g++"
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = directory_file_code;                //Link to directory of file need to execute
            p.StartInfo.Arguments = "-o " + file_code;             // =>    "g++ -o MyClass.exe MyClass.cpp"
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
        
        /// <summary>
        /// get output when compile string source
        /// </summary>
        /// <param name="source"> string of source</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Compiler(Source source)
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
            string full_path = directory_file + "\\" + filename_code;
            //make sure this file not exist
            if (System.IO.File.Exists(full_path + ".cpp"))
            {
                System.IO.File.Delete(full_path + ".cpp");
            }
            
            /*write code to file.cpp*/
            using (StreamWriter w = new StreamWriter(full_path + ".cpp", true))
            {
                w.WriteLine(source.stringSource); // Write the text
            }

            /*run g++ dir\\MyClass{userKey}.cpp*/
            Dictionary<string, string> result_gpp = this.ExecuteGPP(directory_file, filename_code + ".exe " + filename_code + ".cpp");
            if (result_gpp["status"] == "fail")            //return if run g++ fail
            {
                //delete MyClass.cpp
                if (System.IO.File.Exists(full_path + ".cpp"))
                {
                    System.IO.File.Delete(full_path + ".cpp");
                }
                return BadRequest(result_gpp["message"]);
            }

            /*run D:\\MyClass*/
            Dictionary<string, string> result_cpp = this.ExecuteCMD(directory_file, filename_code);

            if (System.IO.File.Exists(full_path + ".exe"))           //delete file MyClass{userKey}.exe and MyClass{userKey}.cpp
            {
                System.IO.File.Delete(full_path + ".exe");
            }
            if (System.IO.File.Exists(full_path + ".cpp"))
            {
                System.IO.File.Delete(full_path + ".cpp");
            }

            //return g++ execute
            //if (result_gpp["status"] == "fail")
            //    return BadRequest(result_cpp["message"]);
            return Ok(result_cpp);

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
    }
}
