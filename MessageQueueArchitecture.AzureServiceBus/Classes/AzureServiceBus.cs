using MessageQueueArchitecture.Commons.Interfaces;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.AzureServiceBus.Classes
{
    public class AzureServiceBus<T> : IClient<T>, IServer<T> where T : class
    {
        private string connectionString = @"Endpoint=sb://fcgpremiun.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZdLOppDD7UNjgdukRLFaRpHMabo4hPU/JLwUsGZWKys=";//@"Endpoint=sb://fcgtest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Bvts8dhzcb7mbKj4PURSD6UPf3eO7hfV7qDWlC4MWyA=";
        private string queueName = @"test";
        private IMessageLoader _loader;
        private IQueueClient queueClient;
        private long messageCounter = 0;
        
        private readonly Stopwatch stopwatch = new Stopwatch();

        public long TotalMessages { get; set; }

        public AzureServiceBus(IMessageLoader loader)
        {
            _loader = loader;        
            queueClient = new QueueClient(connectionString, queueName);
        }       

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            //Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            //Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber}");

            // Complete the message so that it is not received again.
            // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            
            messageCounter++;
            if (messageCounter == TotalMessages)
            {
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("{0} messages received in {1} ms", TotalMessages, elapsedMilliseconds);
                Console.WriteLine("Received {0} per second", elapsedMilliseconds == 0 ? 0L : (1000 * TotalMessages) / elapsedMilliseconds);
            }
            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
            // to avoid unnecessary exceptions.
        }

        // Use this Handler to look at the exceptions received on the MessagePump
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }


        public async Task Receive(Action<T> receiver)
        {
            stopwatch.Start();
            //receiveQueue.ReceiveCompleted += (sender, args) =>
            //{
            //    try
            //    {
            //        var result = args.Message.Body as T;
            //        receiver(result);
            //        receiveQueue.BeginReceive();
            //    }
            //    catch (Exception exception)
            //    {
            //        Console.WriteLine(exception);
            //    }
            //};
            //receiveQueue.BeginReceive();

            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 100,
                 
                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false,   
            };
            
            // Register the function that will process messages
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public async Task Send(T message)
        {
            // Create a new message to send to the queue            
            var messagtobesend = new Message(Encoding.UTF8.GetBytes(Convert.ToString(message)));        

            // Send the message to the queue
            await queueClient.SendAsync(messagtobesend);
        }              
    }
}
