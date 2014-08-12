using System;

namespace NsbInterfaces.Messages.Publisher
{
    public interface IPong
    {
        Guid Identifier { get; set; }
    }
}