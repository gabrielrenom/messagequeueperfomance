using MessageQueueArchitecture.AzureServiceBus.Classes;
using MessageQueueArchitecture.Commons.Interfaces;
using MessageQueueArchitecture.Commons.Messages;
using Microsoft.Azure.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.AzureServiceBus
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    IMessageLoader _loader = new MessageLoader();

        //    const long numberOfMessages = 500;

        //    var azureServiceBus = new AzureServiceBus<string>(_loader);

        //    azureServiceBus.TotalMessages = numberOfMessages;

        //    new PerfomanceTest(azureServiceBus, azureServiceBus).Run(5000, numberOfMessages);

        //    Console.ReadKey();
        //}

        static void Main(string[] args)
        {

            //TestA a = new TestA();
            //a.Run().Wait();

            //TestB b = new TestB();
            //b.Run().Wait();

            //TestC c = new TestC();
            //c.Run().Wait();

            TestD d = new TestD();
            d.Run().Wait();

            Console.ReadKey();


        }

        /// <summary>
        /// Safe multithreading
        /// </summary>
        public class TestD
        {
            private MessagingFactory factory;
            private MessageSender messageSender;
            private const long numberOfMessages = 500;
            private IMessageLoader _loader;
            private readonly Stopwatch stopwatch = new Stopwatch();
            private readonly string queuename = "test";
            private MessageReceiver messageReceiver;

            public TestD()
            {
                _loader = new MessageLoader();
                factory = MessagingFactory.CreateFromConnectionString(@"Endpoint=sb://fcgtest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Bvts8dhzcb7mbKj4PURSD6UPf3eO7hfV7qDWlC4MWyA=");
            }

            public async Task Run()
            {
                await SendMessages();
                await ReceiveMessages();
            }

            public async Task ReceiveMessages()
            {
                messageReceiver = factory.CreateMessageReceiver(queuename);
                stopwatch.Restart();
                stopwatch.Start();

                for (int i = 0; i < numberOfMessages; i++)
                {
                    Task.Factory.StartNew(ReceiveMessage);
                }
          
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("{0} messages received in {1} ms", numberOfMessages, elapsedMilliseconds);
                Console.WriteLine("Received {0} per second", numberOfMessages == 0 ? 0L : (1000 * (long)numberOfMessages) / (long)elapsedMilliseconds);

            }

            public async Task ReceiveMessage()
            {
                var message = await messageReceiver.ReceiveAsync();
                await messageReceiver.CompleteAsync(message.LockToken);
            }

            public async Task SendMessages()
            {
                messageSender = factory.CreateMessageSender(queuename);

                stopwatch.Start();

                for (int i = 0; i < numberOfMessages; i++)
                {
                   Task.Factory.StartNew(MessageToBeSend);
                }

                stopwatch.Stop();

                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("Sent {0} messages in {1} ms", numberOfMessages, elapsedMilliseconds);
                Console.WriteLine("{0} per second", elapsedMilliseconds == 0 ? 0L : (1000 * numberOfMessages) / elapsedMilliseconds);

                Console.WriteLine("Sleeping for 10 seconds before receiving...");
                Thread.Sleep(20000);
            }

            public async Task MessageToBeSend()
            {
                string message = _loader.GetClaim();
                await messageSender.SendAsync(new BrokeredMessage(message));
            }
        }

        /// <summary>
        /// Safe multithreading
        /// </summary>
        public class TestC
        {
            private MessagingFactory factory;
            private MessageSender messageSender;
            private const long numberOfMessages = 50000;
            private IMessageLoader _loader;
            private readonly Stopwatch stopwatch = new Stopwatch();
            private readonly string queuename = "test";
            private MessageReceiver messageReceiver;

            public TestC()
            {
                _loader = new MessageLoader();
                factory = MessagingFactory.CreateFromConnectionString(@"Endpoint=sb://fcgtest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Bvts8dhzcb7mbKj4PURSD6UPf3eO7hfV7qDWlC4MWyA=");
            }

            public async Task Run()
            {
                await SendMessages();
                await ReceiveMessages();
            }

            public async Task ReceiveMessages()
            {
                List<Task> tasks = new List<Task>();
                messageReceiver = factory.CreateMessageReceiver(queuename);
                stopwatch.Restart();
                stopwatch.Start();

                for (int i = 0; i < numberOfMessages; i++)
                {
                    tasks.Add(Task.Factory.StartNew(ReceiveMessage));
                }

                Task.WaitAll(tasks.ToArray());
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("{0} messages received in {1} ms", numberOfMessages, elapsedMilliseconds);
                Console.WriteLine("Received {0} per second", numberOfMessages == 0 ? 0L : (1000 * (long)numberOfMessages) / (long)elapsedMilliseconds);

            }

            public async Task ReceiveMessage()
            {
                var message = await messageReceiver.ReceiveAsync();
                await messageReceiver.CompleteAsync(message.LockToken);
            }

            public async Task SendMessages()
            {
                List<Task> tasks = new List<Task>();
                messageSender = factory.CreateMessageSender(queuename);

                stopwatch.Start();

                for (int i = 0; i < numberOfMessages; i++)
                {
                    tasks.Add(Task.Factory.StartNew(MessageToBeSend));
                }

                Task.WaitAll(tasks.ToArray());

                stopwatch.Stop();

                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("Sent {0} messages in {1} ms", numberOfMessages, elapsedMilliseconds);
                Console.WriteLine("{0} per second", elapsedMilliseconds == 0 ? 0L : (1000 * numberOfMessages) / elapsedMilliseconds);

                Console.WriteLine("Sleeping for 10 seconds before receiving...");
                Thread.Sleep(20000);
            }

            public async Task MessageToBeSend()
            {
                string message = _loader.GetClaim();
                await messageSender.SendAsync(new BrokeredMessage(message));
            }
        }

        /// <summary>
        /// Async simple
        /// </summary>
        public class TestB
        {
            private MessagingFactory factory;
            private MessageSender messageSender;
            private const long numberOfMessages = 500;
            private IMessageLoader _loader;
            private readonly Stopwatch stopwatch = new Stopwatch();
            private readonly string queuename = "test";
            private MessageReceiver messageReceiver;

            public TestB()
            {
                _loader = new MessageLoader();
                factory = MessagingFactory.CreateFromConnectionString(@"Endpoint=sb://fcgtest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Bvts8dhzcb7mbKj4PURSD6UPf3eO7hfV7qDWlC4MWyA=");
            }

            public async Task Run()
            {
                await SendMessages();
                await ReceiveMessages();
            }

            public async Task ReceiveMessages()
            {
                messageReceiver = factory.CreateMessageReceiver(queuename);
                stopwatch.Restart();
                stopwatch.Start();

                for (int i = 0; i < numberOfMessages; i++)
                {
                    var message = await messageReceiver.ReceiveAsync();
                    await messageReceiver.CompleteAsync(message.LockToken);
                }

                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("{0} messages received in {1} ms", numberOfMessages, elapsedMilliseconds);
                Console.WriteLine("Received {0} per second", numberOfMessages == 0 ? 0L : (1000 * (long)numberOfMessages) / (long)elapsedMilliseconds);

            }

            public async Task SendMessages()
            {
                messageSender = factory.CreateMessageSender(queuename);

                stopwatch.Start();

                for (int i = 0; i < numberOfMessages; i++)
                {
                    string message = _loader.GetClaim();
                    await messageSender.SendAsync(new BrokeredMessage(message));
                }

                
                stopwatch.Stop();

                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("Sent {0} messages in {1} ms", numberOfMessages, elapsedMilliseconds);
                Console.WriteLine("{0} per second", elapsedMilliseconds == 0 ? 0L : (1000 * numberOfMessages) / elapsedMilliseconds);

                Console.WriteLine("Sleeping for 10 seconds before receiving...");
                Thread.Sleep(20000);
            }

        }

        /// <summary>
        /// Safe multithreading
        /// </summary>
        public class TestA
        {
            private MessagingFactory factory;
            private MessageSender messageSender;
            private const long numberOfMessages = 500;
            private IMessageLoader _loader;
            private readonly Stopwatch stopwatch = new Stopwatch();
            private readonly string queuename = "test";
            private MessageReceiver messageReceiver;

            public TestA()
            {
                _loader = new MessageLoader();
                factory = MessagingFactory.CreateFromConnectionString(@"Endpoint=sb://fcgtest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Bvts8dhzcb7mbKj4PURSD6UPf3eO7hfV7qDWlC4MWyA=");
            }

            public async Task Run()
            {
                await SendMessages();
                await ReceiveMessages();
            }

            public async Task ReceiveMessages()
            {
                List<Task> tasks = new List<Task>();
                messageReceiver = factory.CreateMessageReceiver(queuename);
                stopwatch.Restart();
                stopwatch.Start();

                for (int i = 0; i < numberOfMessages; i++)
                {
                    tasks.Add(Task.Factory.StartNew(ReceiveMessage));
                }

                Task.WaitAll(tasks.ToArray());
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("{0} messages received in {1} ms", numberOfMessages, elapsedMilliseconds);
                Console.WriteLine("Received {0} per second", numberOfMessages == 0 ? 0L : (1000 * (long)numberOfMessages) / (long)elapsedMilliseconds);

            }

            public async Task ReceiveMessage()
            {
                var message = await messageReceiver.ReceiveAsync();
                await messageReceiver.CompleteAsync(message.LockToken);
            }

            public async Task SendMessages()
            {
                List<Task> tasks = new List<Task>();
                messageSender = factory.CreateMessageSender(queuename);

                stopwatch.Start();

                for (int i = 0; i < numberOfMessages; i++)
                {
                    tasks.Add(Task.Factory.StartNew(MessageToBeSend));
                }

                Task.WaitAll(tasks.ToArray());

                stopwatch.Stop();

                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("Sent {0} messages in {1} ms", numberOfMessages, elapsedMilliseconds);
                Console.WriteLine("{0} per second", elapsedMilliseconds == 0 ? 0L : (1000 * numberOfMessages) / elapsedMilliseconds);

                Console.WriteLine("Sleeping for 10 seconds before receiving...");
                Thread.Sleep(20000);
            }

            public async Task MessageToBeSend()
            {
                string message = _loader.GetClaim();
                await messageSender.SendAsync(new BrokeredMessage(message));
            }
        }
       
    }
 
    }

