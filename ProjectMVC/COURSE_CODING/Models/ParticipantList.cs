using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class ParticipantList
    {
        public List<Participant> Participants { get; set; }

        public int ContestID { get; set; }

        public ParticipantList()
        {
            Participants = new List<Participant>();
        }
    }
}