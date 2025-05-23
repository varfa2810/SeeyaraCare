using Dapper;
using GarageApplication.Interfaces;
using GarageDomain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageInfrastructure.Repositories
{
  
    public class Registration : IRegistration
    {
        private IDbConnection _dbConnection;

        public Registration(IDbConnection connection)

        {
            _dbConnection = connection;
        }

        
        public async Task<bool> Register(Register register)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);


            var parameters = new DynamicParameters();
            parameters.Add("@Email", register.Email);
            parameters.Add("@Password", hashedPassword);
            parameters.Add("@DOB", register.DOB);
            parameters.Add("@RoleID", register.RoleID); 
            parameters.Add("@FirstName", register.FirstName);
            parameters.Add("@LastName", register.LastName);

            var rowsAffected = await _dbConnection.ExecuteAsync(
                "InsertUser2",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

    }
}
