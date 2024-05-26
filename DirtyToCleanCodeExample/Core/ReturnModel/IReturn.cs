using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ReturnModel
{
    public interface IReturn<T> : IReturn
    {
        public T? Data { get; init; }
    }

    public interface IReturn
    {
        public bool Status { get; init; }
        public string? Message { get; init; }
        public Exception? Exception { get; init; }
    }
}
