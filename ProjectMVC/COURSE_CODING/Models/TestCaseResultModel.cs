using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class TestCaseResultModel
    {
        public string Status { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string OutputExpect { get; set; }
    }
}