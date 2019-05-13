using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Source
    {
        public string stringSource { get; set; }
        public string versionFramework { get; set; }
        public string userKey { get; set; }

        public Dictionary<string, string> Data = new Dictionary<string, string>();
    }
}