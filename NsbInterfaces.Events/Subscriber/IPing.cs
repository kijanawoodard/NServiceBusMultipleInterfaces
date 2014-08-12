using System;

namespace NsbInterfaces.Events.Subscriber
{
    public interface IPing
    {
        Guid Identifier { get; set; }
    }
}