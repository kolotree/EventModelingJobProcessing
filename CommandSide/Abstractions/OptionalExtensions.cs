using System;
using System.Collections.Generic;
using System.Linq;

namespace Abstractions
{
    public static class OptionalExtensions
    {
        public static Optional<K> Map<T, K>(this Optional<T> maybe, Func<T, K> transformer)
        {
            if (maybe.HasValue)
            {
                return Optional<K>.From(transformer(maybe.Value));
            }

            return Optional<K>.None;
        }
        
        public static K Unwrap<T, K>(this Optional<T> maybe, Func<T, K> transformer, Func<K> defaultValueProvider)
        {
            if (maybe.HasValue)
            {
                return transformer(maybe.Value);
            }

            return defaultValueProvider();
        }
        
        public static T Unwrap<T>(this Optional<T> optional)
        {
            if (optional.HasValue)
            {
                return optional.Value;
            }

            return default;
        }
        
        public static T Unwrap<T>(this Optional<T> optional, T defaultValue)
        {
            if (optional.HasValue)
            {
                return optional.Value;
            }

            return defaultValue;
        }
        
        public static Optional<K> FlatMap<T, K>(this Optional<T> maybe, Func<T, Optional<K>> transformer)
        {
            if (maybe.HasValue)
            {
                return transformer(maybe.Value);
            }

            return Optional<K>.None;
        }

        public static Optional<T> OptionalFirst<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.FirstOrDefault();
        }
        
        public static Optional<T> Ensure<T>(this Optional<T> optional, Func<Optional<T>, bool> predicate, Action onErrorAction)
        {
            if (!predicate(optional))
            {
                onErrorAction();
            }

            return optional;
        }

        public static Optional<T> ToOptional<T>(this T obj) => obj;
    }
}