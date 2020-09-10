using Microsoft.EntityFrameworkCore;

namespace raspWeb.Models
{
    public class AppDbCont : DbContext
    {
        public DbSet<EveryPara> Paras { get; set; }

        public AppDbCont(DbContextOptions<AppDbCont> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
