using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class LoginModel
    {
        [Display(Name ="User name")]
        [Required(ErrorMessage ="Please enter your name!")]
        public  string UserName { get; set; }

        [Display(Name ="Password")]
        [Required(ErrorMessage ="Please enter your password!")]
        [StringLength(20,MinimumLength =6,ErrorMessage ="Your password length must be from 6 to 20")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}