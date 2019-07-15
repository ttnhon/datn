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
        [Display(Name ="First Name")]
        [Required(ErrorMessage = "Please enter your first name!")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name!")]
        public string LastName { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please enter your country!")]
        public string Country { get; set; }

        public USER_INFO Info { get; set; }
        public SCHOOL School { get; set; }
        public List<DoneChallenge> Challenges = new List<DoneChallenge>();
        public List<COMPETE> Competes = new List<COMPETE>();

        public UserProfileModel()
        {
            Competes = new List<COMPETE>();
            Challenges = new List<DoneChallenge>();
        }
        public List<Skill> skills = new List<Skill>();
    }
}