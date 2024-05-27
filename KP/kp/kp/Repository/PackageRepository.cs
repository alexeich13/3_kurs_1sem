using kp.Interface;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kp.Model;

namespace kp.Repository
{
    public class PackageRepository : IPackageInterface
    {
        private readonly Model.AppContext dbContext;
        public PackageRepository()
        {
            dbContext = new Model.AppContext();
        }
        public IEnumerable<Packages> GetAllList()
        {
            return dbContext.Packages.ToList();
        }
        public Packages Get(int id)
        {
            return dbContext.Packages.Find(id);
        }
    }
}
