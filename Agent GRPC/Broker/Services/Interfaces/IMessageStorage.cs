using Broker.Models;

namespace Broker.Services.Interfaces
{
    public interface IMessageStorage
    {
        void Add(Message message);
        Message GetNext();
        bool IsEmpty();
    }
}
