using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class ForgotPasswordModel
    {

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter your password!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Your password length must be from 6 to 20")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Please enter confirm password have length from 6 to 20!")]
        [Required(ErrorMessage = "Please enter your password")]
        [Compare("Password", ErrorMessage = "Confirm password not match with your password!")]
        public string ComfirmPassword { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your email!")]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "Code validate")]
        [Required(ErrorMessage = "Please enter your code validate!")]
        public string CodeValidate { get; set; }

        public Boolean isResetPassword { get; set; }
    }
}