﻿namespace Utilities.Extensions
{
    public static class BoolExtensions
    {
        public static int ToInt(this bool value) => 
            value ? 1 : 0;
    }
}