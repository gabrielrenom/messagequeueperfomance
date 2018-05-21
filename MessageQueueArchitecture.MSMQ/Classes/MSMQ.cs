using MessageQueueArchitecture.Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.MSMQ.Classes
{
    public class MSMQ<T> : IClient<T>, IServer<T> where T : class
    {
        private string queueName = @".\Private$\DefaultQueue";
        private IMessageLoader _loader;
        private readonly MessageQueue sendQueue;
        private readonly MessageQueue receiveQueue;

        public MSMQ(IMessageLoader loader)
        {
            _loader = loader;

            if (!MessageQueue.Exists(queueName))
            {
                MessageQueue.Create(queueName);
            }
            var formatter = new XmlMessageFormatter(new Type[] { typeof(T) });

            sendQueue = new MessageQueue(queueName) { Formatter = formatter };
            receiveQueue = new MessageQueue(queueName) { Formatter = formatter };
        }

        public long TotalMessages { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task Receive(Action<T> receiver)
        {
            receiveQueue.ReceiveCompleted += (sender, args) =>
            {
                try
                {
                    var result = args.Message.Body as T;
                    receiver(result);
                    receiveQueue.BeginReceive();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            };
            receiveQueue.BeginReceive();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public async Task Send(T message)
        {
            var envelope = new Message(message) { Recoverable = false };
            sendQueue.Send(envelope);
        }
    }
}
