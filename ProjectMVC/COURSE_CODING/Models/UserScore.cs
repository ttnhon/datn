using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class UserScore
    {
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public int ScoreQuestion { get; set; }
        public int ScoreChallenge { get; set; }
        public int TotalScore { get; set; }
        public int QuestionDone { get; set; }
        public int QuestionNumber { get; set; }
        public int ChallengeDone { get; set; }
        public int ChallengeNumber { get; set; }

    }
}