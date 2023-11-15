using Common;

namespace Brocker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Brocker");

            var brockerSocket = new BrockerSocket();
            brockerSocket.Start(Settings.BrockerIp, Settings.BrockerPort);

            var worker = new Worker();
            Task.Run(() => worker.DoSendMessage());

            Console.ReadLine();
        }
    }
}