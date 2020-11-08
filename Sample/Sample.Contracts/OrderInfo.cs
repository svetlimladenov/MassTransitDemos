using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Contracts
{
    public interface OrderInfo
    {
        Guid OrderId { get; set; }

        DateTime OrderedOn { get; set; }

        string Location { get; set; }
    }
}
