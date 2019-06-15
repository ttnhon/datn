using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class InvitationModel
    {
        public int contestID { get; set; }

        public string contestName { get; set; }

        public USER_INFO contestOwner { get; set; }

    }
}