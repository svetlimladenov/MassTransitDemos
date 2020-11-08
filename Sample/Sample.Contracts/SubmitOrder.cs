using System;

namespace Sample.Contracts
{
    public interface SubmitOrder
    {
        public Guid OrderId { get; set; }

        public string CustomerNumber { get; set; }
    }
}
