using Automatonymous;
using MassTransit;
using Sample.Contracts.UtilizeCredit;
using Sample.Contracts.UtilizeCredit.CreateCreditEvents;
using Sample.Contracts.UtilizeCredit.GetCreditUtilizationInfo;
using System;

namespace Sample.Service.Sagas
{
    public class UtilizeCreditStateMachine : MassTransitStateMachine<UtilizeCredit>
    {
        public UtilizeCreditStateMachine()
        {
            // Declares the property to hold the instance's state as a string (the state name is stored in the property)
            InstanceState(x => x.CurrentState);

            Event(() => UtilizeCreditRequested, x => 
            {
                // i - UtilizeCreditState (the state machine instance class)
                // m - the current event message
                x.CorrelateBy(i => i.ExternalId, m => m.Message.CreateCredit.ExternalId);

                x.SelectId(ctx => NewId.NextGuid());

                x.SetSagaFactory(ctx => new UtilizeCredit()
                {
                    CorrelationId = (Guid)ctx.CorrelationId,
                    ExternalId = ctx.Message.CreateCredit.ExternalId,
                    CurrentState = Initial.Name,
                    ResponseAddress = ctx.ResponseAddress,
                });
            });

            Event(() => GetCreditUtilizationInfo, x =>
            {
                x.CorrelateBy(i => i.ExternalId, m => m.Message.ExternalId);

                x.OnMissingInstance(m => m.ExecuteAsync(x => x.RespondAsync<CreditUtilizationInfo>(new
                {
                    State = "Not found"
                })));
            });

            Event(() => CreateCreditCompleted, x => x.CorrelateBy(i => i.ExternalId, m => m.Message.ExternalId));
            Event(() => CreateCreditFaulted, x => x.CorrelateBy(i => i.ExternalId, m => m.Message.ExternalId));

            Initially(
                When(UtilizeCreditRequested)
                    .PublishAsync(context => context.Init<CreateCredit>(new 
                    {
                        context.Data.CreateCredit
                    }))
                    .TransitionTo(Utilizing));

            DuringAny(
                When(GetCreditUtilizationInfo)
                    .RespondAsync(context => context.Init<CreditUtilizationInfo>(new
                    {
                        State = context.Instance.CurrentState
                    })));

            During(Utilizing,
                When(CreateCreditCompleted)
                    .TransitionTo(CreditCreated),
                When(CreateCreditFaulted)
                    .SendAsync(
                        ctx => ctx.Instance.ResponseAddress,
                        ctx => ctx.Init<UtilizeCreditFaulted>(new 
                        {
                            ctx.Data.ValidationError
                        })) 
                    .Finalize());

            // Sets the state machine instance to Completed when in the final state. The saga repository removes completed state machine instances.
            SetCompletedWhenFinalized();
        }

        public Event<UtilizeCreditRequested> UtilizeCreditRequested { get; set; }

        public Event<GetCreditUtilizationInfo> GetCreditUtilizationInfo { get; set; }

        public Event<CreateCreditCompleted> CreateCreditCompleted { get; set; }

        public Event<CreateCreditFaulted> CreateCreditFaulted { get; set; }

        public State Utilizing { get; private set; } 

        public State CreditCreated { get; private set; }
    }
}
