using MessageQueueArchitecture.Commons.Interfaces;
using MessageQueueArchitecture.Commons.Messages;
using MessageQueueArchitecture.MSMQ.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.MSMQ
{
    public class Program
    {       
        static void Main(string[] args)
        {
            IMessageLoader _loader = new MessageLoader();

            const long numberOfMessages = 500;

            var msmq = new MSMQ<string>(_loader);

            new PerfomanceTest(msmq, msmq).Run(500, numberOfMessages);

            Console.ReadKey();
        }            
    }
}
