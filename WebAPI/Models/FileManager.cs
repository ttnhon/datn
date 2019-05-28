using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class FileManager
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public string userKey { get; set; }
    }
}