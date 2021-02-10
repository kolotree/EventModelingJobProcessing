using System.Collections.Generic;
using JobProcessing.Abstractions;

namespace Function
{
    public sealed class FunctionResult : ValueObject
    {
        public int StatusCode { get; }
        public string? Value { get; }
        public string? Error { get; }
        
        public bool IsSuccess => Value != null;
        public bool IsFailure => !IsSuccess;

        private FunctionResult(int statusCode, string? value, string? error)
        {
            StatusCode = statusCode;
            Value = value;
            Error = error;
        }
        
        public static readonly FunctionResult Success = new(200, string.Empty, null);

        public static FunctionResult SuccessWith(string value) => new(200, value, null);

        public static FunctionResult BadRequestFailureWith(string error) => new(400, null, error);
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value ?? "";
            yield return Error ?? "";
        }

        public override string ToString() =>
            IsSuccess
                ? $"{nameof(StatusCode)}: {StatusCode}, {nameof(Value)}: {Value}"
                : $"{nameof(StatusCode)}: {StatusCode}, {nameof(Error)}: {Error}";
    }
}