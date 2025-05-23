using Microsoft.EntityFrameworkCore;
using lab9.Models;

namespace lab9.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        public DbSet<Class> Classes { get; set; }
    }
}
