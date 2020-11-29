using System;

namespace Sample.Contracts.UtilizeCredit
{
    public class CreateCreditDTO
    {
        public string ExternalId { get; set; }

        public decimal Sum { get; set; }

        public DateTime UtilizationDate { get; set; }

        public DateTime FirstPaymentDate { get; set; }
    }
}
