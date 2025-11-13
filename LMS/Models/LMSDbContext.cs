using Microsoft.EntityFrameworkCore;

namespace LMS.Models
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions<LMSDbContext> options) : base (options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
    }
}
