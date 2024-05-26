using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ReturnModel
{
    public sealed record ErrorReturn<T> : Return<T>
    {
        public ErrorReturn() : base(false, default, null, null) { }
        public ErrorReturn(string? Message) : base(false, default, Message, null) { }
        public ErrorReturn(T? Data) : base(false, Data, default, null) { }
        public ErrorReturn(string? Message, T? Data) : base(false, Data, Message, null) { }
        public ErrorReturn(Exception? Exception) : base(false, default, null, Exception) { }
        public ErrorReturn(string? Message, Exception? Exception) : base(false, default, Message, Exception) { }
        public ErrorReturn(T? Data, Exception? Exception) : base(false, Data, null, Exception) { }
        public ErrorReturn(string? Message, T? Data, Exception? Exception) : base(false, Data, Message, Exception) { }
    }

    public sealed record ErrorReturn : Return
    {
        public ErrorReturn() : base(false, null, null) { }
        public ErrorReturn(string? Message) : base(false, Message, null) { }
        public ErrorReturn(Exception? Exception) : base(false, null, Exception) { }
        public ErrorReturn(string? Message, Exception? Exception) : base(false, Message, Exception) { }
    }
}
