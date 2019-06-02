using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class EmailModel
    {
        [Required(ErrorMessage = "Please enter user name!")]
        [StringLength(50)]
        [Display(Name = "User name")]
        public string Name { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please enter phone!")]
        [StringLength(50)]
        public string Mobile { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter email!")]
        [StringLength(50)]
        public string Email { get; set; }

        public string Content { get; set; }
    }
}