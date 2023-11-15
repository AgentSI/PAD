using Common;
using Newtonsoft.Json;
using System.Text;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Publisher");

            var publisherSocket = new PublisherSocket();
            publisherSocket.Connect(Settings.BrockerIp, Settings.BrockerPort);

            if (publisherSocket.IsConnected)
            {
                while (true)
                {
                    var payload = new Payload();
                    Console.Write("Enter the topic: ");
                    payload.Topic = Console.ReadLine().ToLower();

                    if (string.IsNullOrWhiteSpace(payload.Topic))
                    {
                        Console.WriteLine("Topic is required. Please enter a valid topic.");
                        continue;
                    }

                    Console.Write("Enter the message: ");
                    payload.Message = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(payload.Message))
                    {
                        Console.WriteLine("Message is required. Please enter a valid message.");
                        continue;
                    }

                    var payloadString = JsonConvert.SerializeObject(payload);
                    byte[] data = Encoding.UTF8.GetBytes(payloadString);

                    publisherSocket.Send(data);
                }
            }

            Console.ReadLine();
        }
    }
}