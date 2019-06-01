using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class DoneChallenge 
    {
        public CHALLENGE challenge { get; set; }
        public DateTime timeDone { get; set; }
    }
}