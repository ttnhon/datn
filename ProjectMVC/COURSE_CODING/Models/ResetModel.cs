using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class ResetModel
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your email!")]
        [StringLength(50)]
        public string Email { get; set; }
    }
}