using DAO.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class EditModeratorModel
    {
        [Key]
        public int ID { get; set; }
        public List<USER_INFO> Moderators = new List<USER_INFO>();
    }
}