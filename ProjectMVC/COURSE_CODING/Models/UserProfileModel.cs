using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class UserProfileModel
    {
        public USER_INFO Info { get; set; }
        public SCHOOL School { get; set; }
        public List<CHALLENGE> Challenges = new List<CHALLENGE>();
        public List<COMPETE> Competes = new List<COMPETE>();
    }
}