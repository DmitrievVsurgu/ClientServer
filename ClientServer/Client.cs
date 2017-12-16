using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZeroMQ;

namespace ClientServer
{
    class Client
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.WriteLine();
                Console.WriteLine(" Welcome to our chat ()");
                Console.WriteLine();
                args = new string[] { "tcp://127.0.0.1:5570" };
            }

            string endpoint = args[0];

            string UserName;
            Console.Write("Enter your name");
            UserName = Console.ReadLine();

            // Create
            using (var context = new ZContext())
            using (var requester = new ZSocket(context, ZSocketType.REQ))
            {
                // Connect
                requester.Connect(endpoint);

                for (int n = 0; n < 10; ++n)
                {
                    string requestText;
                    Console.Write("Enter your message: ");
                    requestText = Console.ReadLine();
                    Console.WriteLine();
                    Console.Write("Sending {0}: {1}...", UserName, requestText);

                    // Send
                    requester.Send(new ZFrame(requestText));

                    // Receive
                    using (ZFrame reply = requester.ReceiveFrame())
                    {
                        Console.WriteLine(" Received: {0} {1}!", requestText, reply.ReadString());
                    }
                }
            }
        }
    }
}
