namespace Sample.Contracts.UtilizeCredit.CreateCreditEvents
{
    public interface CreateCreditFaulted
    {
        string ExternalId { get; }

        string ValidationError { get; }
    }
}