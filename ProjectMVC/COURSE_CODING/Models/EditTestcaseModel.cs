using CommonProject.Models;
using DAO.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class EditTestcaseModel
    {
        [Key]
        public int ID { get; set; }
        public List<TESTCASE> TestCases = new List<TESTCASE>();
        public List<TestCaseResultModel> TestCaseContent = new List<TestCaseResultModel>();
    }
}