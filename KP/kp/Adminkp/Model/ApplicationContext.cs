using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adminkp.Model
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DB_KPEntAdm") { }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Destinations> Destinations { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<TourOperators> TourOperators { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
