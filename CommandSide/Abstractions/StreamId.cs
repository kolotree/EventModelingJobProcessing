using System;
using System.Collections.Generic;
using Shared;

namespace Abstractions
{
    public sealed class StreamId : ValueObject
    {
        private readonly string _id;

        private StreamId(string id)
        {
            _id = id;
        }
        
        public static StreamId GenerateUnique() => new StreamId(Guid.NewGuid().ToString());

        public static StreamId Assemble(params string[] parts) => 
            new StreamId(string.Join("|", parts));
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _id;
        }

        public override string ToString() => _id;

        public static implicit operator string(StreamId streamId) => streamId.ToString();
    }
}