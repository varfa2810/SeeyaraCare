using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageApplication.Interfaces
{
    public interface INotification
    {
        Task<bool> SendEmail(string email, string subject, string message);
    }
}
