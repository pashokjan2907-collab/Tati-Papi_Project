using Microsoft.EntityFrameworkCore;
using TatiPapi.Api.Models;

namespace TatiPapi.Api.Data
{
    public class AppDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=tatipapi.db");
        }
    }
}