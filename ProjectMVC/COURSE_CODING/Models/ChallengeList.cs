using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class ChallengeList
    {
        public List<CHALLENGE> AssignedChallenge { get; set; }
        public List<CHALLENGE> UnassignedChallenge { get; set; }

        public ChallengeList()
        {
            AssignedChallenge = new List<CHALLENGE>();
            UnassignedChallenge = new List<CHALLENGE>();
        }
    }
}