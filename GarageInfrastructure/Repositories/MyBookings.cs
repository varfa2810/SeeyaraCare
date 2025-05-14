using Dapper;
using GarageApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageDomain.Entities;
namespace GarageInfrastructure.Repositories
{
    public class MyBookings: IMyBookings
    {
        private readonly IDbConnection _dbConnection;

        public MyBookings( IDbConnection connection)
        {
            _dbConnection = connection;

        }

        public async Task<List<MyBooking>> GetMyBookings(Guid userID)
        {
            string allBookings = @"SELECT * FROM ProductBookings WHERE UserID = @userID order by BookingDate desc";

            var myBookings = await _dbConnection.QueryAsync<MyBooking>(
                allBookings,
                new { userID }
            );

            return myBookings.ToList();
        }

        public async Task<bool> CancelBooking(CancelBooking cancelBooking)
        {
            string query = @"delete from ProductBookings where UserID = @userID and BookingID = @bookingID";


            int isCancelled = await _dbConnection.ExecuteAsync(
                query,
                new { userID = cancelBooking.UserID, bookingID = cancelBooking.BookingID }
            );

            return isCancelled > 0;
        }



    }
}
