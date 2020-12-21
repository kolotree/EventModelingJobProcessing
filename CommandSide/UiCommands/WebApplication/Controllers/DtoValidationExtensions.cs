using System;
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    internal static class DtoValidationExtensions
    {
        public static string TryUnwrap(this string? nullableString, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(nullableString))
            {
                throw new BadRequestException($"Invalid input for parameter '{parameterName}'.");
            }

            return nullableString;
        }
        
        public static IReadOnlyList<DateTime> TryUnwrap(this IReadOnlyList<DateTime>? nullableDateTimes, string parameterName)
        {
            if (nullableDateTimes == null)
            {
                throw new BadRequestException($"Invalid input for parameter '{parameterName}'.");
            }

            return nullableDateTimes;
        }
    }

    internal class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}