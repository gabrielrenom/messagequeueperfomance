using MessageQueueArchitecture.Commons.Interfaces;
using MessageQueueArchitecture.Commons.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.Commons.Classes
{
    public class MessageSendingMachine
    {
        private readonly IClient<string> messageSender;
        private const long reportingInterval = 1000000;
        private IMessageLoader messageloader;

        public MessageSendingMachine(IClient<string> messageSender)
        {
            this.messageSender = messageSender;
            messageloader = new MessageLoader();
        }

        public void RunTest(int messageSize, long numberToSend)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (long i = 0; i < numberToSend; i++)
            {
                if (i % reportingInterval == 0)
                {
                    Console.WriteLine("Sent {0}", i);
                }

                messageSender.Send(messageloader.GetClaim());
            }

            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("Sent {0} messages in {1} ms", numberToSend, elapsedMilliseconds);
            Console.WriteLine("{0} per second", elapsedMilliseconds == 0 ? 0L : (1000 * numberToSend) / elapsedMilliseconds);
        }
    }
}
