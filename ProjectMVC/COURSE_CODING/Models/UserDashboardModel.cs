using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class UserDashboardModel
    {
        public USER_INFO Info { get; set; }
        public List<LANGUAGE_CODE> Languages = new List<LANGUAGE_CODE>();
        public DataAchieve Data = new DataAchieve();
        public List<Skill> Skills = new List<Skill>();
        public List<CHALLENGE> Challenges = new List<CHALLENGE>();
    }
}