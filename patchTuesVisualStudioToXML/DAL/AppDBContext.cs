using Microsoft.EntityFrameworkCore;
using patchTuesVisualStudioToXML.DAL.Entities;


namespace patchTuesVisualStudioToXML.DAL
{
    public class AppDBContext : DbContext
    {
        public DbSet<OVALXML> OVALXMLs => Set<OVALXML>();
        public AppDBContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Username=postgres;Password=12345;Database=postgres;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
