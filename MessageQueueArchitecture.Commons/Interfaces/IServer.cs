using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.Commons.Interfaces
{
    public interface IServer<T>
    {
        void Run();
        Task Receive(Action<T> receiver);
        long TotalMessages { get; set; }
    }
}
