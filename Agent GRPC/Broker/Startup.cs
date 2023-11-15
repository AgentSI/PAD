using Broker.Services.Interfaces;
using Broker.Services;

namespace Broker
{
     public class Startup
     {
          public void ConfigureServices(IServiceCollection services)
          {
               services.AddGrpc();
               services.AddSingleton<IMessageStorage, MessageStorage>();
               services.AddSingleton<IConnectionStorage, ConnectionStorage>();
               services.AddHostedService<SenderWorker>();
          }

          public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
          {
               app.UseRouting();
               app.UseEndpoints(endpoints =>
               {
                    endpoints.MapGrpcService<PublisherService>();
                    endpoints.MapGrpcService<SubscriberService>();
                    endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

               });
          }
     }
}
