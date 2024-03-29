﻿using System.Collections.Generic;

namespace JobProcessing.Abstractions
{
    public sealed class StreamId : ValueObject
    {
        private readonly string _id;

        private StreamId(string id)
        {
            _id = id;
        }
        
        public static StreamId AssembleFor<T>(params string[] parts) where T : IStream =>
            new($"{typeof(T).Name}-{string.Join("|", parts)}");

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _id;
        }

        public override string ToString() => _id;

        public static implicit operator string(StreamId streamId) => streamId.ToString();
    }
}