using Common;
using Grpc.Net.Client;
using GrpcAgent;

namespace Sender
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Publisher");

            var channel = GrpcChannel.ForAddress(EndpointsConstants.BrokerAddress);
            var client = new Publisher.PublisherClient(channel);

            while(true)
            {
                Console.Write("Enter the topic: ");
                var topic = Console.ReadLine().ToLower();

                Console.Write("Enter context: ");
                var context = Console.ReadLine();

                var request = new PublishRequest() { Topic = topic, Content = context };

                try
                {
                    var reply = await client.PublishMessageAsync(request);
                    Console.WriteLine($"Publish Reply {reply.IsSuccess}");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Error publishing: {e.Message}");
                }
            }
        }
    }
}