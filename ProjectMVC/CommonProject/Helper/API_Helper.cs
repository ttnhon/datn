using CommonProject.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

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
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", this.JWT());
            var responsePostTask =  client.PostAsJsonAsync<Source>("", src);
            Dictionary<string, string> resultAPI = new Dictionary<string, string>();
            try
            {
                responsePostTask.Wait();
                var result = responsePostTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Dictionary<string, string>>();
                    readTask.Wait();
                    resultAPI = readTask.Result;
                }
                return resultAPI;
            }
            catch (Exception e)
            {
                return resultAPI;
            }
        }

        /// <summary>
        /// in client call this method need to check result return with flag: result.IsSuccessStatusCode
        /// </summary>
        /// <param name="typeAPI"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public List<TestCaseResultModel> RequestAPIRunCode(string typeAPI, Source src)
        {
            typeAPI = typeAPI.ToUpper();

            // choose type api
            if (CommonConstant.TYPE_CSHARP_COMPILER.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTER_CSHARP_RUNCODE_CHALLENGE_API);
            }
            else if (CommonConstant.TYPE_CPLUS_COMPILER.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTER_CPLUS_RUNCODE_CHALLENGE_API);
            }
            else if (CommonConstant.TYPE_JAVA_COMPILER.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTER_JAVA_RUNCODE_CHALLENGE_API);
            }

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", this.JWT());
            var responsePostTask = client.PostAsJsonAsync<Source>("", src);
            List<TestCaseResultModel> resultAPI = new List<TestCaseResultModel>();
            try
            {
                responsePostTask.Wait();
                var result = responsePostTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<TestCaseResultModel>>();
                    readTask.Wait();
                    resultAPI = readTask.Result;
                }
                return resultAPI;
            }
            catch (Exception e)
            {
                return resultAPI;
            }
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
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", this.JWT());
            var responsePostTask = client.PostAsJsonAsync<Dictionary<int, Dictionary<string, string>>>("", TestCase);
            Dictionary<int, Dictionary<string, string>> resultAPI = new Dictionary<int, Dictionary<string, string>>();
            try
            {
                responsePostTask.Wait();
                var result = responsePostTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Dictionary<int, Dictionary<string, string>>>();
                    readTask.Wait();
                    resultAPI =  readTask.Result;
                }
                return resultAPI;
            }
            catch (Exception e)
            {
                return resultAPI;
            }
            
        }

        /// <summary>
        /// in client call this method need to check result return with flag: result.IsSuccessStatusCode
        /// </summary>
        /// <param name="typeAPI"></param>
        /// <param name="file"></param>
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
            else if (CommonConstant.TYPE_DELETE_FILE_API.Equals(typeAPI))
            {
                client.BaseAddress = new Uri(CommonConstant.URL_HOST_API + CommonConstant.ROUTE_DELETE_TESTCASE_API);
            }

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", this.JWT());
            var responsePostTask = client.PostAsJsonAsync("", file);
            try
            {
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
            catch (Exception e)
            {
                return e.ToString();
            }
            
        }

        protected string JWT()
        {
            int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string jwt = Helper.Encrypt.EncryptString((unixTimestamp + CommonConstant.TIME_ALIVE_TOKEN).ToString(), CommonConstant.SECRET_KEY_TOKEN);
            return jwt;
        }
    }
}
