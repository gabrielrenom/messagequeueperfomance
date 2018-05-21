using MessageQueueArchitecture.Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.MSMQ.Classes
{
    //public class Client: IClient
    //{
    //    private readonly string DEFAULT_QUEUE = @".\Private$\DefaultQueue";
    //    private string _queue;

    //    public Client(string queue) => _queue = queue;
        
    //    public Client() => _queue = DEFAULT_QUEUE;
        
    //    public void Run()
    //    {
    //        //if (!MessageQueue.Exists(_queue))
    //        //{
    //        //    MessageQueue.Create(_queue);
    //       // }
            
    //        //From Windows application
    //        MessageQueue messageQueue = new MessageQueue(_queue);
            
    //        messageQueue.Send("<hello>21323</hello>", "Title");
    //        messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

    //        System.Messaging.Message msgTxn = messageQueue.Receive(MessageQueueTransactionType.Single);



    //        System.Messaging.Message[] messages = messageQueue.GetAllMessages();            
            
    //        var startingtime = DateTime.Now;
    //        foreach (System.Messaging.Message message in messages)
    //        {
    //            System.Threading.Thread.Sleep(1);                                
    //        }
    //        Console.WriteLine($"{Thread.CurrentThread.Name}::: Processing Time {(DateTime.Now - startingtime).Milliseconds} Milliseconds");

    //        // after all processing, delete all the messages
    //        messageQueue.Purge();
    //    }
    //}
}
