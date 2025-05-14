using GarageDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageApplication.Interfaces
{
    public interface IProductDetails
    {
        Task<Products> GetProductDetailsById(Guid productID);
        Task<bool> BookProduct(BookProduct bookProduct);
    }
}
