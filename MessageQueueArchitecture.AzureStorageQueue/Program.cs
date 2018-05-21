using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Queue; // Namespace for Queue storage types
using MessageQueueArchitecture.Commons.Interfaces;
using MessageQueueArchitecture.Commons.Messages;
using MessageQueueArchitecture.AzureStorageQueue.Classes;
using System.Diagnostics;
using System.Configuration;
using System.Threading;

namespace MessageQueueArchitecture.AzureStorageQueue
{
    class Program
    {
        static void Main(string[] args)
        {

            var test = new TestE();            

            Console.ReadKey();            
        }

        /// <summary>
        /// 50000 messages thread safe
        /// </summary>
        public class TestE
        {
            private const long numberOfMessages = 50000;
            private AzureStorageQueue<string> azureStorageQueue;
            private IMessageLoader _loader;
            private CloudQueue queue;
            private string queueName = @"test";
            private long messageCounter = 0;            

            private readonly Stopwatch stopwatch = new Stopwatch();

            public TestE()
            {
                _loader = new MessageLoader();
                
                azureStorageQueue = new AzureStorageQueue<string>(_loader);

                azureStorageQueue.TotalMessages = numberOfMessages;

                Run().Wait();

            }

            public async Task Run()
            {
                await SendMessages();

                await ReceiveMessages();
            }

            public async Task SendMessages()
            {
                var tasks = new List<Task>();

                string message = _loader.GetClaim();

                await CreateQueueAsync(queueName);


                var stopwatch = new Stopwatch();
                stopwatch.Start();

                for (long i = 0; i < numberOfMessages; i++)
                {
                    
                 
                    tasks.Add(Task.Factory.StartNew(MessageToBeSend));
                    //Task.Factory.StartNew(()=> MessageToBeSend());

                    //await queue.AddMessageAsync(new CloudQueueMessage(Convert.ToString((object)message)));
                }

                // Wait for all the tasks to finish.
                Task.WaitAll(tasks.ToArray());

                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("Sent {0} messages in {1} ms", numberOfMessages, elapsedMilliseconds);
                Console.WriteLine("{0} per second", elapsedMilliseconds == 0 ? 0L : (1000 * numberOfMessages) / elapsedMilliseconds);

                Console.WriteLine("Sleeping for 10 seconds before receiving...");
                Thread.Sleep(20000);
                //var sendingMachine = new MessageQueueArchitecture.Commons.Classes.MessageSendingMachine(azureStorageQueue);

                //sendingMachine.RunTest(_loader.GetClaim().Length, numberOfMessages);
            }

            public async Task MessageToBeSend()
            {
                string message = _loader.GetClaim();
                await queue.AddMessageAsync(new CloudQueueMessage(Convert.ToString((object)message)));
            }

            public async Task MessageToBeReceived()
            {
                CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();

                // Async delete the message
                await queue.DeleteMessageAsync(retrievedMessage);
            }

            public async Task ReceiveMessages()
            {
                stopwatch.Restart();
                stopwatch.Start();
                var tasks = new List<Task>();
                queue.FetchAttributes();
                int? cachedMessageCount = queue.ApproximateMessageCount;
                messageCounter = (int)cachedMessageCount;

                Console.WriteLine("{0} messages to be collected", messageCounter);


                try
                {
                    for (int i = 0; i < cachedMessageCount; i++)
                    {

                        //CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();                      

                        // Async delete the message
                        //await queue.DeleteMessageAsync(retrievedMessage);
                        //Task.Factory.StartNew(() => MessageToBeReceived());

                        messageCounter--;

                        tasks.Add(Task.Factory.StartNew(() => MessageToBeReceived()));
                        /*if (messageCounter == 0)
                        {
                            stopwatch.Stop();
                            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                            Console.WriteLine("{0} messages received in {1} ms", cachedMessageCount, elapsedMilliseconds);
                            Console.WriteLine("Received {0} per second", cachedMessageCount == 0 ? 0L : (1000 * (long)cachedMessageCount) / (long)elapsedMilliseconds);
                        }*/
                    }

                    Task.WaitAll(tasks.ToArray());

                        stopwatch.Stop();
                        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                        Console.WriteLine("{0} messages received in {1} ms", cachedMessageCount, elapsedMilliseconds);
                        Console.WriteLine("Received {0} per second", cachedMessageCount == 0 ? 0L : (1000 * (long)cachedMessageCount) / (long)elapsedMilliseconds);
                    
                    //foreach (CloudQueueMessage msg in await queue.GetMessagesAsync((int)cachedMessageCount, TimeSpan.FromMinutes(5), null, null))
                    //{
                    //    //Console.WriteLine("Processing & deleting message with content: {0}", msg.AsString);

                    //    // Process all messages in less than 5 minutes, deleting each message after processing.
                    //    await queue.DeleteMessageAsync(msg);
                    //    messageCounter--;

                    //    if (messageCounter == 0)
                    //    {
                    //        stopwatch.Stop();
                    //        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    //        Console.WriteLine("{0} messages received in {1} ms", cachedMessageCount, elapsedMilliseconds);
                    //        Console.WriteLine("Received {0} per second", cachedMessageCount == 0 ? 0L : (1000 * (long)cachedMessageCount) / (long)elapsedMilliseconds);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

            public async Task<CloudQueue> CreateQueueAsync(string queueName)
            {
                // Retrieve storage account information from connection string.
                CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create a queue client for interacting with the queue service
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                queue = queueClient.GetQueueReference(queueName);
                try
                {
                    await queue.CreateIfNotExistsAsync();
                }
                catch
                {
                    Console.WriteLine("If you are running with the default configuration please make sure you have started the storage emulator.  ess the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
                    Console.ReadLine();
                    throw;
                }

                return queue;
            }

            /// <summary>
            /// Validate the connection string information in app.config and throws an exception if it looks like 
            /// the user hasn't updated this to valid values. 
            /// </summary>
            /// <param name="storageConnectionString">The storage connection string</param>
            /// <returns>CloudStorageAccount object</returns>
            public static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
            {
                CloudStorageAccount storageAccount;
                try
                {
                    storageAccount = CloudStorageAccount.Parse(storageConnectionString);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                    Console.ReadLine();
                    throw;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                    Console.ReadLine();
                    throw;
                }

                return storageAccount;
            }

            public static void WriteException(Exception ex)
            {
                Console.WriteLine("Exception thrown. {0}, msg = {1}", ex.Source, ex.Message);
            }
        }

        /// <summary>
        /// 500 messages thread safe 
        /// </summary>
        public class TestD
        {
            private const long numberOfMessages = 500;
            private AzureStorageQueue<string> azureStorageQueue;
            private IMessageLoader _loader;
            private CloudQueue queue;
            private string queueName = @"test";
            private long messageCounter = 0;

            private readonly Stopwatch stopwatch = new Stopwatch();

            public TestD()
            {
                _loader = new MessageLoader();

                azureStorageQueue = new AzureStorageQueue<string>(_loader);

                azureStorageQueue.TotalMessages = numberOfMessages;

                Run().Wait();

            }

            public async Task Run()
            {
                await SendMessages();

                await ReceiveMessages();
            }

            public async Task SendMessages()
            {
                var tasks = new List<Task>();

                string message = _loader.GetClaim();

                await CreateQueueAsync(queueName);


                var stopwatch = new Stopwatch();
                stopwatch.Start();

                for (long i = 0; i < numberOfMessages; i++)
                {


                    tasks.Add(Task.Factory.StartNew(MessageToBeSend));
                    //Task.Factory.StartNew(()=> MessageToBeSend());

                    //await queue.AddMessageAsync(new CloudQueueMessage(Convert.ToString((object)message)));
                }

                // Wait for all the tasks to finish.
                Task.WaitAll(tasks.ToArray());

                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("Sent {0} messages in {1} ms", numberOfMessages, elapsedMilliseconds);
                Console.WriteLine("{0} per second", elapsedMilliseconds == 0 ? 0L : (1000 * numberOfMessages) / elapsedMilliseconds);

                Console.WriteLine("Sleeping for 10 seconds before receiving...");
                Thread.Sleep(20000);
                //var sendingMachine = new MessageQueueArchitecture.Commons.Classes.MessageSendingMachine(azureStorageQueue);

                //sendingMachine.RunTest(_loader.GetClaim().Length, numberOfMessages);
            }

            public async Task MessageToBeSend()
            {
                string message = _loader.GetClaim();
                await queue.AddMessageAsync(new CloudQueueMessage(Convert.ToString((object)message)));
            }

            public async Task MessageToBeReceived()
            {
                CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();

                // Async delete the message
                await queue.DeleteMessageAsync(retrievedMessage);
            }

            public async Task ReceiveMessages()
            {
                stopwatch.Restart();
                stopwatch.Start();
                var tasks = new List<Task>();
                queue.FetchAttributes();
                int? cachedMessageCount = queue.ApproximateMessageCount;
                messageCounter = (int)cachedMessageCount;

                Console.WriteLine("{0} messages to be collected", messageCounter);


                try
                {
                    for (int i = 0; i < cachedMessageCount; i++)
                    {

                        //CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();                      

                        // Async delete the message
                        //await queue.DeleteMessageAsync(retrievedMessage);
                        //Task.Factory.StartNew(() => MessageToBeReceived());

                        messageCounter--;

                        tasks.Add(Task.Factory.StartNew(() => MessageToBeReceived()));
                        /*if (messageCounter == 0)
                        {
                            stopwatch.Stop();
                            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                            Console.WriteLine("{0} messages received in {1} ms", cachedMessageCount, elapsedMilliseconds);
                            Console.WriteLine("Received {0} per second", cachedMessageCount == 0 ? 0L : (1000 * (long)cachedMessageCount) / (long)elapsedMilliseconds);
                        }*/
                    }

                    Task.WaitAll(tasks.ToArray());

                    stopwatch.Stop();
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine("{0} messages received in {1} ms", cachedMessageCount, elapsedMilliseconds);
                    Console.WriteLine("Received {0} per second", cachedMessageCount == 0 ? 0L : (1000 * (long)cachedMessageCount) / (long)elapsedMilliseconds);

                    //foreach (CloudQueueMessage msg in await queue.GetMessagesAsync((int)cachedMessageCount, TimeSpan.FromMinutes(5), null, null))
                    //{
                    //    //Console.WriteLine("Processing & deleting message with content: {0}", msg.AsString);

                    //    // Process all messages in less than 5 minutes, deleting each message after processing.
                    //    await queue.DeleteMessageAsync(msg);
                    //    messageCounter--;

                    //    if (messageCounter == 0)
                    //    {
                    //        stopwatch.Stop();
                    //        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    //        Console.WriteLine("{0} messages received in {1} ms", cachedMessageCount, elapsedMilliseconds);
                    //        Console.WriteLine("Received {0} per second", cachedMessageCount == 0 ? 0L : (1000 * (long)cachedMessageCount) / (long)elapsedMilliseconds);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

            public async Task<CloudQueue> CreateQueueAsync(string queueName)
            {
                // Retrieve storage account information from connection string.
                CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create a queue client for interacting with the queue service
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                queue = queueClient.GetQueueReference(queueName);
                try
                {
                    await queue.CreateIfNotExistsAsync();
                }
                catch
                {
                    Console.WriteLine("If you are running with the default configuration please make sure you have started the storage emulator.  ess the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
                    Console.ReadLine();
                    throw;
                }

                return queue;
            }

            /// <summary>
            /// Validate the connection string information in app.config and throws an exception if it looks like 
            /// the user hasn't updated this to valid values. 
            /// </summary>
            /// <param name="storageConnectionString">The storage connection string</param>
            /// <returns>CloudStorageAccount object</returns>
            public static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
            {
                CloudStorageAccount storageAccount;
                try
                {
                    storageAccount = CloudStorageAccount.Parse(storageConnectionString);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                    Console.ReadLine();
                    throw;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                    Console.ReadLine();
                    throw;
                }

                return storageAccount;
            }

            public static void WriteException(Exception ex)
            {
                Console.WriteLine("Exception thrown. {0}, msg = {1}", ex.Source, ex.Message);
            }
        }
    }
}
