using DAO.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class QuestionModel
    {
        //public int userId { get; set; }
        //public List<CHALLENGE> lsChallenges { get; set; }
        
        public string title { get; set; }
        public short? type { get; set; }
        public dynamic answers { get; set; }
        public List<int> last_choised = new List<int>();
    }
}