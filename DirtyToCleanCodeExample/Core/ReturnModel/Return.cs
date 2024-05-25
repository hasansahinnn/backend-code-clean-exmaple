using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ReturnModel
{
    public record Return<T> : IReturn<T>
    {
        public T? Data { get; init; }
        public bool Status { get; init; }
        public string? Message { get; init; }
        public Exception? Exception { get; init; }

        public Return(bool Status, T? Data, string? Message, Exception? Exception)
        {
            this.Data = Data;
            this.Status = Status;
            this.Message = Message;
            this.Exception = Exception;
        }
    }
    public record Return : IReturn
    {
        public bool Status { get; init; }
        public string? Message { get; init; }
        public Exception? Exception { get; init; }

        public Return(bool Status, string? Message, Exception? Exception)
        {
            this.Status = Status;
            this.Message = Message;
            this.Exception = Exception;
        }
    }
}
