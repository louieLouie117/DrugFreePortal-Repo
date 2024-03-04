using Microsoft.EntityFrameworkCore;

namespace DrugFreePortal.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<User>? Users { get; set; }
    }
}