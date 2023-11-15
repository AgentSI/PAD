using Common;
using Grpc.Net.Client;
using Microsoft.AspNetCore;
using System.Net;

namespace Broker
{
     public class Program
     {
          public static void Main(string[] args)
          {
               AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
               WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseUrls(EndpointsConstants.BrokerAddress)
                    .Build()
                    .Run();
          }
     }
}
