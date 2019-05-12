using System;
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
    public class UploadController : ApiController
    {
        /// <summary>
        /// upload string to create file in server
        /// </summary>
        /// <param name="file"> content of file</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Upload(File file)
        {
            try
            {
                //return full path of file
                string res = "";
                string content = file.Content;
                string app_path = AppDomain.CurrentDomain.BaseDirectory;
                //string directory_file = app_path + Constant.FOLDER_CODE_DIR;
                //string filename_code = "MyClass" + source.userKey;
                //string full_path = directory_file + "\\" + filename_code;

                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}