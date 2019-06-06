using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class CreateChallengeModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProblemStatement { get; set; }
        public string InputFormat { get; set; }
        public string Constraints { get; set; }
        public string OutputFormat { get; set; }
        public string Tags { get; set; }
    }
}