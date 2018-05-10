using MessageQueueArchitecture.Commons.Interfaces;
using MessageQueueArchitecture.Commons.Messages;
using MessageQueueArchitecture.MSMQ.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.MSMQ
{
    class Program
    {
        private static int INSTANCES = 10;
        private static string LOCAL_QUEUE = ".\\DefaultPublicQueue";
        private static string REMOTE_QUEUE = @"FormatName:DIRECT=TCP:51.144.79.189\Private$\DefaultQueue"; //"51.144.79.189\\Private$\\DefaultPublicQueue";
        static void Main(string[] args)
        {
            //IClient client = new Client(@".\Private$\DefaultQueue");
            //IClient client = new Client(".\\DefaultPublicQueue");
            IClient client = new Client(REMOTE_QUEUE);
            //IServer server = new Server(@".\Private$\DefaultQueue", MessageLoader.GetClaim(),10);
            IServer server = new Server(REMOTE_QUEUE, MessageLoader.GetClaim(), 10);
            server.Run();
            client.Run();
            server.Run();

            Console.WriteLine("Ready To test Message Queue");

            for (int i = 0; i < INSTANCES; i++)
            {
                var serverthread = new Thread(server.Run);                

                serverthread.Name = $"SERVER:{i}";                

                serverthread.Start();                   
            }

            System.Threading.Thread.Sleep(1000);

            for (int i = 0; i < INSTANCES; i++)
            {                
                var clientthread = new Thread(client.Run);
             
                clientthread.Name = $"CLIENT:{i}";

                clientthread.Start();
            }

            Console.ReadKey();       
        }
    }
}
