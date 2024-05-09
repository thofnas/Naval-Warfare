using System;

namespace Utilities.Extensions
{
    public static class IntExtensions
    {
        public static bool ToBoolean(this int value) =>
            value switch
            {
                0 => false,
                1 => true,
                _ => throw new ArgumentException($"Value {value} can't be converted to bool", nameof(value))
            };
    }
}