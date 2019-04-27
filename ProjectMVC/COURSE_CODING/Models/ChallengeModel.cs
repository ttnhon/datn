using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class ChallengeModel
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string InputFormat { get; set; }
        public string OutputFormat { get; set; }
        public int ChallengeDifficulty { get; set; }
        public string Constraints { get; set; }
        public int TimeDo { get; set; }
        public int Score { get; set; }
        public string Solution { get; set; }
        public string Tags { get; set; }

        public List<LANGUAGE_CODE> languages = new List<LANGUAGE_CODE>();
    }
}