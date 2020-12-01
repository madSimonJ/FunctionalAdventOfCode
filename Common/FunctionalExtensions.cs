using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Common
{
    public static class FunctionalExtensions
    {
        public static TOutput Map<TInput, TOutput>(this TInput @this, Func<TInput, TOutput> f) =>
            f(@this);

        public static bool Validate<T>(this T @this, params Func<T, bool>[] predicates) =>
            predicates.All(x => x(@this));

        public static bool Eq<T>(this T @this, T OtherThing) =>
            EqualityComparer<T>.Default.Equals(@this, OtherThing);

        public static bool NotEq<T>(this T @this, T OtherThing) =>
            !@this.Eq(OtherThing);

        public static T ApplyXTimes<T>(this T seed, int times, Func<T, T> f)
        {
            var r = seed;
            for (int i = 0; i < times; i++)
            {
                r = f(r);
            }
            return r;
        }

    }
}
