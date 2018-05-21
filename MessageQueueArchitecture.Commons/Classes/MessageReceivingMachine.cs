using MessageQueueArchitecture.Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.Commons.Classes
{
    public class MessageReceivingMachine
    {
        private readonly IServer<string> messageReceiver;
        private readonly long numberOfMessages;

        public MessageReceivingMachine(IServer<string> messageReceiver, long numberOfMessages)
        {
            this.messageReceiver = messageReceiver;
            this.messageReceiver.Receive(ReceiveMessage);
            this.numberOfMessages = numberOfMessages;
        }

        private bool started = false;
        private bool completed = false;
        private long messageCounter = 0;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly object theLock = new object();

        private void ReceiveMessage(string message)
        {
            if (!started)
            {
                lock (theLock)
                {
                    if (!started)
                    {
                        started = true;
                        stopwatch.Start();
                    }
                }
            }

            Interlocked.Increment(ref messageCounter);

            if (messageCounter == numberOfMessages)
            {
                lock (theLock)
                {
                    stopwatch.Stop();
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine("{0} messages received in {1} ms", numberOfMessages, elapsedMilliseconds);
                    Console.WriteLine("Received {0} per second", elapsedMilliseconds == 0 ? 0L : (1000 * numberOfMessages) / elapsedMilliseconds);
                    completed = true;
                }
            }
        }

        public void WaitForCompletion()
        {
            while (NotCompleted())
            {
                Thread.Sleep(100);
            }
        }

        private bool NotCompleted()
        {
            lock (theLock)
            {
                return !completed;
            }
        }
    }
}
