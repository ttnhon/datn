using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Common
{
    public class StatusDefinition
    {
        public int ID { get; set; }
        public string NameStatus { get; set; }
        public StatusDefinition(int id,string nameStatus)
        {
            this.ID = id;
            this.NameStatus = nameStatus;
        }
    }
}