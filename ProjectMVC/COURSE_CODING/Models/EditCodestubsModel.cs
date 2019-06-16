using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class EditCodestubsModel
    {
        [Key]
        public int ID { get; set; }
        public string CodeStubs_CSharp { get; set; }
        public string CodeStubs_Cpp { get; set; }
        public string CodeStubs_Java { get; set; }
    }
}