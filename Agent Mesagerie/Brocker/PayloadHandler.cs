using Common;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace Brocker
{
    class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes, ConnectionInfo connectionInfo, BrockerSocket brockerSocket)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);

            if (payloadString.StartsWith("subscribe"))
            {
                connectionInfo.Topic = payloadString.Split("subscribe").LastOrDefault();

                // adaugam conexiunea in storage
                ConnectionsStorage.Add(connectionInfo);
            }
            else
            {
                PayloadStorage.SavePayloadToFile(payloadString);

                // Verificați dacă există mesaje în coadă pentru topic-ul subscriberului și trimiteți-le
                List<Payload> messagesInQueue = PayloadStorage.GetMessagesInQueue(connectionInfo.Topic);
                foreach (var message in messagesInQueue)
                {
                    payloadString = JsonConvert.SerializeObject(message);
                    byte[] data = Encoding.UTF8.GetBytes(payloadString);

                    SocketError sendResult = SocketError.Success;
                    try
                    {
                        connectionInfo.Socket.Send(data, SocketFlags.None, out sendResult);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error sending message from queue: {e.Message}");
                    }

                    if (sendResult == SocketError.Success)
                    {
                        PayloadStorage.DeletePayloadFromFile(payloadString);
                    }
                }
            }
        }
    }
}
