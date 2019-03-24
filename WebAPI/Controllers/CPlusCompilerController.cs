using Microsoft.VisualC;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebAPI.Common;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    public class CPlusCompilerController : ApiController
    {
        /// <summary>
        /// get output when compile string source
        /// </summary>
        /// <param name="source"> string of source</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Compiler(Source source)
        {
            try
            {
                StringBuilder resultCompiler = new StringBuilder();
                String outputCompiler = String.Empty;
                CppCodeProvider ccp = new CppCodeProvider();
                CompilerParameters parameters = new CompilerParameters(new[] { Constant.MSCOR_LIB,
            Constant.CORE_LIB }, outputCompiler, true);
                parameters.GenerateExecutable = true;
                CompilerResults result = ccp.CompileAssemblyFromSource(parameters, source.stringSource);
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
                    resultCompiler.Append(Constant.OUTPUT + outputCompiler);
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
