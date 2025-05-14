

using Microsoft.EntityFrameworkCore;
using Sabatex.ObjectExchange.Core;
using System.Runtime.CompilerServices;

namespace Sabatex.ObjectExchange.ClientDataAdapter.Memory
{
    public class DataStorage : IClientExchangeDataAdapter
    {
        readonly MemoryDbContext _memoryDbContext;
        public DataStorage(string memoryBaseName="MemoryDatabase")
        {
            var optionsBuilder = new DbContextOptionsBuilder<ObjectExchange.ClientDataAdapter.Memory.MemoryDbContext>().UseInMemoryDatabase(memoryBaseName).Options;
            _memoryDbContext = new MemoryDbContext(optionsBuilder);
        }
 
        public async Task<Guid> RegisterUploadMessageAsync(Guid destination, string messageHeader, string message)
        {



            var data = new UploadObject
            {
                DateStamp = DateTime.UtcNow,
                Message = message,
                MessageHeader = messageHeader,
                NodeId = destination
            };
            await _memoryDbContext.AddAsync(data);
            await _memoryDbContext.SaveChangesAsync();
            return data.Id;
        }

        public async Task RemoveUploadMessageAsync(Guid destination, Guid id)
        {
            var data = await _memoryDbContext.UploadObjects.SingleAsync(s => s.Id == id && s.NodeId == destination);
            _memoryDbContext.Remove(data);
            await _memoryDbContext.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<UploadObject>> GetUploadMessagesAsync(Guid destinationNode, int take = 10)
        {
            return await _memoryDbContext.UploadObjects.Where(s => s.NodeId == destinationNode).OrderBy(o => o.Id).Take(take).ToArrayAsync();
        }
        
        public async Task RegisterUnresolvedMessageAsync(Guid destination, UnresolvedObject unresolvedObject)
        {
            unresolvedObject.DateStamp = DateTime.UtcNow;
            await _memoryDbContext.AddAsync(unresolvedObject);
            await _memoryDbContext.SaveChangesAsync();
        }

        public async Task RegisterUnresolvedMessageStatusAsync(Guid destination, Guid id, string state)
        {
            var data = _memoryDbContext.UnresolvedObjects.Single(s => s.Id == id && s.NodeId == destination);
            data.State = state;
            data.LiveLevel++;
            await _memoryDbContext.SaveChangesAsync();
        }

        public async Task RemoveUnresolvedMessageAsync(Guid destination, Guid id)
        {
            var data = _memoryDbContext.UnresolvedObjects.Single(s => s.Id == id && s.NodeId == destination);
            _memoryDbContext.Remove(data);
            await _memoryDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UnresolvedObject>> GetUnresolvedMessagesAsync(Guid destinationNode, int take = 10)
        {
            return await _memoryDbContext.UnresolvedObjects.Where(s=>s.NodeId == destinationNode).OrderBy(o=>o.Id).Take(take).ToArrayAsync();
        }

        public async Task<IEnumerable<ExchangeNode>> GetExchangeNodesAsync()
        {
            return await _memoryDbContext.ExchangeNodes.Where(s=>s.IsActive).ToArrayAsync();
        }

        public IClientExchangeDataAdapter Initial(IEnumerable<ExchangeNode> exchangeNodes)
        {
            foreach (var node in exchangeNodes)
            {
                _memoryDbContext.ExchangeNodes.Add(node);
            }
            _memoryDbContext.SaveChanges();
            return this;
        }

        Task IClientExchangeDataAdapter.RegisterUploadMessageAsync(Guid destination, string messageHeader, string message)
        {
            return RegisterUploadMessageAsync(destination, messageHeader, message);
        }

    }
}
