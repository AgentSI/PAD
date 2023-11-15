using Common;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Brocker
{
    class BrockerSocket
    {
        private Socket? _socket;
        private const int ConnectionLimit = 8;
        private BrockerSocket brockerSocket;

        public BrockerSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(string ip, int port)
        {
            try
            {
                _socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                _socket.Listen(ConnectionLimit);
                Accept();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error starting broker: {e.Message}");
            }
        }

        private void Accept()
        {
            _socket.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = new ConnectionInfo();

            try
            {
                connection.Socket = _socket.EndAccept(asyncResult);
                connection.Address = connection.Socket.RemoteEndPoint.ToString();
                connection.Socket.BeginReceive(connection.Data, 0, connection.Data.Length, 
                    SocketFlags.None, ReceiveCallback, connection);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't accept: {e.Message}");
            }
            finally
            {
                Accept();
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = asyncResult.AsyncState as ConnectionInfo;

            try
            {
                Socket senderSocket = connection.Socket;
                SocketError response;
                int bufferSize = senderSocket.EndReceive(asyncResult, out response);

                if (response == SocketError.Success)
                {
                    byte[] payloadBytes = new byte[bufferSize];
                    Array.Copy(connection.Data, payloadBytes, payloadBytes.Length);

                    // Handle payload
                    PayloadHandler.Handle(payloadBytes, connection, brockerSocket);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't receive data: {e.Message}");
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
                    Console.WriteLine($"Error resuming receive: {e.Message}");
                    var address = connection.Socket.RemoteEndPoint.ToString();

                    // stergem din storage
                    ConnectionsStorage.Remove(address);
                    connection.Socket.Close();
                }
            }
        }
    }
}
