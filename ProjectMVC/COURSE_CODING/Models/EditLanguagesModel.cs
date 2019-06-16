using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class EditLanguagesModel
    {
        [Key]
        public int ID { get; set; }
        public bool LanguageCSharp { get; set; }
        public bool LanguageCpp { get; set; }
        public bool LanguageJava { get; set; }
    }
}