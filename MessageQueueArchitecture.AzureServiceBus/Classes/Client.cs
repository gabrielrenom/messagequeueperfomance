using MessageQueueArchitecture.Commons.Interfaces;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;


namespace MessageQueueArchitecture.AzureServiceBus.Classes
{
    public class Client : IClient
    {
        private string _connectionstring;
        private string _queue;
        private QueueClient queueClient;

        public Client(string connectionstring, string queue)
        {
            _connectionstring = connectionstring;
            _queue = queue;
        }

        public void Run()
        {
            
        }

      
    }
}
