using Microsoft.EntityFrameworkCore;
using DonutzStudio.Models;

namespace DonutzStudio.Data
{
    public class DonutzStudioContext : DbContext
    {
        public DonutzStudioContext(DbContextOptions<DonutzStudioContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Lab> Lab { get; set; }
        public DbSet<Booking> Booking { get; set; }
    }
}