using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Contracts
{
    // Response
    public interface OrderSubmissionAccepted
    {
        public Guid OrderId { get; set; }

        DateTime Timestamp { get; }

        public string CustomerNumber { get; set; }
    }
}
