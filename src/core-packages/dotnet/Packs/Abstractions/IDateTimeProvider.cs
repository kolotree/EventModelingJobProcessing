using System;

namespace JobProcessing.Abstractions
{
    public interface IDateTimeProvider
    {
        DateTime CurrentUtcDateTime { get; }
    }
}