using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patchTuesVisualStudioToXML.Data
{
    public class OVALXMLsContext : DbContext
    {
        public DbSet<OVALXMLs> OVALXMLs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString:
               "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=postgres;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OVALXMLs>()
                .Property(XML => XML.CreationDate)
                .HasDefaultValue(DateTime.Now);
            base.OnModelCreating(modelBuilder);
        }
    }


    [Table("OVALXMLs")]
    public class OVALXMLs : DbContext
    {
        [Column("id", TypeName = "bigserial")]
        [System.ComponentModel.DataAnnotations.Key]
        public long Id { get; internal set; }

        [Column("data", TypeName = "bytea")]
        public byte[] Data { get; internal set; }

        [Column("creation_date", TypeName = "timestamp")]
        public DateTime CreationDate { get; }

    }

}
