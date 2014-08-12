using System;

namespace NsbInterfaces.Events.Publisher
{
	public interface ISomeEvent : ISomeInterface, ISomeOtherInterface
	{
	    Guid Identifier { get; set; }
	}
}
