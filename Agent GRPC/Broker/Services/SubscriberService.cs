using Broker.Models;
using Broker.Services.Interfaces;
using Grpc.Core;
using GrpcAgent;

namespace Broker.Services
{
    public class SubscriberService : Subscriber.SubscriberBase
    {
        private readonly IConnectionStorage _connectionStorage;

        public SubscriberService(IConnectionStorage connectionStorage)
        {
            _connectionStorage = connectionStorage;
        }
        public override Task<SubscribeReply> Subscribe(SubscribeRequest request, ServerCallContext context)
        {
            Console.WriteLine($"New client trying to subscribe: {request.Address} {request.Topic}");

            try
            {
                var connection = new Connection(request.Address, request.Topic);
                _connectionStorage.Add(connection);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not add the new connection: {e.Message}");
            }

            return Task.FromResult(new SubscribeReply()
            {
                IsSuccess = true
            });
        }
    }
}
