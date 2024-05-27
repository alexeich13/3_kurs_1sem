using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kp.Model;

namespace kp.Interface
{
    public interface IPackageInterface : IBaseInterface<Packages>
    {
        IEnumerable<Packages> GetAllList();
        Packages Get(int id);
    }
}
