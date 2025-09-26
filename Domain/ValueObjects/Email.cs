using System.Text.RegularExpressions;
using Shared.Exceptions;

namespace Domain.ValueObjects
{
    public readonly record struct Email
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled
        );

        public Email(string value, bool allowToValidate = true)
        {
            Value = value;

            if (!allowToValidate) return;

            if (value == null || !EmailRegex.IsMatch(value))
                throw new IncorrectFormatException(nameof(Email));
        }

        public string Value { get; init; }

        public static implicit operator string(Email email) => email.Value;
        public static implicit operator Email(string val) => new(val);
        public override string ToString() => Value;
    }
}
