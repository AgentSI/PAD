using System.Net;
using System.Net.Sockets;

namespace Publisher
{
    class PublisherSocket
    {
        private Socket? _socket;
        public bool IsConnected;

        public PublisherSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectedCallback, null);
            Thread.Sleep(2000);
        }

        private void ConnectedCallback(IAsyncResult asyncResult)
        {
            if (_socket.Connected)
            {
                Console.WriteLine("Publisher connected to Brocker");
            }
            else
            {
                Console.WriteLine("Error: Publisher not connected to Brocker");
            }

            IsConnected = _socket.Connected;
        }

        public void Send(byte[] data)
        {
            try
            {
                _socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not send data: {e.Message}");
            }
        }
    }
}
