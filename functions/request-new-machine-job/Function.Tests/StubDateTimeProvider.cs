using System;
using JobProcessing.Abstractions;

namespace Function.Tests
{
    internal sealed class StubDateTimeProvider : IDateTimeProvider
    {
        private StubDateTimeProvider(DateTime currentUtcDateTime)
        {
            CurrentUtcDateTime = currentUtcDateTime;
        }

        public static IDateTimeProvider Today = new StubDateTimeProvider(DateTime.Now);
        public static IDateTimeProvider Yesterday = new StubDateTimeProvider(Today.CurrentUtcDateTime.AddDays(-1));

        public DateTime CurrentUtcDateTime { get; }
    }
}