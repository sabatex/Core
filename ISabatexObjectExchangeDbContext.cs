using Microsoft.EntityFrameworkCore;
using Sabatex.ObjectExchange.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.ClientDataAdapter
{
    public interface ISabatexObjectExchangeDbContext
    {
        DbSet<UploadObject> UploadObjects { get; set; }
        DbSet<UnresolvedObject> UnresolvedObjects { get; set; }
        DbSet<ExchangeNode> ExchangeNodes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        void SabatexObjectExchangeModelCreating(ModelBuilder builder)
        {
        }
    }
}
