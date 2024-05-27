using Adminkp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adminkp.Model;
using System.Data.Entity;

namespace Adminkp.Repository
{
    public class BookingRepository : IBookingInterface
    {
        private readonly Model.ApplicationContext dbContext;
        public BookingRepository()
        {
            dbContext = new Model.ApplicationContext();
        }
        public IEnumerable<Bookings> GetAllList()
        {
            return dbContext.Bookings.ToList();
        }
        public Bookings Get(int id)
        {
            return dbContext.Bookings.Find(id);
        }
        public void CancelBooking(int bookingId)
        {
            var bookingToCancel = dbContext.Bookings.FirstOrDefault(b => b.booking_id == bookingId && b.payment_status == "Pending");

            if (bookingToCancel != null)
            {
                dbContext.Bookings.Remove(bookingToCancel);
                dbContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Бронирование с указанным идентификатором уже оплачено или не найдено");
            }
        }
    }
}
