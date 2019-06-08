using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class CompeteDetailModel
    {
        
        public int ID { get; set; }

        public int OwnerID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm:ss}")]
        public DateTime? TimeEnd { get; set; }

        [Required]
        [Display(Name = "This compete is public")]
        public Boolean isPublic { get; set; }
    }
}