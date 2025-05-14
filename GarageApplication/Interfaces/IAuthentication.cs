using GarageDomain.DTOs;
using GarageDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GarageApplication.Interfaces
{
    public interface IAuthentication
    {
        Task<string> GetOtp(LoginRequest loginUser);

        Task<UserDTO?> CheckUser(string email);

        Task<List<string>> GetUserRole(Guid userId);
        Task<bool> Register(RegisterUser registerUser);
    }
}
