using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using kp.View;

namespace kp.Model
{
    internal class AppContext : DbContext
    {
        public AppContext() : base("DB_KPEntities") { }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Destinations> Destinations { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<TourOperators> TourOperators { get; set; }
        public DbSet<Users> Users { get; set; }

       
    }
}
