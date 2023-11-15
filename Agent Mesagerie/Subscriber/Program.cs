using Common;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Subscriber");

            string? topic;

            while (true)
            {
                Console.Write("Enter the topic: ");
                topic = Console.ReadLine()?.ToLower();

                if (!string.IsNullOrWhiteSpace(topic))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Topic is required. Please enter a valid topic.");
                }
            }

            var subscriberSocket = new SubscriberSocket(topic);
            subscriberSocket.Connect(Settings.BrockerIp, Settings.BrockerPort);

            Console.ReadLine();
        }
    }
}