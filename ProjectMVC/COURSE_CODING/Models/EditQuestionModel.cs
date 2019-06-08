using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class EditQuestionModel
    {
        [Key]
        public int ID { get; set; }
        public List<Question> Questions = new List<Question>();
    }
}