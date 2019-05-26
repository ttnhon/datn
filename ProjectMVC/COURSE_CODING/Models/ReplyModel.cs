using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class ReplyModel
    {
        public REPLY reply { get; set; }
        public USER_INFO owner { get; set; }
    }
}