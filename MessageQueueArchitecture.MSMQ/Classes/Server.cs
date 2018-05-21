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
    //public class Server : IServer
    //{
    //    private readonly string DEFAULT_QUEUE = @".\Private$\DefaultQueue";
    //    private readonly string DEFAULT_MESSAGE = @"Test Messge";
    //    private readonly int DEFAULT_NUMBER_OF_MESSAGES = 1;

    //    private string _queue;
    //    private string _message;
    //    private int _numberofmessages;

    //    public Server(string queue)
    //    {
    //        _queue = queue;
    //        _message = DEFAULT_MESSAGE;
    //        _numberofmessages = DEFAULT_NUMBER_OF_MESSAGES;
    //    }

    //    public Server(string queue, string message)
    //    {
    //        _queue = queue;
    //        _message =message;
    //    }

    //    public Server(string queue, string message, int numberofmessages)
    //    {
    //        _queue = queue;
    //        _message = message;
    //        _numberofmessages = numberofmessages;
    //    }

    //    public Server()
    //    {
    //        _queue = DEFAULT_QUEUE;
    //        _message = DEFAULT_MESSAGE;
    //        _numberofmessages = DEFAULT_NUMBER_OF_MESSAGES;
    //    }

    //    public void Run()
    //    {
    //        //From Windows Service, use this code
    //        MessageQueue messageQueue = null;
    //      //  if (MessageQueue.Exists(_queue))
    //      //  {
    //            messageQueue = new MessageQueue(_queue);
    //            //messageQueue.Label = "Testing Queue";
    //       // }
    //        //else
    //        //{
    //            //// Create the Queue
    //        //    MessageQueue.Create(_queue);
    //        //    messageQueue = new MessageQueue(_queue);
    //        //    messageQueue.Label = "Test Message";
    //        //}

    //        var startingtime = DateTime.Now;
    //        for (int i = 0; i < _numberofmessages; i++)
    //        {
    //            messageQueue.Send(_message, "Title");
    //            System.Threading.Thread.Sleep(1000);
    //        }
          
    //        Console.WriteLine($"{Thread.CurrentThread.Name}::: Loading Time {(DateTime.Now - startingtime).Milliseconds} Milliseconds");
    //    }
    //}
}
