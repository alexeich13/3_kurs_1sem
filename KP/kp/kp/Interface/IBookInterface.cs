using kp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kp.Interface
{
    public interface IBookInterface : IBaseInterface<Bookings>
    {
        IEnumerable<Bookings> GetAllList();
        Bookings Get(int id);
        void AddBooking(int userId, int packageId);
    }
}
