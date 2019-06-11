using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class CompeteChallengesModel
    {
        public COMPETE compete { get; set; }
        public List<CHALLENGE> challenges = new List<CHALLENGE>();
        public string status { get; set; }
        public string difficulty { get; set; }

        public List<bool> questions = new List<bool>();
    }
}