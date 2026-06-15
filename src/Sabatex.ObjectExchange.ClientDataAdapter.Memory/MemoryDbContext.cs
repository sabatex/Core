using Microsoft.EntityFrameworkCore;
using Sabatex.ObjectExchange.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.ClientDataAdapter.Memory
{
    public class MemoryDbContext : DbContext, ISabatexObjectExchangeDbContext
    {
        public DbSet<UploadObject> UploadObjects { get; set; }
        public DbSet<UnresolvedObject> UnresolvedObjects { get; set;}
        public DbSet<ExchangeNode> ExchangeNodes { get; set; }
        public MemoryDbContext(DbContextOptions options):base(options)
        {
        }
    }
}
