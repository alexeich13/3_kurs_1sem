using Adminkp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adminkp.Interfaces
{
    public interface IBookingInterface : IBaseInterface<Bookings>
    {
        IEnumerable<Bookings> GetAllList();
        Bookings Get(int packageId);
        void CancelBooking(int BookingId);
    }
}
