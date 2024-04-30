using EndorsementRejection.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EndorsementRejection.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EndoRejection> EndoRejections { get; set; }
    }   
    
    
}
