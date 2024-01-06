using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace HR.Data
{
    //step 3
    public class HRContext:IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _config;

        public HRContext(IConfiguration config)
        {
            _config = config;
        }
        public DbSet<Employee> employee { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<LeaveRequest> leaveRequests { get; set; }
        public DbSet<Position> positions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("HRContext"));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
