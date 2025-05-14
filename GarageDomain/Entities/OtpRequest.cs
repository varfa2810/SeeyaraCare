using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDomain.Entities.Authentication
{
    public sealed class OtpRequest
    {
        [Required]
        [StringLength(6, ErrorMessage = "OTP must be 6 digits long.")]
        public string? OTP { get; set; }

        [Required]
        [EmailAddress]
        public string? Email  { get; set; }
    }
}
