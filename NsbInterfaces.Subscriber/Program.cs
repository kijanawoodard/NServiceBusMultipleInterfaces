using Ninject;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Saga;

namespace NsbInterfaces.Subscriber
{
	class Program
	{
		static void Main(string[] args)
		{
			var kernel = new StandardKernel();

			Configure.Features.Disable<NServiceBus.Features.TimeoutManager>().Disable<NServiceBus.Features.SecondLevelRetries>();
		    Configure.Features.Enable<NServiceBus.Features.Sagas>();
			Configure.With()
				.DefineEndpointName("nsbinterfaces.subscriber")
				.DefiningEventsAs(t => t.Namespace != null && t.Namespace.Contains(".Events"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.Contains("NsbInterfaces.Messages"))
				//.NinjectBuilder(kernel)
				.DefaultBuilder()
				.Log4Net()
                .UseTransport<Msmq>()
					.PurgeOnStartup(false)
				.MsmqSubscriptionStorage("nsbinterfaces.subscriber")
				.UnicastBus()
					.LoadMessageHandlers()
					.ImpersonateSender(false)
				.CreateBus()
				.Start(
					() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());


			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
