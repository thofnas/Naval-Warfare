using System;

namespace Misc
{
    public struct NonEmptyString
    {
        private readonly string _value;

        private NonEmptyString(string value)
        {
            _value = !string.IsNullOrWhiteSpace(value) 
                ? value.Trim() 
                : throw new ArgumentException("Value cannot be null or white space", nameof(value));
        }
        
        public static implicit operator NonEmptyString(string value) => new(value);
        public static implicit operator string(NonEmptyString value) => value._value;
    }
}