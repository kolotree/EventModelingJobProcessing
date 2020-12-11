using System;

    namespace Abstractions
{
    public struct Optional<T> : IEquatable<Optional<T>>
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (HasNoValue)
                    throw new InvalidOperationException();

                return _value;
            }
        }

        public static Optional<T> None => new Optional<T>(default(T));

        public bool HasValue => _value != null;

        public bool HasNoValue => !HasValue;

        private Optional(T value)
        {
            _value = value;
        }

        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

        public static Optional<T> From(T obj)
        {
            return new Optional<T>(obj);
        }

        public static bool operator ==(Optional<T> optional, T value)
        {
            if (optional.HasNoValue)
                return false;

            return optional.Value.Equals(value);
        }

        public static bool operator !=(Optional<T> optional, T value)
        {
            return !(optional == value);
        }

        public static bool operator ==(Optional<T> a, Optional<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Optional<T> a, Optional<T> b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is T)
            {
                obj = new Optional<T>((T) obj);
            }

            if (!(obj is Optional<T>))
                return false;

            var other = (Optional<T>) obj;
            return Equals(other);
        }

        public bool Equals(Optional<T> other)
        {
            if (HasNoValue && other.HasNoValue)
                return true;

            if (HasNoValue || other.HasNoValue)
                return false;

            return _value.Equals(other._value);
        }

        public override int GetHashCode()
        {
            if (HasNoValue)
                return 0;

            return _value.GetHashCode();
        }

        public override string ToString()
        {
            if (HasNoValue)
                return "No value";

            return Value.ToString();
        }
    }
}