using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteServer();
        }

        public static void ExecuteServer()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = hostEntry.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 11111);

            Socket hostSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                hostSocket.Bind(endPoint);

                hostSocket.Listen(2);

                while (true)
                {
                    Console.WriteLine("Waiting connection ....");
                    Socket incomingClientSocket = hostSocket.Accept();

                    byte[] buffer = new byte[1024];
                    string decodData = null;

                    while (true)
                    {
                        int receivedSize = incomingClientSocket.Receive(buffer);

                        decodData = Encoding.ASCII.GetString(buffer, 0, receivedSize);

                        if (decodData.IndexOf("world") > -1)
                            break;
                    }

                    Console.WriteLine("HOST > Received message ", decodData);

                    byte[] toClientMessage = Encoding.ASCII.GetBytes("Server tested successfully");

                    incomingClientSocket.Send(toClientMessage);

                    incomingClientSocket.Shutdown(SocketShutdown.Both);
                    incomingClientSocket.Close();
                }
            }
            catch (SocketException ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }
    }
}
