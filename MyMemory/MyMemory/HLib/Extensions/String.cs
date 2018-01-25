﻿


namespace HLib.Extensions
{

    using System;


    public static class String
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
