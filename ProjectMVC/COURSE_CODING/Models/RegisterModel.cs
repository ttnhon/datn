using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COURSE_CODING.Models
{
    public class RegisterModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage ="Please enter your user name!")]
        [StringLength(50)]
        [Display(Name ="User name")]
        public string UserName { get; set; }

        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Please enter your password")]
        [StringLength(20,MinimumLength =6,ErrorMessage ="Please enter password have length from 6 to 20!")]
        public string PasswordUser { get; set; }

        [Display(Name ="Confirm password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Please enter confirm password have length from 6 to 20!")]
        [Required(ErrorMessage = "Please enter your password")]
        [Compare("PasswordUser",ErrorMessage ="Confirm password not match with your password!")]
        public string ComfirmPasswordUser { get; set; }

        [Display(Name ="First name")]
        [Required(ErrorMessage ="Please enter your first name!")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name ="Last name")]
        [Required(ErrorMessage ="Please enter your last name! ")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="Please enter your email!")]
        [StringLength(50)]
        public string Email { get; set; }
    }
}