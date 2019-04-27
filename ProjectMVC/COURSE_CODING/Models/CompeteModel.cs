using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class CompeteModel
    {
        //public  int         ID              { get; set; }
        //public  int         OwnerID         { get; set; }
        //public  string      Title           { get; set; }
        //public  string      Slug            { get; set; }
        //public  string      Desciption      { get; set; }
        //public  string      Rules           { get; set; }
        //public  int         TotalScore      { get; set; }
        //public  DateTime    TimeEnd         { get; set; }
        //public  int         ParticipantCount { get; set; }

        public List<COMPETE> competes = new List<COMPETE>();
        public List<LANGUAGE> languages = new List<LANGUAGE>();

    }
}