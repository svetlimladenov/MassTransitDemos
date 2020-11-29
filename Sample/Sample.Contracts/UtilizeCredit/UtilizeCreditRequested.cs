using System;

namespace Sample.Contracts.UtilizeCredit
{
    public interface UtilizeCreditRequested
    {
        CreateCreditDTO CreateCredit { get; }

        BonusPointDTO BonusPoints { get; }
    }
}
