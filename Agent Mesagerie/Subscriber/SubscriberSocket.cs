using Common;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Subscriber
{
    class SubscriberSocket
    {
        private Socket _socket;
        private string _topic;
        public SubscriberSocket(string topic)
        {
            _topic = topic;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectedCallback, null);
            Console.WriteLine("Waiting for connection");
        }

        private void ConnectedCallback(IAsyncResult asyncResult)
        {
            if (_socket.Connected)
            {
                Console.WriteLine("Subscriber connected to Brocker");
                Subscribe();
                StartReceive();
            }
            else
            {
                Console.WriteLine("Error: Subscriber not connected to Brocker");
            }
        }

        private void Subscribe()
        {
            var data = Encoding.UTF8.GetBytes("subscribe" + _topic);
            Send(data);
        }

        private void StartReceive()
        {
            ConnectionInfo connection = new ConnectionInfo();
            connection.Socket = _socket;

            _socket.BeginReceive(connection.Data, 0, connection.Data.Length,
                    SocketFlags.None, ReceiveCallback, connection);
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = (ConnectionInfo)asyncResult.AsyncState;

            try
            {
                SocketError response;
                int bufferSize = _socket.EndReceive(asyncResult, out response);

                if (response == SocketError.Success)
                {
                    byte[] payloadBytes = new byte[bufferSize];
                    Array.Copy(connection.Data, payloadBytes, payloadBytes.Length);

                    // Handle payload
                    PayloadHandler.Handle(payloadBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't receive data from Brocker: {e.Message}");
            }
            finally
            {
                try
                {
                    connection.Socket.BeginReceive(connection.Data, 0, connection.Data.Length,
                        SocketFlags.None, ReceiveCallback, connection);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error loss of connection: {e.Message}");
                    connection.Socket.Close();
                }
            }
        }

        private void Send(byte[] data)
        {
            try
            {
                _socket.Send(data);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: Could not send topic: {e.Message}");
            }
        }
    }
}
