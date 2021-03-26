using System;
using JobProcessing.Abstractions;

namespace JobProcessing.Infrastructure
{
    public sealed class StandardDateTimeProvider : IDateTimeProvider
    {
        public DateTime CurrentUtcDateTime => DateTime.UtcNow;
    }
}