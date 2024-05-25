using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ReturnModel
{
    public sealed record SuccessReturn<T> : Return<T>
    {
        public SuccessReturn() : base(true, default, null, null) { }
        public SuccessReturn(string? Message) : base(true, default, Message, null) { }
        public SuccessReturn(T? Data) : base(true, Data, null, null) { }
        public SuccessReturn(string? Message, T? Data) : base(true, Data, Message, null) { }
    }

    public sealed record SuccessReturn : Return
    {
        public SuccessReturn() : base(true, null, null) { }
        public SuccessReturn(string? Message) : base(true, Message, null) { }
    }
}
