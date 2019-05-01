﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace CommonProject
{
    /// <summary>
    /// Handel call API 
    /// </summary>
    public class API_Helper
    {
        private HttpClient client;    

        /// <summary>
        /// create api helper with type API constant
        /// </summary>
        /// <param name="typeAPI"></param>
        public API_Helper()
        {
            client = new HttpClient();                
        }

        /// <summary>
        /// in client call this method need to check result return with flag: result.IsSuccessStatusCode
        /// </summary>
        /// <param name="typeAPI"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public Dictionary<string, string> RequestAPI(int typeAPI,Source src)
        {
            // choose type api
            if (CommonConstant.TYPE_CSHAP_COMPILER.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTER_CSHAP_COMPILER_API);
            }
            else if (CommonConstant.TYPE_CPLUSS_COMPILER.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTER_CPLUSS_COMPILER_API);
            }
            else if (CommonConstant.TYPE_JAVA_COMPILER.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTER_JAVA_COMPILER_API);
            }
            //client.BaseAddress = new Uri("http://localhost:52508/api/CSharpCompiler/Compiler");
            client.DefaultRequestHeaders.Accept.Clear();
           // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var responsePostTask =  client.PostAsJsonAsync<Source>("", src);
            responsePostTask.Wait();
            var result = responsePostTask.Result;
            Dictionary<string, string> resultAPI = new Dictionary<string, string>();
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Dictionary<string, string>>();
                readTask.Wait();
                resultAPI = readTask.Result;
            }
            return resultAPI;
        
        }
    }
}
