namespace Library.Contracts
{
    using System;

    public interface ReservationRequested
    {
        Guid ReservationId { get; set; }

        DateTime Timestamp { get; }

        Guid MemberId { get; }

        Guid BookId { get; }
    }
}