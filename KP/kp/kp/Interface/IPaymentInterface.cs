using kp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kp.Interface
{
    public interface IPaymentInterface : IBaseInterface<Payments>
    {
        IEnumerable<Payments> GetAllList();
        Payments Get(int id);
        void MakePayment(int bookingId, decimal amount);
    }
}
