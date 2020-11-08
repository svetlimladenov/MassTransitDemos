using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Contracts
{
    public interface GetOrderInfo
    {
        Guid OrderId { get; set; }
    }
}
