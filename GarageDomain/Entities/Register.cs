using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDomain.Entities
{
    public class Register
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
      ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.DateTime)]

        public DateTime DOB {  get; set; }



        [Required(ErrorMessage = "First name is required.")]
        [StringLength(15)]
        public string? FirstName { get; set; }


        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(15)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        
        public string? RoleID { get; set; }
    }

}
