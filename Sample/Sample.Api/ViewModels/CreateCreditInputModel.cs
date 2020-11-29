using System;

namespace Sample.Api.ViewModels
{
    public class CreateCreditInputModel
    {
        public string ExternalId { get; set; }

        public decimal Sum { get; set; }

        public DateTime FirstPaymentDate { get; set; }

        public DateTime UtilizationDate { get; set; }
    }
}
