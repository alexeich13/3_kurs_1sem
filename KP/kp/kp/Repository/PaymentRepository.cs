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
    public class PaymentRepository : IPaymentInterface
    {
        private readonly Model.AppContext dbContext;
        public PaymentRepository()
        {
            dbContext = new Model.AppContext();
        }
        public IEnumerable<Payments> GetAllList()
        {
            return dbContext.Payments.ToList();
        }
        public Payments Get(int id)
        {
            return dbContext.Payments.Find(id);
        }
        public void MakePayment(int bookingId, decimal amount)
        {
            var bookingToUpdate = dbContext.Bookings.FirstOrDefault(b => b.booking_id == bookingId);

            if (bookingToUpdate != null)
            {
                if (bookingToUpdate.payment_status == "Paid")
                {
                    throw new InvalidOperationException("Это бронирование уже оплачено.");
                }

                Payments newPayment = new Payments
                {
                    booking_id = bookingId,
                    payment_date = DateTime.Now,
                    amount = amount
                };

                bookingToUpdate.payment_status = "Paid";
                dbContext.Payments.Add(newPayment);
                dbContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Бронирование не найдено.");
            }
        }
    }
}
