using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class Question
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        Answer[] List { get; set; }
    }
}