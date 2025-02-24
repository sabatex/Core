using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.ApiAdapter;
public interface IExchangeDbContext
{
    public DbSet<UnresolvedObject> UnresolvedObjects { get; set; }
}
public class ExchangeDbContext : DbContext, IExchangeDbContext,IDataBaseAdapter
{
    public DbSet<UnresolvedObject> UnresolvedObjects { get; set; }

    public async Task<string> DownloadObjectsAsync(ExchangeNode exchangeNode)
    {



        throw new NotImplementedException();
    }
}
