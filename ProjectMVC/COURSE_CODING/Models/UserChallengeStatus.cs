using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class UserChallengeStatus
    {

        public int ID { get; set; }
        public string Title { get; set; }
        public int Difficulty { get; set; }
        public int Score { get; set; }
        public bool isSolved { get; set; }
    }
}