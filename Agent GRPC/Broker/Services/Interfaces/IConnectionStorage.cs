using Broker.Models;

namespace Broker.Services.Interfaces
{
    public interface IConnectionStorage
    {
        void Add(Connection connection);
        void Remove(string address);
        IList<Connection> GetConnectionsByToopic(string topic);
    }
}
