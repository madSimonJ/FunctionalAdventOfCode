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

    }
}
