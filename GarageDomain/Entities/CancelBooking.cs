using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDomain.Entities
{
    public class CancelBooking
    {
        [Required]
        public string UserID { get; set; }

        [Required]
        public string BookingID { get; set; }
    }
}
