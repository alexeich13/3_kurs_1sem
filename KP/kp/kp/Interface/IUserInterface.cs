using kp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kp.Interface
{
    public interface IUserInterface : IBaseInterface<Users>
    {
        IEnumerable<Users> GetAllList();
        Users Get(int id);
    }
}
