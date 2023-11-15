using Receiver.Services;

namespace Receiver
{
     public class Startup
     {
          public void ConfigureServices(IServiceCollection services)
          {
               services.AddGrpc();
          }

          public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
          {
               app.UseRouting();
               app.UseEndpoints(endpoints =>
               {
                    endpoints.MapGrpcService<NotificationService>();
                    endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

               });
          }
     }
}
