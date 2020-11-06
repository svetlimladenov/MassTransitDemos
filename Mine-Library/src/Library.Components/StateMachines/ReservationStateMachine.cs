namespace Library.Components.StateMachines
{
    using Automatonymous;
    using Library.Contracts;

    public class ReservationStateMachine : MassTransitStateMachine<Reservation>
    {
        static ReservationStateMachine()
        {
            MessageContracts.Initialize();
        }

        public ReservationStateMachine()
        {
            InstanceState(x => x.CurrentState, Requested);

            // Not really needed since added in global topology
            Event(() => ReservationRequested, x => x.CorrelateById(m => m.Message.ReservationId));

            Event(() => BookReserved, x => x.CorrelateById(m => m.Message.ReservationId));

            Initially(
                When(ReservationRequested)
                    .Then(x =>
                    {
                        x.Instance.BookId = x.Data.BookId;
                        x.Instance.MemberId = x.Data.MemberId;
                        x.Instance.Created = x.Data.Timestamp;
                    })
                    .TransitionTo(Requested)
            );

            During(Requested,
                When(BookReserved)
                    .Then(context => context.Instance.Reserved = context.Data.Timestamp)
                    .TransitionTo(Reserved)
            );
        }
         
        public State Requested { get; }

        public State Reserved { get; set; }

        public Event<ReservationRequested> ReservationRequested { get; }

        public Event<BookReserved> BookReserved { get; set; }
    }
}