using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adminkp.Interfaces
{
    public interface IBaseInterface<T>
    {
        IEnumerable<T> GetAllList();
        T Get(int id);
    }
}
