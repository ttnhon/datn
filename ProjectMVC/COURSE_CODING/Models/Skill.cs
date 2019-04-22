using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class Skill
    {
        public LANGUAGE_CODE Language { get; set; }
        public int Solved { get; set; }
        public int Count { get; set; }
    }
}