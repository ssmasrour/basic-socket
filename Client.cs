using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        public const int PortNumber = 11111;

        static void Main(string[] args)
        {
            ExecuteClient();
        }

        private static void ExecuteClient()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = hostEntry.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 11111);

            Socket clientSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                clientSocket.Connect(endPoint);
                // send ======================
                byte[] message = Encoding.ASCII.GetBytes("Hello networking world");

                var sizeSent = clientSocket.Send(message);

                // received
                byte[] byteReceived = new byte[1024];
                var sizeReceived = clientSocket.Receive(byteReceived);

                Console.WriteLine("Received: {0}", Encoding.ASCII.GetString(byteReceived, 0, sizeReceived));

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
