using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class Question
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Score { get; set; }
        public string Description { get; set; }
        public short? Type { get; set; }
        public Answer[] List { get; set; }
    }

    public enum QuestionType
    {
        Single,
        Multiple
    }
}