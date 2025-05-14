using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDomain.Entities
{
    public class BookProduct
    {
        public Guid UserID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
       
    }
}
