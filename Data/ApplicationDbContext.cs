using Microsoft.EntityFrameworkCore;
using CRUDwithIdentity.Models.Entities;

namespace CRUDwithIdentity.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<BookEntity> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection"); // Referencing the connection string in Web.config (or appsettings.json)
            }
        }
    }
}
