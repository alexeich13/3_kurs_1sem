using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adminkp.Model;

namespace Adminkp.Interfaces
{
    public interface ITourInterface : IBaseInterface<PackageInfo>
    {
        IEnumerable<PackageInfo> GetAllList();
        PackageInfo Get(int packageId);
        void AddPackage(int tourOperatorId, int destId, string name, decimal price, DateTime startDate, DateTime endDate);
        void UpdatePackage(int tourNumber, int tourOperatorId, int destId, string name, decimal price, DateTime startDate, DateTime endDate);
        void DeletePackage(int packageId);
    }
    public class PackageInfo
    {
        public int PackageId { get; set; }
        public string TourOperator { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
