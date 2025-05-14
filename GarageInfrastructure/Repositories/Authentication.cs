using Azure.Core;
using Dapper;
using GarageApplication.Interfaces;
using GarageDomain.DTOs;
using GarageDomain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GarageInfrastructure.Repositories
{
    public class Authentication : IAuthentication
    {
        private IDbConnection _dbConnection;

        public Authentication(IDbConnection connection)

        {
            _dbConnection = connection;
        }
        private string GenerateOtp()
        {
            var random = new Random();
            var OTP = random.Next(100000, 999999).ToString();
            return OTP;
        }

      


        public async Task<string?> GetOtp(LoginRequest loginUser)
        {
            
            const string query = "SELECT Email, Password FROM Users2 WHERE Email = @Email";

            var user = await _dbConnection.QuerySingleOrDefaultAsync<InternalUser>(query, new { loginUser.Email });

            if (user == null)
                return null;

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password);

            if (!isPasswordCorrect)
                return null;


            var otp = GenerateOtp();
           
            return otp;

        }


        public async Task<UserDTO?> CheckUser(string email)
        {
            const string checkQuery = "SELECT * FROM Users2 WHERE Email = @Email";

            var result = await _dbConnection.QuerySingleOrDefaultAsync<UserDTO>(checkQuery, new { Email = email });

            if (result == null)
                return null;

            return new UserDTO
            {
                Id = result.Id,
                Email = result.Email,
                FirstName = result.FirstName,
                LastName = result.LastName,
                
            };
        }



        public async Task<List<string>> GetUserRole(Guid userId)
        {
            const string query = @"select RoleName from Roles2
                                   left join Users2 on Roles2.RoleID = Users2.RoleID
                                   where Users2.Id = @Id";
            var result = await _dbConnection.QueryAsync<string>(query, new { Id = userId });
            return result.ToList(); 
        }







        public async Task<bool> Register(RegisterUser registerUser)
        {
            const string query = @"INSERT INTO Users2 (FirstName, LastName, DOB, Email, Password, RoleID) 
                                  VALUES (@FirstName, @LastName, @DOB, @Email, @Password, 'FDBE0E1C-9771-4634-972B-3A145AF1F438')";

           
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);

            var result = await _dbConnection.ExecuteAsync(query, new
            {
                registerUser.FirstName,
                registerUser.LastName,
                registerUser.DOB,
                registerUser.Email,
                Password = hashedPassword,
              
            });

            return result > 0;
        }

    }
}
