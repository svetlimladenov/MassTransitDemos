namespace Library.Components.StateMachines
{
    using Automatonymous;
    using Automatonymous.Binders;
    using MassTransit;
    using Library.Contracts;

    public class BookStateMachine : MassTransitStateMachine<Book>
    {
        static BookStateMachine()
        {
            MessageContracts.Initialize();
        }

        public BookStateMachine()
        {
            InstanceState(x => x.CurrentState, Available, Reserved);

            // Not really needed since added in global topology
            Event(() => Added, x => x.CorrelateById(m => m.Message.BookId));

            Event(() => ReservationRequested, x => x.CorrelateById(m => m.Message.BookId));

            Initially(
                When(Added)
                    .CopyDataToInstance()
                    .TransitionTo(Available)
            );

            During(Available,
                When(ReservationRequested)
                    .PublishAsync(context => context.Init<BookReserved>(new
                    {
                        context.Data.ReservationId,
                        InVar.Timestamp,
                        context.Data.MemberId,
                        context.Data.BookId
                    }))
            );
        }

        public Event<BookAdded> Added { get; set;}

        public Event<ReservationRequested> ReservationRequested { get; set; }
        
        public State Available { get; set; }

        public State Reserved { get; }
    }

    public static class BookStateMachineExtensions
    {
        public static EventActivityBinder<Book, BookAdded> CopyDataToInstance(this EventActivityBinder<Book, BookAdded> binder)
        {
            return binder.Then(x =>
            {
                x.Instance.DateAdded = x.Data.Timestamp.Date;
                x.Instance.Title = x.Data.Title;
                x.Instance.Isbn = x.Data.Isbn;
            });
        }
    }
}