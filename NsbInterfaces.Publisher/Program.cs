using NsbInterfaces.Events.Publisher;
using NsbInterfaces.Events.Subscriber;
using NServiceBus;
using System;
using NServiceBus.Saga;

namespace NsbInterfaces.Publisher
{
	class Program
	{
		static void Main(string[] args)
		{
			Configure.Features.Disable<NServiceBus.Features.SecondLevelRetries>();
            Configure.Features.Enable<NServiceBus.Features.Sagas>();
			var bus = Configure.With()
				.DefineEndpointName("nsbinterfaces.publisher")
				.DefiningEventsAs(t => t.Namespace != null && t.Namespace.Contains(".Events"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.Contains("NsbInterfaces.Messages"))
				.DefaultBuilder()
                .Log4Net()
                .UseTransport<Msmq>()
					.PurgeOnStartup(false)
				.MsmqSubscriptionStorage("nsbinterfaces.publisher")
				.UnicastBus()
					.LoadMessageHandlers()
					.ImpersonateSender(false)
				.EnablePerformanceCounters()
				.CreateBus()
				.Start(
					() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

            bus.Publish<ISomeEvent>(@event => { @event.Identifier = Guid.NewGuid(); });

			Console.WriteLine("Press esc to exit, or any other key to send an event");
			while(Console.ReadKey().Key != ConsoleKey.Escape)
			{
                bus.Publish<ISomeEvent>(@event => { @event.Identifier = Guid.NewGuid(); });
			}
		}
	}

    public class LittleSaga : Saga<LittleSagaData>,
        IAmStartedByMessages<IPing>
    {
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<IPing>(ev => ev.Identifier).ToSaga(s => s.Id);
        }

        public void Handle(IPing message)
        {
            Console.WriteLine("Ping");
            Bus.Reply<IPong>(pong => { pong.Identifier = message.Identifier; });
            MarkAsComplete();
        }
    }

    public class LittleSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
    }
}
