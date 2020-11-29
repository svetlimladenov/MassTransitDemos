namespace Sample.Contracts.UtilizeCredit.CreateCreditEvents
{
    public interface CreateCreditCompleted
    {
        string ExternalId { get; }

        decimal TotalDue { get; }
    }
}