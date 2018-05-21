using MessageQueueArchitecture.Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.Commons.Messages
{
    public class PerfomanceTest
    {
        private readonly IServer<string> messageReceiver;
        private readonly IClient<string> messageSender;

        public PerfomanceTest(IServer<string> messageReceiver, IClient<string> messageSender)
        {
            this.messageReceiver = messageReceiver;
            this.messageSender = messageSender;
        }

        public void Run(int messageSizeInBytes, long numberOfMessages)
        {
            var receivingMachine = new MessageQueueArchitecture.Commons.Classes.MessageReceivingMachine(messageReceiver, numberOfMessages);
            var sendingMachine = new MessageQueueArchitecture.Commons.Classes.MessageSendingMachine(messageSender);
            

            sendingMachine.RunTest(messageSizeInBytes, numberOfMessages);

            receivingMachine.WaitForCompletion();
        }
    }
}
