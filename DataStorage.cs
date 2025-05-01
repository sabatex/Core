using Sabatex.ObjectsExchange.ApiAdapter;

namespace Sabatex.ObjectExchange.ClientDataAdapter.Memory
{
    public class DataStorage : IClientExchangeDataAdapter
    {
        static List<UploadObject> _uploadData = new List<UploadObject>();
        static List< UnresolvedObject> _unresolvedData = new List<UnresolvedObject>();
        static Dictionary<Guid, string > _unresolvedDataStatus = new Dictionary<Guid, string>();
        public async Task<Guid> RegisterMessageAsync(Guid destination, string messageHeader, string message)
        {
            var data = new UploadObject
            {
                Id = Guid.NewGuid(),
                DateStamp = DateTime.UtcNow,
                Message = message,
                MessageHeader = messageHeader,
                NodeId = destination
            };
            _uploadData.Add(data);
            await Task.Yield();
            return data.Id;
        }

        public async Task RemoveUploadMessageAsync(Guid destination, Guid id)
        {
            var data = _uploadData.Single(s => s.Id == id && s.NodeId == destination);
            _uploadData.Remove(data);
            await Task.Yield();
        }
        public async Task RegisterUnresolvedMessageAsync(Guid destination, UnresolvedObject unresolvedObject)
        {
            _unresolvedData.Add(unresolvedObject);
            await Task.Yield();
        }

        public async Task RegisterUnresolvedMessageStatusAsync(Guid id, string status)
        {
            if (_unresolvedDataStatus.ContainsKey(id))
                _unresolvedDataStatus[id] = status;
            else
                _unresolvedDataStatus.Add(id, status);
            await Task.Yield();
        }
        public async Task RemoveUnresolvedMessageAsync(Guid destination, Guid id)
        {
            var data = _unresolvedData.Single(s => s.Id == id && s.NodeId == destination);
            _unresolvedData.Remove(data);
            _unresolvedDataStatus.Remove(id);
            await Task.Yield();
        }

        public async Task<IEnumerable<UploadObject>> GetUploadObjectsAsync(Guid destinationNode, int take = 10)
        {
            await Task.Yield();
            return _uploadData.Where(s=>s.NodeId == destinationNode).OrderBy(o=>o.Id).Take(take).ToArray();
        }
    }
}
