using System;

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
    }

    internal class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}