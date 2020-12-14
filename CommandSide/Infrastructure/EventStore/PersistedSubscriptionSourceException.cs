using System;

namespace Infrastructure.EventStore
{
    internal sealed class PersistedSubscriptionSourceException : Exception
    {
        public PersistedSubscriptionSourceException(Exception innerException) 
            : base("Error while processing subscription. See inner exception for more details.", innerException)
        {   
        }
    }
}