using System;

namespace NsbInterfaces.Events.Publisher
{
    public interface IPong
    {
        Guid Identifier { get; set; }
    }
}