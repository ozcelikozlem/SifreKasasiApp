using Microsoft.EntityFrameworkCore;
using SifreKasasi.API.Models;

namespace SifreKasasi.API.Data
{
    public class DatabaseContext:DbContext
    {
        public DbSet<Kayit> Kayits { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
    }
}
