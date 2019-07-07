using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class DomainModel
    {
        public LANGUAGE Language { get; set; }
        public List<CHALLENGE> ListSolved = new List<CHALLENGE>();
        public List<CHALLENGE> ListUnsolved = new List<CHALLENGE>();
        public string status { get; set; }
        public string difficulty { get; set; }
    }
}