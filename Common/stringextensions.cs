using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace Common
{
    public static class stringextensions
    {
        public static int ToInt(this string @this) =>
            int.TryParse(@this, out var output)
                ? output
                : 0;

        public static short ToShort(this string @this) =>
            short.TryParse(@this, out var output)
                ? output
                : (short)0;

        public static ushort ToUShort(this string @this) =>
            ushort.TryParse(@this, out var output)
                ? output
                : (ushort)0;

        public static uint ToUInt(this string @this) =>
            uint.TryParse(@this, out var output)
                ? output
                : (uint)0;


        public static int CountOf(this string @this, params char[] characterList) =>
            @this.Count(x => characterList.Contains(x));

        public static bool IsNumeric(this string @this) => float.TryParse(@this, out var _);

        public static string RemoveWhitespace(this string @this) => 
            Regex.Replace(@this, @"\s+", "");
    }
}
