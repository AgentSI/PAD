using Common;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace Brocker
{
    class Worker
    {
        private const int TimeToSleep = 500;

        public void DoSendMessage()
        {
            while(true)
            {
                while (!PayloadStorage.IsEmpty())
                {
                    var payload = PayloadStorage.GetNext();

                    if (payload != null)
                    {
                        var connections = ConnectionsStorage.GetConnectionsByTopic(payload.Topic);

                        foreach(var connection in connections)
                        {
                            var payloadString = JsonConvert.SerializeObject(payload);
                            byte[] data = Encoding.UTF8.GetBytes(payloadString);
                            PayloadStorage.SavePayloadToFile(payloadString);
                            SocketError sendResult = SocketError.Success;

                            try
                            {
                                connection.Socket.Send(data, SocketFlags.None, out sendResult);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Error sending message: {e.Message}");
                            }

                            if (sendResult == SocketError.Success)
                            {
                                PayloadStorage.DeletePayloadFromFile(payloadString);
                            }
                        }
                    }
                }

                Thread.Sleep(TimeToSleep);
            }
        }
    }
}
