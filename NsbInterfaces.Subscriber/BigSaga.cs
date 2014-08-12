using System;
using NsbInterfaces.Events.Publisher;
using NsbInterfaces.Events.Subscriber;
using NsbInterfaces.Messages.Publisher;
using NServiceBus;
using NServiceBus.Saga;

namespace NsbInterfaces.Subscriber
{
    public class BigSaga : Saga<BigSagaData>,
        IAmStartedByMessages<ISomeEvent>,
        IHandleMessages<IPong>

    {
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<ISomeEvent>(ev => ev.Identifier).ToSaga(s => s.Id);
            ConfigureMapping<IPong>(ev => ev.Identifier).ToSaga(s => s.Id);
        }

        public void Handle(ISomeEvent message)
        {
            Bus.Publish<IPing>(x => x.Identifier = message.Identifier);
        }

        public void Handle(IPong message)
        {
            Console.WriteLine("Pong");
        }
    }

    public class BigSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
    }
}