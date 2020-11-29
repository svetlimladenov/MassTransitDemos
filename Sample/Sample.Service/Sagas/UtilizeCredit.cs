using Automatonymous;
using System;

namespace Sample.Service.Sagas
{
    public class UtilizeCredit : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
            
        public string ExternalId { get; set; }
     
        public string CurrentState { get; set; }

        public Uri ResponseAddress { get; set; }
    }
}
