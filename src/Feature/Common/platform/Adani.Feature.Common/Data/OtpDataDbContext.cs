using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Adani.Feature.Common.Data
{
    public partial class OtpDataDbContext : DbContext
    {
        public OtpDataDbContext()
            : base("name=OtpDataConnectionString")
        {
        }

        public virtual DbSet<OtpData> OtpDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
