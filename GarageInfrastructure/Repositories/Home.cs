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
    
    public class Home : IHome
    {
        private readonly IDbConnection _dbConnection;

        public Home(IDbConnection connection)
        {
            _dbConnection = connection;
                
        }

        
        public async Task<List<Products>> GetAllProducts()
        {
            string allProducts = @"SELECT * FROM Products";
            var allProductsList = await _dbConnection.QueryAsync<Products>(
                allProducts
            );
            return allProductsList.ToList();
            }

    }
}
