using Broker.Models;
using Broker.Services.Interfaces;

namespace Broker.Services
{
    public class ConnectionStorage : IConnectionStorage
    {
        private readonly List<Connection> _connections;
        private readonly object _locker;

        public ConnectionStorage()
        {
            _connections = new List<Connection>();
            _locker = new object();
        }

        public void Add(Connection connection)
        {
            lock(_locker)
            {
                _connections.Add(connection);
            }
        }

        public IList<Connection> GetConnectionsByToopic(string topic)
        {
            lock (_locker)
            {
                var filtredConnections = _connections.Where(x => x.Topic == topic).ToList();
                return filtredConnections;
            }
        }

        public void Remove(string address)
        {
            lock (_locker)
            {
                _connections.RemoveAll(x => x.Address == address);
            }
        }
    }
}
