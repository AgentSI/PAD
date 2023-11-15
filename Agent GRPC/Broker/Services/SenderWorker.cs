using Broker.Services.Interfaces;
using Grpc.Core;
using GrpcAgent;

namespace Broker.Services
{
    public class SenderWorker : IHostedService
    {
        private Timer _timer;
        private const int TimeToWait = 90000;
        private readonly IMessageStorage _messageStorage;
        private readonly IConnectionStorage _connectionStorage;

        public SenderWorker(IServiceScopeFactory serviceScopeFactory)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                _messageStorage = scope.ServiceProvider.GetRequiredService<IMessageStorage>();
                _connectionStorage = scope.ServiceProvider.GetRequiredService<IConnectionStorage>();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoSendWork, null, 0, TimeToWait);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void DoSendWork(object state)
        {
            while (!_messageStorage.IsEmpty())
            {
                var message = _messageStorage.GetNext();

                if (message != null)
                {
                    var connections = _connectionStorage.GetConnectionsByToopic(message.Topic);

                    foreach(var connection in connections)
                    {
                        var client = new Notifier.NotifierClient(connection.Channel);
                        var request = new NotifyRequest() { Content = message.Content };

                        try
                        {
                            var reply = client.Notify(request);
                            Console.WriteLine($"Notified subscriber {connection.Address} with {message.Content}");
                        }
                        catch (RpcException rpce)
                        {
                            if (rpce.StatusCode == StatusCode.Internal)
                            {
                                _connectionStorage.Remove(connection.Address);
                            }

                            Console.WriteLine($"RPC Eror notify subscriber {connection.Address}. {rpce.Message}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error notifying subscriber {connection.Address}. {e.Message}");
                        }
                    }
                }
            }
        }
    }
}
