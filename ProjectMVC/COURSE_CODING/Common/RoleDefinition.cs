using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Common
{
    public class RoleDefinition
    {
        public int ID { get; set; }
        public string NameRole { get; set; }
        public RoleDefinition(int id,string nameRole)
        {
            this.ID = id;
            this.NameRole = nameRole;
        }
    }
}