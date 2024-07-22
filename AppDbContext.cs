using Microsoft.EntityFrameworkCore;
using ClientManagement.Models;

namespace ClientManagement
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<StockQuote> StockQuotes { get; set; }
        public DbSet<Client> Clients { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
