using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Contracts
{
    public interface RecalculateCreditMessage
    {
        int CreditId { get; }
    }
}
