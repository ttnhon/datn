using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class Participant
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

    }
}