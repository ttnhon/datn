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
        public Dictionary<string, string> RequestAPI(string typeAPI,Source src)
        {
            typeAPI = typeAPI.ToUpper();

            // choose type api
            if (CommonConstant.TYPE_CSHARP_COMPILER.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTER_CSHARP_COMPILER_API);
            }
            else if (CommonConstant.TYPE_CPLUS_COMPILER.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTER_CPLUS_COMPILER_API);
            }
            else if (CommonConstant.TYPE_JAVA_COMPILER.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTER_JAVA_COMPILER_API);
            }
            
            client.DefaultRequestHeaders.Accept.Clear();
          
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

        /// <summary>
        /// in client call this method need to check result return with flag: result.IsSuccessStatusCode
        /// </summary>
        /// <param name="typeAPI"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, string>> ReadTestCase(Dictionary<int,Dictionary<string,string>> TestCase)
        {
            client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTE_TESTCASE_API);

            client.DefaultRequestHeaders.Accept.Clear();

            var responsePostTask = client.PostAsJsonAsync<Dictionary<int, Dictionary<string, string>>>("", TestCase);
            responsePostTask.Wait();
            var result = responsePostTask.Result;
            Dictionary<int, Dictionary<string, string>> resultAPI = new Dictionary<int, Dictionary<string, string>>();
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Dictionary<int, Dictionary<string, string>>>();
                readTask.Wait();
                resultAPI = readTask.Result;
            }
            return resultAPI;
        }

        /// <summary>
        /// in client call this method need to check result return with flag: result.IsSuccessStatusCode
        /// </summary>
        /// <param name="typeAPI"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public string RequestUploadAPI(FileManager file, string typeAPI)
        {
            if (CommonConstant.TYPE_UPLOAD_FILE_API.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTE_UPLOAD_TESTCASE_API);
            }
            else if (CommonConstant.TYPE_UPDATE_FILE_API.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTE_UPDATE_TESTCASE_API);
            }

            client.DefaultRequestHeaders.Accept.Clear();

            var responsePostTask = client.PostAsJsonAsync("", file);
            responsePostTask.Wait();
            var result = responsePostTask.Result;
            string resultAPI = "";
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<string>();
                readTask.Wait();
                resultAPI = readTask.Result;
            }
            return resultAPI;
        }
    }
}
