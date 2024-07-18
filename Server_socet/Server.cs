using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main(string[] args)
    {
        // Server settings
        IPAddress ipAddress = IPAddress.Parse("192.168.0.103");
        int port = 11000;
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

        // Create a socket
        Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            // Bind the socket to the local endpoint
            listener.Bind(localEndPoint);
            // Set the socket to listen for incoming connections
            listener.Listen(10);

            Console.WriteLine("Waiting for a connection...");

            // Accept the connection
            Socket handler = listener.Accept();
            string data = null;

            // Receive data from the client
            byte[] bytes = new byte[1024];
            int bytesRec = handler.Receive(bytes);
            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

            // Display the received message
            Console.WriteLine($"At {DateTime.Now.ToShortTimeString()} received from {((IPEndPoint)handler.RemoteEndPoint).Address}: {data}");

            // Send a response to the client
            byte[] msg = Encoding.UTF8.GetBytes("Hello, client!");
            handler.Send(msg);

            // Close the connection
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}