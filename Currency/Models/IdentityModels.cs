using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currency.Models
{
    public class EFDbContext : DbContext
    {
        public EFDbContext() : base("TESTBASE") { }
        public DbSet<Valuta> Valutas { get; set; }
        public DbSet<ValCurs> ValCurs { get; set; }

        public static EFDbContext Create()
        {
            return new EFDbContext();
        }
    }
}
