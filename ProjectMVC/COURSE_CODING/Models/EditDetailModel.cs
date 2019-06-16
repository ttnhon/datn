using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class EditDetailModel
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Language")]
        public Language Language { get; set; }

        [Display(Name = "Challenge Difficulty")]
        public Difficulty Difficulty { get; set; }

        [Display(Name = "Challenge Name")]
        public string Name { get; set; }

        public string Slug { get; set; }

        [Display(Name = "Score")]
        public int Score { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Problem Statement")]
        public string ProblemStatement { get; set; }

        [Display(Name = "Input Format")]
        public string InputFormat { get; set; }

        [Display(Name = "Constraints")]
        public string Constraints { get; set; }

        [Display(Name = "Output Format")]
        public string OutputFormat { get; set; }

        [Display(Name = "Tags")]
        public string Tags
        {
            get; set;
        }
    }
    public enum Language
    {
        English,
        Vietnamese
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Advanced,
        Expert
    }
}