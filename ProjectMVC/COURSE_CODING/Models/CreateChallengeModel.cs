using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class CreateChallengeModel
    {
        [Display(Name = "Challenge Difficulty")]
        public Difficulty Difficulty { get; set; }

        [Display(Name = "Challenge Name")]
        [Required]
        public string Name { get; set; }

        public string Slug { get; set; }

        [Display(Name = "Score")]
        [Required]
        public int Score { get; set; }

        [Display(Name = "Description")]
        [Required]
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
        public string Tags { get; set; }

        public int competeID { get; set; }
    }
}