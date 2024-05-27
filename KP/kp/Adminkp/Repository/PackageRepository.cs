using Adminkp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adminkp.Model;

namespace Adminkp.Repository
{
    public class PackageRepository : ITourInterface
    {
        private readonly Model.ApplicationContext _dbContext;
        public PackageRepository()
        {
            _dbContext = new Model.ApplicationContext();
        }
        public IEnumerable<PackageInfo> GetAllList()
        {
            var query =
                from packages in _dbContext.Packages
                join destinations in _dbContext.Destinations
                on packages.destination_id equals destinations.destination_id
                join touroperators in _dbContext.TourOperators
                on packages.tour_operator_id equals touroperators.tour_operator_id
                select new PackageInfo
                {
                    PackageId = packages.package_id,
                    TourOperator = touroperators.name,
                    Description = packages.description,
                    StartDate = packages.start_date,
                    EndDate = packages.end_date,
                    Price = packages.price,
                    Country = destinations.country_name,
                    City = destinations.city_name
                };
            return query.ToList();
        }

        public PackageInfo Get(int packageId)
        {
            var package =
                from packages in _dbContext.Packages
                join destinations in _dbContext.Destinations
                on packages.destination_id equals destinations.destination_id
                join touroperators in _dbContext.TourOperators
                on packages.tour_operator_id equals touroperators.tour_operator_id
                where packages.package_id == packageId
                select new PackageInfo
                {
                    PackageId = packages.package_id,
                    TourOperator = touroperators.name,
                    Description = packages.description,
                    StartDate = packages.start_date,
                    EndDate = packages.end_date,
                    Price = packages.price,
                    Country = destinations.country_name,
                    City = destinations.city_name
                };
            return package.FirstOrDefault();
        }
        public void AddPackage(int tourOperatorId, int destId, string name, decimal price, DateTime startDate, DateTime endDate)
        {
            var newpackage = new Model.Packages
            {
                tour_operator_id = tourOperatorId,
                destination_id = destId,
                description = name,
                price = price,
                start_date = startDate,
                end_date = endDate
            };

            _dbContext.Packages.Add(newpackage);
            _dbContext.SaveChanges();
        }

        public void UpdatePackage(int tourNumber, int tourOperatorId, int destId, string name, decimal price, DateTime startDate, DateTime endDate)
        {
            try
            {
                var packageToUpdate = _dbContext.Packages.FirstOrDefault(p => p.package_id == tourNumber);

                if (packageToUpdate != null)
                {
                    packageToUpdate.tour_operator_id = tourOperatorId;
                    packageToUpdate.destination_id = destId;
                    packageToUpdate.description = name;
                    packageToUpdate.price = price;
                    packageToUpdate.start_date = startDate;
                    packageToUpdate.end_date = endDate;

                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception($"Тур с номером {tourNumber} не найден");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении данных тура: {ex.Message}");
            }
        }

        public void DeletePackage(int packageId)
        {
            var packageToDelete = _dbContext.Packages.FirstOrDefault(p => p.package_id == packageId);

            if (packageToDelete != null)
            {
                _dbContext.Packages.Remove(packageToDelete);
                _dbContext.SaveChanges();
            }
        }
    }
}
