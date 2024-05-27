using kp.Interface;
using kp.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kp.Repository
{
    public class BookingRepository : IBookInterface
    {
        private readonly Model.AppContext dbContext;
        public BookingRepository()
        {
            dbContext = new Model.AppContext();
        }
        public IEnumerable<Bookings> GetAllList()
        {
            return dbContext.Bookings.ToList();
        }
        public Bookings Get(int id)
        {
            return dbContext.Bookings.Find(id);
        }
        public void AddBooking(int userId, int packageId)
        {
            Bookings newBooking = new Bookings
            {
                users_id = userId,
                package_id = packageId,
                booking_date = DateTime.Now,
                payment_status = "Pending"
            };
            dbContext.Bookings.Add(newBooking);
            dbContext.SaveChanges();
        }
    }
}
