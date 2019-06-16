using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class EditSettingsModel
    {
        [Key]
        public int ID { get; set; }
        public bool DisCompileTest { get; set; }
        public bool DisSubmissions { get; set; }
        public bool PublicSolutions { get; set; }
        public bool IsPublic { get; set; }
    }
}