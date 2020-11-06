using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class stringextensions
    {
        public static int ToInt(this string @this) =>
            int.TryParse(@this, out var output)
                ? output
                : 0;

        public static int CountOf(this string @this, params char[] characterList) =>
            @this.Count(x => characterList.Contains(x));
    }
}
