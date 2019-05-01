using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
                String fileName = "Out.exe";
                String pathFolder = System.AppDomain.CurrentDomain.BaseDirectory + @"Compilers\";
                String outputCompiler = pathFolder+fileName;
                CSharpCodeProvider ccp = new CSharpCodeProvider();
                ICodeCompiler icc = ccp.CreateCompiler();
                    CompilerParameters parameters = new CompilerParameters(new[] { Constant.MSCOR_LIB,
                Constant.CORE_LIB }, outputCompiler, true);
                //CompilerParameters parameters = new CompilerParameters();
                //parameters.GenerateExecutable = true;
                //parameters.OutputAssembly = outputCompiler;
                CompilerResults result = icc.CompileAssemblyFromSource(parameters, source.stringSource);
                if (result.Errors.HasErrors)
                {
                    var listErrors = result.Errors.Cast<CompilerError>().ToList();
                    foreach (var error in listErrors)
                    {
                        resultCompiler.Append(error.ErrorText);
                    }
                }
                else
                {
                    //resultCompiler.Append(Constant.OUTPUT + outputCompiler);
                    //string a= result.PathToAssembly;
                    //Module module = result.CompiledAssembly.GetModules()[0];
                    //Type mt = null;
                    //MethodInfo methInfo = null;
                    //if (module != null)
                    //{
                    //    mt = module.GetType("DynaCore.DynaCore");
                    //}
                    //if (mt != null)
                    //{
                    //    methInfo = mt.GetMethod("Main");
                    //}
                    //if (methInfo != null)
                    //{
                    //    Console.WriteLine(methInfo.Invoke(null, new object[] { "here in dyna code" }));
                    //    outputCompiler = methInfo.Invoke(null, new object[] { "here in dyna code" }).ToString();
                    //    resultCompiler.Append(Constant.OUTPUT + outputCompiler);
                    //}         


                    //Assembly loAssembly = loCompiled.CompiledAssembly;
                    //// *** Retrieve an obj ref – generic type only
                    //object loObject = loAssembly.CreateInstance("MyNamespace.MyClass");
                    //if (loObject == null)
                    //{
                    //    MessageBox.Show("Couldn't load class.");
                    //    return;
                    //}
                    //object[] loCodeParms = new object[1];
                    //loCodeParms[0] = "West Wind Technologies";
                    //try
                    //{
                    //    object loResult = loObject.GetType().InvokeMember(
                    //                     "DynamicCode", BindingFlags.InvokeMethod,
                    //                     null, loObject, loCodeParms);
                    //    DateTime ltNow = (DateTime)loResult;
                    //    MessageBox.Show("Method Call Result:\r\n\r\n" +
                    //                    loResult.ToString(), "Compiler Demo");
                    //}
                    //catch (Exception loError)
                    //{
                    //    MessageBox.Show(loError.Message, "Compiler Demo");
                    //}

                  //  Get the compiled method and execute it.
                    //foreach (Type a_type in result.CompiledAssembly.GetTypes())
                    //{
                    //    Object instanceClass = result.CompiledAssembly.CreateInstance(a_type.Name);
                    //    // Get a MethodInfo object describing the SayHi method.
                    //    MethodInfo method_info = a_type.GetMethod("Main");
                    //    if (method_info != null && a_type.IsClass && !a_type.IsNotPublic)
                    //    {
                    //        object[] method_params = new object[] { this };
                    //        // Execute the method.
                    //        //outputCompiler = method_info.Invoke(instanceClass,BindingFlags.InvokeMethod,
                    //        //     null, method_params, CultureInfo.CurrentCulture).ToString();
                    //        outputCompiler = method_info.Invoke(null, method_params).ToString();
                    //        resultCompiler.Append(Constant.OUTPUT + outputCompiler);

                    //    }
                    //}


                    //System.Diagnostics.Process p = new System.Diagnostics.Process();
                    //p.StartInfo.FileName = @"cmd.exe";  //run cmd
                    //p.StartInfo.UseShellExecute = false;
                    //p.StartInfo.WorkingDirectory = pathFolder;        //Link to directory of file need to execute
                    //p.StartInfo.Arguments = @"/c" + pathFolder + fileName;         //=> "MyClass"
                    //p.StartInfo.CreateNoWindow = true;
                    //p.StartInfo.RedirectStandardInput = true;
                    //p.StartInfo.RedirectStandardOutput = true;
                    //p.StartInfo.RedirectStandardError = true;

                    ////run
                    //p.Start();
                    //string result_string = p.StandardOutput.ReadToEnd();
                    //string error_string = p.StandardError.ReadToEnd();
                    //p.WaitForExit();

                    //  string resultString = Process.Start(@"cmd.exe ", @"/c" + outputCompiler).StandardOutput.ReadToEnd();
                    // Process.Start(@"cmd.exe ", @"/c"+ outputCompiler).WaitForExit();
                }
                return Ok(resultCompiler);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}
