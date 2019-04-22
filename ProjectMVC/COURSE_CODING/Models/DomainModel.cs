using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class DomainModel
    {
        public LANGUAGE_CODE Language { get; set; }
        public List<CHALLENGE> List = new List<CHALLENGE>();
        public string status { get; set; }
        public string difficulty { get; set; }
    }
}