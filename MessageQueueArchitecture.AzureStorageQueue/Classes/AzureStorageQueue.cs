using MessageQueueArchitecture.Commons.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.AzureStorageQueue.Classes
{
    public class AzureStorageQueue<T> : IClient<T>, IServer<T> where T : class
    {
        public long TotalMessages { get; set; }
        private IMessageLoader _loader;
        private string connectionString = @"Endpoint=sb://fcgpremiun.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZdLOppDD7UNjgdukRLFaRpHMabo4hPU/JLwUsGZWKys=";//@"Endpoint=sb://fcgtest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Bvts8dhzcb7mbKj4PURSD6UPf3eO7hfV7qDWlC4MWyA=";
        private string queueName = @"test";
        private CloudQueue queue;
        private long messageCounter = 0;

        private readonly Stopwatch stopwatch = new Stopwatch();

        public AzureStorageQueue(IMessageLoader loader)
        {
            _loader = loader;

            try
            {
                CreateQueueAsync(queueName).Wait();        
            }
            catch
            {
                Console.WriteLine("If you are running with the default configuration please make sure you have started the storage emulator.  ess the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
                Console.ReadLine();
                throw;
            }
        }

        public async Task Receive(Action<T> receiver)
        {
            stopwatch.Start();

                queue.FetchAttributes();
                int? cachedMessageCount = queue.ApproximateMessageCount;
                messageCounter = (int)cachedMessageCount;

                try
                {
                    foreach (CloudQueueMessage msg in await queue.GetMessagesAsync((int)cachedMessageCount, TimeSpan.FromMinutes(5), null, null))
                    {
                        //Console.WriteLine("Processing & deleting message with content: {0}", msg.AsString);

                        // Process all messages in less than 5 minutes, deleting each message after processing.
                        await queue.DeleteMessageAsync(msg);
                        messageCounter--;

                        if (messageCounter == 0)
                        {
                            stopwatch.Stop();
                            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                            Console.WriteLine("{0} messages received in {1} ms", cachedMessageCount, elapsedMilliseconds);
                            Console.WriteLine("Received {0} per second", cachedMessageCount == 0 ? 0L : (1000 * (long)cachedMessageCount) / (long)elapsedMilliseconds);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.ToString();
                }
            
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public async Task Send(T message)
        {
            await queue.AddMessageAsync(new CloudQueueMessage(Convert.ToString((object)message)));
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
