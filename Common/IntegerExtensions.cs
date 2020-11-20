using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class IntegerExtensions
    {
        public static bool InRange(this int @this, int start, int end) =>
            @this >= start && @this <= end;

        public static char ToChar(this uint @this) =>
            Convert.ToChar(@this);
    }
}
