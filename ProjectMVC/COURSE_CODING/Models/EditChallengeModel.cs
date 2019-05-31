using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class EditChallengeModel
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
        public string Tags { get; set; }
        //moderators
        public List<USER_INFO> Moderators = new List<USER_INFO>();
        //test cases
        public List<TESTCASE> TestCases = new List<TESTCASE>();
        public List<TestCaseResultModel> TestCaseContent = new List<TestCaseResultModel>();
        //code stubs
        public string CodeStubs_CSharp { get; set; }
        public string CodeStubs_Cpp { get; set; }
        public string CodeStubs_Java { get; set; }
        //languages
        public bool LanguageCSharp { get; set; }
        public bool LanguageCpp { get; set; }
        public bool LanguageJava { get; set; }
        //settings
        public bool DisCompileTest { get; set; }
        public bool DisCustomTestcase { get; set; }
        public bool DisSubmissions { get; set; }
        public bool PublicTestcase { get; set; }
        public bool PublicSolutions { get; set; }
        //editorial
        public string RequiredKnowledge { get; set; }
        public string TimeComplexity { get; set; }
        public string Editorialist { get; set; }
        public bool PartialEditorial { get; set; }
        public string Approach { get; set; }
        public string ProblemSetter { get; set; }
        public string SetterCode { get; set; }
        public string ProblemTester { get; set; }
        public string TesterCode { get; set; }
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