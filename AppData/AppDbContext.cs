using Microsoft.EntityFrameworkCore;
using SixMinApi.Models;

namespace SixMinApi.AppData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<DbContext> options) : base(options)
        {
            
        }
        public DbSet<Command> Commands => Set<Command>();

    }
}
