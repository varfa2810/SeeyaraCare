using GarageDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageApplication.Interfaces
{
    public interface IMyBookings
    {
        Task<List<MyBooking>> GetMyBookings(Guid userID);

        Task<bool> CancelBooking(CancelBooking cancelBooking);
    }
}
