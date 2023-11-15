using Broker.Models;
using Broker.Services.Interfaces;
using Grpc.Core;
using GrpcAgent;

namespace Broker.Services
{
    public class PublisherService : Publisher.PublisherBase
    {
        private readonly IMessageStorage _messageStorage;

        public PublisherService(IMessageStorage messageStorage)
        {
            _messageStorage = messageStorage;
        }

        public override Task<PublishReply> PublishMessage(PublishRequest request, ServerCallContext context)
        {
            Console.WriteLine($"Received: {request.Topic} {request.Content}");

            var message = new Message(request.Topic, request.Content);
            _messageStorage.Add(message);

            return Task.FromResult(new PublishReply()
            {
                IsSuccess = true
            });
        }
    }
}
