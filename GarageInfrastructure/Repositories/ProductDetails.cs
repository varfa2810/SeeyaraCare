using Dapper;
using GarageApplication.Interfaces;
using GarageDomain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageInfrastructure.Repositories
{
    public class ProductDetails : IProductDetails
    {
        private readonly IDbConnection _dbConnection;

        public ProductDetails(IDbConnection connection)
        {
            _dbConnection = connection;

        }

        public async Task<Products> GetProductDetailsById(Guid productID)
        {
            string query = @"SELECT * FROM Products WHERE ProductID = @productID";

            var productDetails = await _dbConnection.QueryFirstOrDefaultAsync<Products>(
                query,
                new { productID }
            );

            return productDetails;
        }


        public async Task<bool> BookProduct(BookProduct bookProduct)
        {
            string query = @"INSERT INTO ProductBookings (ProductID, UserID, Quantity, TotalPrice, IsFullFilled) 
                             VALUES (@ProductID, @UserID, @Quantity, @TotalPrice, 0)";
            var result = await _dbConnection.ExecuteAsync(
                query,
                new { bookProduct.ProductID, bookProduct.UserID, bookProduct.Quantity, bookProduct.TotalPrice });

            return result > 0;
        }

    }
}
